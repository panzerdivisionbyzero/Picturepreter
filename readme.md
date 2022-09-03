# About the project
The Picturepreter is kind of graphics editor that performs user-written LUA script for each pixel. It's mainly designed for custom mixing 2 or more images, but it can be used for one image as well.  
The script is processed asynchronously by multiple threads.

<details open><summary>(click to hide image)</summary>

![Picturepreter_readme_anim](https://user-images.githubusercontent.com/109442319/188285295-e058a8b8-b795-48d2-99b4-8dc1404505c4.gif)

</details>

# Usage instructions
### 1. Basics:
- Use "+" or "X" buttons (top-left corner) to specify source images number;
- Load images for all defined inputs; The result image dimensions are defined by source images intersection;
- Edit script as you want (see point 3 in this section for more details);
- Click "Run script" button; "Result image preview" radio button should be selected automatically to show result image;
### 2. Other features:
- Switching between images preview: click on the radiobuttons corresponding to the images (top-left corner); Alternatively use CTRL+{1..9} shortcut;
- Swapping source images: click the "v" button to swap [n] source image with [n+1] source image;
- Interpolation mode selection: changes display smoothing;
- Pixel info format checkboxes: changes visibility and format of pointed pixel info;
- Image Pointer: click left mouse button on the image preview to point pixel constantly; click right mouse button to clear selection;
- Script output text box: displays user-defined debug messages and (in case of error) script error information with full script printout;
- Load/Save script (always with load/save dialog);
- Save result image (without Image Pointer; always with save dialog);
### 3. Script global variables and functions:
```
- imagesPixels[imageIndex] = {A,R,G,B} // source images pixel
- result = {A,R,G,B} // result image pixel
// for read/use only purposes:
- chunkStartX
- chunkStartY
- chunkLastX
- chunkLastY
- imageW
- imageH
- currentX
- currentY
- imagesCount
- CastToByte(int) // returns "byte" (int within range: [0..255])
- TableLength(tab) // returns int
- DebugEachPx(str) // prints given string into output text box
- DebugForPx(x,y,str)
- DebugChunkBegin(str)
- DebugChunkEnd(str)
- DebugImageBegin(str)
- DebugImageEnd(str)
```
### 4. For more scripting information see "examples" directory;

# Executable
The executable file is attached in [Releases](https://github.com/panzerdivisionbyzero/Picturepreter/releases) section. It requires .NET SDK libraries, so you may need to install them if the app won't start:  
https://dotnet.microsoft.com/en-us/download/dotnet/6.0

# License
Licensed under the terms of the GNU GPL 2.0 license, excluding used libraries:
- MoonSharp, licensed under MIT license;

and used code snippets marked with link to original source and listed below:
- C#:
  - https://stackoverflow.com/questions/1600962/displaying-the-build-date?answertab=modifieddesc#tab-top
  - https://stackoverflow.com/questions/22426390/disable-selection-of-controls-by-arrow-keys-in-a-form
  - https://www.codeproject.com/messages/3182303/re-image-changed-in-picturebox-event-question.aspx
  - https://stackoverflow.com/questions/29157/how-do-i-make-a-picturebox-use-nearest-neighbor-resampling
  - https://www.codeproject.com/Articles/20923/Mouse-Position-over-Image-in-a-PictureBox
- LUA:
  - https://stackoverflow.com/questions/2705793/how-to-get-number-of-entries-in-a-lua-table
- GIT:
  - https://github.com/github/gitignore/blob/main/VisualStudio.gitignore

Sample images come from KKND2: Krossfire;  

Copyright (c) 2022 by Paweł Witkowski

In case of questions contact me at: pawel.vitek.witkowski@gmail.com  
  
https://github.com/panzerdivisionbyzero

