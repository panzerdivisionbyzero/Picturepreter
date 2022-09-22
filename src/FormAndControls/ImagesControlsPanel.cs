/*
 * This unit is part of Picturepreter.
 * 
 * Licensed under the terms of the GNU GPL 2.0 license,
 * excluding used libraries:
 * - MoonSharp, licensed under MIT license;
 * and used code snippets marked with link to original source.
 * 
 * Copyright(c) 2022 by Paweł Witkowski
 * 
 * pawel.vitek.witkowski@gmail.com 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Picturepreter
{
    /// <summary>
    /// Contains and manages a list of ImageControlsItem objects (sub-panels);
    /// Each ImageControlsItem (except the last one) corresponts to specific FrmMain.sourceImages[] with the same index;
    /// The last imagesControlsItems[] member corrensponds to FrmMain.resultImage;
    /// </summary>
    public class ImagesControlsPanel : Panel
    {
        private int controlAddCount = 0;
        private ImageControlsItem.ImageControlsItemEvents internalEvents; // events assigned to each ImageControlsItem (sub-panel)
        private List<ImageControlsItem> imagesControlsItems = new List<ImageControlsItem>(); // list of sub-panels

        public event EventHandler? OnImageChecked;
        public event EventHandler? OnNewImageControlsAdded;
        public event EventHandler? OnLoadImageButtonClick;
        public event EventHandler? OnImageControlsRemoved;
        public event EventHandler? OnBeforeSwapImageControls;

        /// <summary>
        /// Class constructor; Prepares internalEvents struct and creates first imagesControlsItems[] item (for resultImage)
        /// </summary>
        public ImagesControlsPanel()
        {
            // prepare events for sub-panels:
            internalEvents = new ImageControlsItem.ImageControlsItemEvents(rbSelectImage_CheckedChanged, btnSwitchOrAddImage_Click, btnRemoveImage_Click, btnLoadImage_Click);
            
            // create first sub-panel and adjust it to "result image" panel role:
            AddImageControlsSet(internalEvents);
            imagesControlsItems[0].SetToResultImageRole();
            this.Controls.SetChildIndex(imagesControlsItems[0], 0); // dock order: the last of all panels
        }
        // EVENTS METHODS: ****************************************************************
        /// <summary>
        /// ImageControlsItem rbSelectImage checkedChanged event; Resets other imagesControlsItems[] radio buttons state and calls OnImageChecked() event;
        /// </summary>
        private void rbSelectImage_CheckedChanged(object sender, EventArgs e)
        {
            if (!(sender is RadioButton)) return;

            RadioButton rb = (RadioButton)sender;
            if (rb.Checked) // radio buttons belong to different panels, so it's necessary to manually uncheck others
            {
                foreach (ImageControlsItem panel in imagesControlsItems)
                {
                    if ((int)panel.Tag != (int)rb.Tag)
                    {
                        panel.rbSelectImage.Checked = false;
                    }
                }
            }
            if (OnImageChecked != null) {
                OnImageChecked(sender, e);
            }
        }
        /// <summary>
        /// ImageControlsItem btnSwitchOrAddImage click event; Action depends on button Tag value (interpreted as image index):
        /// - Tag == imagesControlsItems.Count - 1 means resultImage index, which cannot be switched (always at the end of the list; btnSwitchOrAddImage is hidden for this index);
        /// - Tag == imagesControlsItems.Count - 2 means the last source image index; in this case the button performs "Add image" function ("+" button);
        /// - Tag  < imagesControlsItems.Count - 2 means some other source image index; in this case the button performs "Switch image" function ("v" button);
        /// </summary>
        private void btnSwitchOrAddImage_Click(object sender, EventArgs e)
        {
            if (!(sender is Button)) return;

            Button btn = (Button)sender;
            int tagIndex = (int)btn.Tag;
            if (tagIndex > imagesControlsItems.Count - 2) // [index-1] is result image index; [index-2] should have btnAddImage_Click() action;
            {
                return;
            }
            else if (tagIndex == imagesControlsItems.Count - 2) // "+" button
            {
                AddImageControlsSet();
                if (OnNewImageControlsAdded != null)
                {
                    OnNewImageControlsAdded(sender, e);
                }
            }
            else // "v" button
            {
                int checkedPanelIndex = GetCheckedPanelIndex();
                if (OnBeforeSwapImageControls != null) // trigger form event first (with current sender.Tag value)
                {
                    OnBeforeSwapImageControls(sender, e);
                }
                imagesControlsItems.Reverse(tagIndex, 2);
                RefreshControlsTagsAndText(); // reset sender.Tag value
                
                if (checkedPanelIndex != -1) {
                    TryCheckPanelAtIndex(checkedPanelIndex);
                }
            }
        }
        /// <summary>
        /// ImageControlsItem btnRemoveImage click event; initiates removing its ImageControlsItem and calls OnImageControlsRemoved() event;
        /// ImageControlsItem index is read from button Tag property;
        /// </summary>
        private void btnRemoveImage_Click(object sender, EventArgs e)
        {
            if (!(sender is Button) || (imagesControlsItems.Count < 3)) return; // at least one source image panel must remain (keeps btnSwitchOrAddImage with "+" functionality)

            Button btn = (Button)sender;
            int tagIndex = (int)btn.Tag;

            imagesControlsItems[tagIndex].Controls.Clear();
            this.Controls.Remove(imagesControlsItems[tagIndex]);
            imagesControlsItems.RemoveAt(tagIndex);

            RefreshControlsTagsAndText();

            if (OnImageControlsRemoved != null) {
                OnImageControlsRemoved(sender, e);
            }
        }
        /// <summary>
        /// ImageControlsItem btnLoadImage click; calls OnLoadImageButtonClick() event;
        /// Loading image is in responsibility of the FrmMain;
        /// </summary>
        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            if (OnLoadImageButtonClick != null) {
                OnLoadImageButtonClick(sender, e);
            }
        }
        // PUBLIC METHODS: **************************************************************
        /// <summary>
        /// Creates a new ImageControlsItem in imagesControlsItems[] by calling another version of AddImageControlsSet();
        /// </summary>
        public void AddImageControlsSet()
        {
            AddImageControlsSet(internalEvents);
        }
        /// <summary>
        /// Creates a new ImageControlsItem in imagesControlsItems[] and calls RefreshControlsTagsAndText();
        /// </summary>
        /// <param name="events">Events set passed to the new ImageControlsItem controls</param>
        public void AddImageControlsSet(ImageControlsItem.ImageControlsItemEvents events)
        {
            ImageControlsItem newPanel = new ImageControlsItem(controlAddCount++, events);
            newPanel.SuspendLayout();
            newPanel.Size = new System.Drawing.Size(this.Width - 10, ImageControlsItem.buttonHeight + 2 * ImageControlsItem.buttonMargin);
            newPanel.Location = new System.Drawing.Point(0, 0);
            newPanel.Dock = DockStyle.Top;
            newPanel.SetControlsTags(imagesControlsItems.Count);
            int index = Math.Max(0, imagesControlsItems.Count - 1);
            imagesControlsItems.Insert(index, newPanel);
            this.Controls.Add(newPanel);

            newPanel.ResumeLayout(true);
            RefreshControlsTagsAndText();
        }
        /// <summary>
        /// Returns imagesControlsItems.Count;
        /// </summary>
        /// <returns>imagesControlsItems.Count</returns>
        public int GetPanelsCount()
        {
            return imagesControlsItems.Count;
        }
        /// <summary>
        /// Returns the index of first found item from imagesControlsItems[] with rbSelectImage.Checked;
        /// Literally means: which image is selected?
        /// </summary>
        /// <returns>first found imagesControlsItems[] index with rbSelectImage.Checked; "-1" if none;</returns>
        public int GetCheckedPanelIndex()
        { 
            for (int i = 0; i < imagesControlsItems.Count; i++)
            {
                if (imagesControlsItems[i].rbSelectImage.Checked)
                    return i;
            }
            return -1;
        }
        /// <summary>
        /// Checks and sets focus to imagesControlsItems[panelIndex].rbSelectImage if item with panelIndex exists;
        /// </summary>
        /// <param name="panelIndex">index of imagesControlsItems[] item</param>
        /// <returns>"true" if item with panelIndex was found; otherwise "false";</returns>
        public bool TryCheckPanelAtIndex(int panelIndex)
        {
            if ((panelIndex >= 0) && (panelIndex < imagesControlsItems.Count))
            {
                imagesControlsItems[panelIndex].rbSelectImage.Checked = true;
                imagesControlsItems[panelIndex].rbSelectImage.Focus(); // forced focus
                return true;
            }
            return false;
        }
        /// <summary>
        /// Returns imagesControlsItems[] with rbSelectImage.Handle the same as msg.HWnd;
        /// Needed to alternatively specify by FrmMain.ProcessCmdKey() which radiobutton received KeyDown event;
        /// See more information in FrmMain.ProcessCmdKey();
        /// </summary>
        /// <param name="msg">Component message (received by FrmMain.ProcessCmdKey())</param>
        /// <returns>imagesControlsItems[] with rbSelectImage.Handle the same as msg.HWnd; "-1" if not found;</returns>
        public int GetRadioButtonSenderIndex(ref Message msg)
        {
            for (int i = 0; i < imagesControlsItems.Count; i++)
            {
                if (msg.HWnd.Equals(imagesControlsItems[i].rbSelectImage.Handle))
                {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// Hides or shows pixel format labels (lblPixelInfo) for each imagesControlsItems[] item;
        /// </summary>
        /// <param name="visible">visibility to be set to the labels</param>
        public void SetPixelInfoLabelsVisibility(bool visible)
        {
            foreach (ImageControlsItem panel in imagesControlsItems)
            {
                panel.lblPixelInfo.Visible = visible;
            }
        }
        /// <summary>
        /// Returns information whether pixel format labels are visible;
        /// The visibility of the labels is assumed to be the same for all;
        /// </summary>
        /// <returns>"false" if there are no labels or they are not visible; "true" otherwise;</returns>
        public bool PixelInfoLabelsVisible()
        {
            return (imagesControlsItems.Count > 0) && (imagesControlsItems[0].lblPixelInfo.Visible);
        }
        /// <summary>
        /// Sets the Text value to pixel info label belonging to imagesControlsItems[panelIndex];
        /// </summary>
        /// <param name="panelIndex">imagesControlsItems[] index</param>
        /// <param name="text">text with pointed pixel info</param>
        public void SetPixelInfoLabelText(int panelIndex, string text)
        {
            if (panelIndex >= 0 && panelIndex <= imagesControlsItems.Count)
            {
                imagesControlsItems[panelIndex].lblPixelInfo.Text = text;
            }
        }
        // OTHER METHODS: ***************************************************************
        /// <summary>
        /// Refreshes imagesControlsItems[] components properties values and panels order;
        /// Components properties:
        /// - imagesControlsItems[] components Tag value (by ImageControlsItem.SetControlsTags());
        /// - refreshes panel number label (lblImageNumber.Text);
        /// - btnSwapOrAddImage caption ("v" or "+" depending on current index);
        /// - checks if anyPanelChecked and initiates TryCheckPanelAtIndex(0) if none is checked;
        /// </summary>
        private void RefreshControlsTagsAndText()
        {
            bool anyPanelChecked = false;
            for (int i = 0; i < imagesControlsItems.Count; i++)
            {
                imagesControlsItems[i].SetControlsTags(i);
                this.Controls.SetChildIndex(imagesControlsItems[i], 0); // dock order: before imagesControlsItems[0] (which has dock order = 0)
                if (i < imagesControlsItems.Count - 2)
                {
                    imagesControlsItems[i].btnSwapOrAddImage.Text = "v";
                }
                else if (i == imagesControlsItems.Count - 2)
                {
                    imagesControlsItems[i].btnSwapOrAddImage.Text = "+";
                }

                if (!anyPanelChecked)
                {
                    anyPanelChecked = imagesControlsItems[i].rbSelectImage.Checked;
                }

                if (i < imagesControlsItems.Count - 1)
                {
                    imagesControlsItems[i].lblImageNumber.Text = (i + 1).ToString();
                }
            }
            if (!anyPanelChecked)
            {
                TryCheckPanelAtIndex(0);
            }
            this.Height = imagesControlsItems.Last().Bottom;
        }
    }
    /// <summary>
    /// The member of ImagesControlsPanel.imagesControlsItems[];
    /// Each ImageControlsItem corresponds to image from FrmMain (sourceImages[], resultImage);
    /// Contains controls allowing user to manage FrmMain images (adding, removing, swapping, loading, selecting, pointed pixel info display)
    /// </summary>
    public class ImageControlsItem : Panel
    {
        public const int buttonHeight = 23;
        public const int buttonMargin = 4;

        private Panel leftPanel = new Panel();
        private Panel middlePanel = new Panel();
        private Panel rightPanel = new Panel();
        public Label lblImageNumber = new Label();
        public RadioButton rbSelectImage = new RadioButton();
        public Button btnSwapOrAddImage = new Button();
        public Button btnRemoveImage = new Button();
        public Button btnLoadImage = new Button();
        public Label lblPixelInfo = new Label();
        /// <summary>
        /// Structure containing set of events for ImageControlsItem components;
        /// </summary>
        public struct ImageControlsItemEvents
        {
            public EventHandler rbSelectImageEvent, btnSwapOrAddImageEvent, btnRemoveImageEvent, btnLoadImageEvent;
            public ImageControlsItemEvents (EventHandler rbSelectImageEvent, EventHandler btnSwapOrAddImageEvent, EventHandler btnRemoveImageEvent, EventHandler btnLoadImageEvent)
            {
                this.rbSelectImageEvent = rbSelectImageEvent;
                this.btnSwapOrAddImageEvent = btnSwapOrAddImageEvent;
                this.btnRemoveImageEvent = btnRemoveImageEvent;
                this.btnLoadImageEvent = btnLoadImageEvent;
            }
        }

        /// <summary>
        /// Class constructor; creates controls, sets their events;
        /// </summary>
        public ImageControlsItem(int panelNumber, ImageControlsItemEvents events)
        {
            this.Name = "imageControlsItem" + (panelNumber).ToString();

            // preparing leftPanel:
            leftPanel.Name = "leftPanel" + (panelNumber).ToString();
            leftPanel.AutoSize = true;
            leftPanel.Dock = DockStyle.Left;
            leftPanel.Padding = new System.Windows.Forms.Padding(buttonMargin);

            // preparing rightPanel:
            rightPanel.Name = "rightPanel" + (panelNumber).ToString();
            rightPanel.AutoSize = true;
            rightPanel.Dock = DockStyle.Right;
            rightPanel.Padding = new System.Windows.Forms.Padding(buttonMargin, 0, 0, 0);

            // preparing middlePanel:
            middlePanel.Name = "middlePanel" + (panelNumber).ToString();
            middlePanel.AutoSize = true;
            middlePanel.Dock = DockStyle.Fill;
            middlePanel.Padding = new System.Windows.Forms.Padding(0, buttonMargin,0, buttonMargin);

            // preparing lblImageNumber:
            lblImageNumber.SuspendLayout();
            lblImageNumber.Name = "lblImageNumber" + (panelNumber).ToString();
            lblImageNumber.AutoSize = false;
            lblImageNumber.Font = new Font("Cascadia Mono", 8);
            lblImageNumber.Size = new System.Drawing.Size(buttonHeight, buttonHeight + buttonMargin);
            lblImageNumber.TextAlign = ContentAlignment.MiddleCenter;
            lblImageNumber.Text = "00";
            lblImageNumber.Location = new System.Drawing.Point(0, 2);

            // preparing rbSelectImage:
            rbSelectImage.SuspendLayout();
            rbSelectImage.Name = "rbSelectImage" + (panelNumber).ToString();
            rbSelectImage.AutoSize = true;
            rbSelectImage.Size = new System.Drawing.Size(buttonHeight - buttonMargin, buttonHeight);
            rbSelectImage.Location = new System.Drawing.Point(buttonHeight, 10);
            rbSelectImage.Text = "";
            rbSelectImage.CheckedChanged += events.rbSelectImageEvent;

            // preparing btnSwapOrAddImage:
            btnSwapOrAddImage.SuspendLayout();
            btnSwapOrAddImage.Name = "btnSwapOrAddImage" + (panelNumber).ToString();
            btnSwapOrAddImage.Size = new System.Drawing.Size(buttonHeight, buttonHeight);
            btnSwapOrAddImage.Location = new System.Drawing.Point(rbSelectImage.Right, buttonMargin);
            btnSwapOrAddImage.Margin = new System.Windows.Forms.Padding(buttonMargin);
            btnSwapOrAddImage.Text = "?";
            btnSwapOrAddImage.Click += events.btnSwapOrAddImageEvent;

            // preparing btnRemoveImage:
            btnRemoveImage.SuspendLayout();
            btnRemoveImage.Name = "btnRemoveImage" + (panelNumber).ToString();
            btnRemoveImage.Size = new System.Drawing.Size(buttonHeight, buttonHeight);
            btnRemoveImage.Location = new System.Drawing.Point(btnSwapOrAddImage.Right + buttonMargin, buttonMargin);
            btnRemoveImage.Margin = new System.Windows.Forms.Padding(0, buttonMargin, 0, buttonMargin);
            btnRemoveImage.Text = "X";
            btnRemoveImage.Click += events.btnRemoveImageEvent;

            // preparing btnLoadImage:
            btnLoadImage.SuspendLayout();
            btnLoadImage.Name = "btnLoadImage" + (panelNumber).ToString();
            btnLoadImage.Size = new System.Drawing.Size(64, buttonHeight);
            btnLoadImage.Location = new System.Drawing.Point(100, 0);
            btnLoadImage.Margin = new System.Windows.Forms.Padding(0, buttonMargin, 0, buttonMargin);
            btnLoadImage.Text = "Load image";
            btnLoadImage.Dock = DockStyle.Fill;
            btnLoadImage.Click += events.btnLoadImageEvent;

            // preparing lblPixelInfo:
            lblPixelInfo.SuspendLayout();
            lblPixelInfo.Name = "lblPixelInfo" + (panelNumber).ToString();
            lblPixelInfo.AutoSize = false;
            lblPixelInfo.Font = new Font("Cascadia Mono", 8);
            lblPixelInfo.Size = new System.Drawing.Size(112, buttonHeight - buttonMargin);
            lblPixelInfo.TextAlign = ContentAlignment.MiddleCenter;
            lblPixelInfo.Text = lblPixelInfo.Name;
            lblPixelInfo.Location = new System.Drawing.Point(0, 0);
            lblPixelInfo.Margin = new System.Windows.Forms.Padding(buttonMargin);
            lblPixelInfo.Dock = DockStyle.Right;

            // adding controls to panels:
            middlePanel.Controls.Add(btnLoadImage);
            rightPanel.Controls.Add(lblPixelInfo);
            leftPanel.Controls.Add(btnRemoveImage);
            leftPanel.Controls.Add(btnSwapOrAddImage);
            leftPanel.Controls.Add(rbSelectImage);
            leftPanel.Controls.Add(lblImageNumber);

            this.Controls.Add(leftPanel);
            this.Controls.Add(middlePanel);
            this.Controls.Add(rightPanel);

            this.Controls.SetChildIndex(middlePanel, 0); // dock order fix: fill after docking others

            btnLoadImage.ResumeLayout(false);
            lblPixelInfo.ResumeLayout(false);
            btnRemoveImage.ResumeLayout(false);
            btnSwapOrAddImage.ResumeLayout(false);
            rbSelectImage.ResumeLayout(false);
            lblImageNumber.ResumeLayout(false);
        }
        /// <summary>
        /// Hides controls reserved for source image management;
        /// </summary>
        public void SetToResultImageRole()
        {
            this.btnLoadImage.Hide();
            this.btnRemoveImage.Hide();
            this.btnSwapOrAddImage.Hide();
            this.rbSelectImage.Text = "Result image preview";
            this.lblImageNumber.Text = "";
            this.rbSelectImage.Location = new System.Drawing.Point(ImageControlsItem.buttonHeight, 8);
        }
        /// <summary>
        /// Sets Tag property value for owned controls;
        /// </summary>
        /// <param name="tag">new Tag value</param>
        public void SetControlsTags(int tag)
        {
            this.Tag = tag;
            btnLoadImage.Tag = tag;
            lblPixelInfo.Tag = tag;
            lblImageNumber.Tag = tag;
            btnRemoveImage.Tag = tag;
            btnSwapOrAddImage.Tag = tag;
            rbSelectImage.Tag = tag;
        }
    }
}
