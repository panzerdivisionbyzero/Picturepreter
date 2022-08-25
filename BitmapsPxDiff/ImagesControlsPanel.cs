using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitmapsPxDiff
{
    public class ImagesControlsPanel : Panel
    {
        private int controlAddCount = 0;
        private ImageControlsPanel.ImageControlsPanelEvents internalEvents; // events assigned to each ImageControlsPanel (sub-panel)
        public List<ImageControlsPanel> imageControlsPanels = new List<ImageControlsPanel>(); // list of sub-panels

        public event EventHandler? OnImageSelected;
        public event EventHandler? OnAddImageClick;
        public event EventHandler? OnLoadImageClick;
        public event EventHandler? OnRemoveImageClick;
        public event EventHandler? OnSwitchImageClick;

        public ImagesControlsPanel()
        {
            // prepare events for sub-panels:
            internalEvents = new ImageControlsPanel.ImageControlsPanelEvents(rbSelectImage_CheckedChanged, btnSwitchOrAddImage_Click, btnRemoveImage_Click, btnLoadImage_Click);
            
            // create first sub-panel and adjust it to "result image" panel role:
            AddImageControlsSet(internalEvents);
            imageControlsPanels[0].btnLoadImage.Hide();
            imageControlsPanels[0].btnRemoveImage.Hide();
            imageControlsPanels[0].btnSwapOrAddImage.Hide();
            imageControlsPanels[0].rbSelectImage.Text = "Result image preview";
            imageControlsPanels[0].lblImageNumber.Text = "";
            imageControlsPanels[0].rbSelectImage.Location = new System.Drawing.Point(ImageControlsPanel.buttonHeight, 8);
            this.Controls.SetChildIndex(imageControlsPanels[0], 0); // dock order: the last of all panels
        }
        // EVENTS METHODS: ****************************************************************
        private void rbSelectImage_CheckedChanged(object sender, EventArgs e)
        {
            if (!(sender is RadioButton))
            {
                return;
            }
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked) // radio buttons belong to different panels, so it's necessary to manually uncheck others
            {
                foreach (ImageControlsPanel panel in imageControlsPanels)
                {
                    if ((int)panel.Tag != (int)rb.Tag)
                    {
                        panel.rbSelectImage.Checked = false;
                    }
                }
            }
            if (OnImageSelected != null)
            {
                OnImageSelected(sender, e);
            }
        }
        private void btnSwitchOrAddImage_Click(object sender, EventArgs e)
        {
            if (!(sender is Button))
            {
                return;
            }
            Button btn = (Button)sender;
            int tagIndex = (int)btn.Tag;
            if (tagIndex > imageControlsPanels.Count - 2) // [index-1] is result image index; [index-2] should have btnAddImage_Click() action;
            {
                return;
            }
            else if (tagIndex == imageControlsPanels.Count - 2) // "+" button
            {
                AddImageControlsSet();
                if (OnAddImageClick != null)
                {
                    OnAddImageClick(sender, e);
                }
            }
            else // "v" button
            {
                imageControlsPanels.Reverse(tagIndex, 2);
                if (OnSwitchImageClick != null) // trigger form event first (with current sender.Tag value)
                {
                    OnSwitchImageClick(sender, e);
                }
                RefreshControlsTagsAndTexts(); // reset sender.Tag value
            }
        }
        private void btnRemoveImage_Click(object sender, EventArgs e)
        {
            if (!(sender is Button) || (imageControlsPanels.Count < 3))
            {
                return;
            }
            Button btn = (Button)sender;
            int tagIndex = (int)btn.Tag;
            imageControlsPanels[tagIndex].Controls.Clear();
            this.Controls.Remove(imageControlsPanels[tagIndex]);
            imageControlsPanels.RemoveAt(tagIndex);
            RefreshControlsTagsAndTexts();
            if (OnRemoveImageClick != null)
            {
                OnRemoveImageClick(sender, e);
            }
        }
        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            if (!(sender is Button))
            {
                return;
            }
            Button btn = (Button)sender;
            int tagIndex = (int)btn.Tag;
            if (tagIndex > imageControlsPanels.Count - 2)
            {
                return;
            }
            if (OnLoadImageClick != null)
            {
                OnLoadImageClick(sender, e);
            }
        }
        public int GetRadioButtonSenderIndex(ref Message msg)
        {
            for (int i = 0; i < imageControlsPanels.Count; i++)
            {
                if (msg.HWnd.Equals(imageControlsPanels[i].rbSelectImage.Handle))
                {
                    return i;
                }
            }
            return -1;
        }
        // PUBLIC METHODS: **************************************************************
        public void AddImageControlsSet()
        {
            AddImageControlsSet(internalEvents);
        }
        public void AddImageControlsSet(ImageControlsPanel.ImageControlsPanelEvents events)
        {
            ImageControlsPanel newPanel = new ImageControlsPanel(controlAddCount++, events);
            newPanel.SuspendLayout();
            newPanel.Size = new System.Drawing.Size(this.Width - 10, ImageControlsPanel.buttonHeight + 2 * ImageControlsPanel.buttonMargin);
            newPanel.Location = new System.Drawing.Point(0, 0);
            newPanel.Dock = DockStyle.Top;
            newPanel.SetControlsTags(imageControlsPanels.Count);
            int index = Math.Max(0, imageControlsPanels.Count - 1);
            imageControlsPanels.Insert(index, newPanel);
            this.Controls.Add(newPanel);

            newPanel.ResumeLayout(true);
            RefreshControlsTagsAndTexts();
        }
        public int GetPanelsCount()
        {
            return imageControlsPanels.Count;
        }
        public bool TryCheckPanelAtIndex(int panelIndex)
        {
            if ((panelIndex >= 0) && (panelIndex < imageControlsPanels.Count))
            {
                imageControlsPanels[panelIndex].rbSelectImage.Checked = true;
                imageControlsPanels[panelIndex].rbSelectImage.Focus(); // forced focus
                return true;
            }
            return false;
        }
        public void ShowPixelFormatLabels()
        {
            foreach (ImageControlsPanel panel in imageControlsPanels)
            {
                panel.lblPixelInfo.Show();
            }
        }
        public void HidePixelFormatLabels()
        {
            foreach (ImageControlsPanel panel in imageControlsPanels)
            {
                panel.lblPixelInfo.Hide();
            }
        }
        public bool PixelFormatLabelsVisible()
        {
            return (imageControlsPanels.Count > 0) && (imageControlsPanels[0].lblPixelInfo.Visible);
        }
        public void SetPixelFormatLabelText(int panelIndex, string text)
        {
            if (panelIndex >= 0 && panelIndex <= imageControlsPanels.Count)
            {
                imageControlsPanels[panelIndex].lblPixelInfo.Text = text;
            }
        }
        // OTHER METHODS: ***************************************************************
        private void RefreshControlsTagsAndTexts()
        {
            bool anyPanelChecked = false;
            for (int i = 0; i < imageControlsPanels.Count; i++)
            {
                imageControlsPanels[i].SetControlsTags(i);
                this.Controls.SetChildIndex(imageControlsPanels[i], 0); // dock order: before imageControlsPanels[0] (which has dock order = 0)
                if (i < imageControlsPanels.Count - 2)
                {
                    imageControlsPanels[i].SetSwapOrAddImageButton("v");
                }
                else if (i == imageControlsPanels.Count - 2)
                {
                    imageControlsPanels[i].SetSwapOrAddImageButton("+");
                }
                if (!anyPanelChecked)
                {
                    anyPanelChecked = imageControlsPanels[i].rbSelectImage.Checked;
                }

                if (i < imageControlsPanels.Count - 1)
                {
                    imageControlsPanels[i].lblImageNumber.Text = (i + 1).ToString();
                }
            }
            if (!anyPanelChecked)
            {
                TryCheckPanelAtIndex(0);
            }
            this.Height = imageControlsPanels.Last().Bottom;
        }
    }

    public class ImageControlsPanel : Panel
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

        public struct ImageControlsPanelEvents
        {
            public EventHandler rbSelectImageEvent, btnSwapOrAddImageEvent, btnRemoveImageEvent, btnLoadImageEvent;
            public ImageControlsPanelEvents (EventHandler rbSelectImageEvent, EventHandler btnSwapOrAddImageEvent, EventHandler btnRemoveImageEvent, EventHandler btnLoadImageEvent)
            {
                this.rbSelectImageEvent = rbSelectImageEvent;
                this.btnSwapOrAddImageEvent = btnSwapOrAddImageEvent;
                this.btnRemoveImageEvent = btnRemoveImageEvent;
                this.btnLoadImageEvent = btnLoadImageEvent;
            }
        }
        public ImageControlsPanel(int panelNumber, ImageControlsPanelEvents events)
        {
            this.Name = "imageControlsPanel" + (panelNumber).ToString();
            this.TabIndex = panelNumber;

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
        public void SetSwapOrAddImageButton(string buttonText)
        {
            btnSwapOrAddImage.Text = buttonText;
        }
    }
}
