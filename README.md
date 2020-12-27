# File Explorer Helper

## About

This tool is an unofficial "add-on" for the Windows File Explorer. It functions entirely independently from the Windows File Explorer and adds useful features that are currently not present in Windows.

## How to Intall

Download the .zip file from the "Releases" section of this repository. After unzipping the file, run "setup.exe" to install the application. 

You can also clone the repository and run the shortcut in the root folder of the repository.

## Features

To begin using the features of the File Explorer Helper, you must select a folder to modify files in using the "Browse for Folder" button.

#### Cleanup Folder

![Cleanup Folder Function GIF](/FileExplorerHelper/Assets/cleanup-folder.gif)

The "Cleanup Folder" button will search through all the files in the folder and automatically sort them into newly created sub-folders based on the file type/file extension.

**NOTE:** This action auto-creates folder(s) in the currently selected folder. If a folder with the same name already exists, the sorted files will be added to that folder.

The folder names (along with the currently supported extensions) are:
* **Audio**
  * AIF, CDA, MID, MIDI, MP3, MPA, OGG, WAV, WMA, WPL, AAC, FLAC, M4A
* **Documents**
  * ARJ, DEB, PKG, RAR, RPM, GZ, Z, ZIP, CSV, DAT, DB, DBF, LOG, MDB, SAV, SQL, TAR, XML, FNT, FON, OTF, TTF, ASP, ASPX, CER, CFM, CGI, CSS, HTM, HTML, JS, JSP, PART, PHP, PY, RSS, XHTML, KEY, OPD, PPS, PPT, PPTX, C, CLASS, CPP, CS, H, JAVA, SH, SWIFT, VB, ODS, XLR, XLS, XLSX, DOC, DOCX, ODT, PDF, RTF, TEX, TXT, WKS, WPS, WPD
* **Executables**
  * APK, BAT, CGI, PL, COM, EXE, GADGET, JAR, PY, WSF
* **Images**
  * AI, BMP, GIF, ICO, JPEG, JPG, PNG, PS, PSD, SVG, TIF, TIFF
* **Shortcuts**
  * LNK, URL
* **Videos**
  * AVI, FLV, M4V, H264, MKV, MOV, MP4, MPG, MPEG, RM, SWF, VOB, WMV, QT, WEBM

#### Find and Replace

This will find **ALL** instances of the text provided in the "Find" text box and replace them with the text provided in the "Replace With" text box. 
NOTE: This only applies to the file name itself.

![Replace Function GIF](/FileExplorerHelper/Assets/replace.gif)

#### Find and Remove

This will remove **ALL** instances of the text provided in the "Remove" text box from all the files in the folder. 
NOTE: This only applies to the file name itself.

#### Undo

The Undo button will undo the most recent action and restore the modified file(s) to their original name.

**NOTE** The Undo function currently does not work on the Cleanup Folder function.

#### The Output Window

The output window will provide you will helpful status messages or warnings as you use the program.

## Other Info

Please feel free to download this project and use it however you would like!
This project is still in development, so reporting any bugs would be greatly appreciated.
Thank you!

Track the project development on [Trello](https://trello.com/b/gQziN8Dk/, "Trello Board")

This project was made using [Microsoft Visual Studio](https://visualstudio.microsoft.com/vs/community/, "Visual Studio Community") with C# and .NET

**NOTE:** This program has permission to access and modify file names completely. This could result in file corruption. Use it at your own risk.

## Credits

Created by Jonathan McLatcher.

[Material Design in XAML](http://materialdesigninxaml.net/, "Material Design in XAML") was used for UI styling.
