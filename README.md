# File Explorer Helper

## About

This tool is an unofficial "add-on" for the Windows File Explorer. It interacts completely independently from the Windows File Explorer, but adds useful features that are currently not present in Windows.

## How to Open

Download the repository and unzip the folder. Then, click on the shortcut called "File Explorer Helper" to run the program.

You can also clone the repository and run directly from the source code.

## Features

To begin the using the features of the File Explorer Helper, you must select a folder to modify files in using the "Browse for Folder" button.

#### Cleanup Folder

The "Cleanup Folder" button will search through all the files in the folder and automatically sort them into newly created sub-folders based on the file type/file extension.

NOTE: This action auto-creates folder(s) in the set folder. If a folder with the same name already exists, the sorted files will be added to that folder.

#### Rename Images

A common file naming convention for auto-generated files named by cameras, phones, etc. is *"FILTYPE_YYYYMMDD_HHMMSS"*. 

If the files in the folder are using this scheme, you can choose from a variety of naming convention options to automatically rename all the files in that folder to.

NOTE: AM is represented with a "0" in front of the hour, while PM has no "0".

#### Find and Replace

This will find **ALL** the instances of the letter or phrase you specified in the "Find" text box and replace them with the value you provide in the "Replace With" text box. This only applies to the file name itself.

#### Find and Remove

This will remove **ALL** instances of the letter of phrase you specifed in the "Remove" text box from all the files in the folder. This only applies to the file name itself.

#### Undo

The Undo button will undo the most recent action and restore the modified file(s) to their original place/name.

#### The Output Window

The output window will provide you will helpful status messages or warnings as you use the program.

#### Print Details

The "Print Details" button will print out specific details about the current folder in a .txt file. This file will be created in the folder that you selected with the "Browse for Folder" button.

## Other Info

Please feel free to download this project and use it however you would like!
This project is still in development, so reporting any bugs would be greatly appreciated.
Thank you!

Track the project development on [Trello](https://trello.com/b/gQziN8Dk/, "Trello Board")

This project was made using Microsoft Visual Studio with C# and .NET

NOTE: This program has permission to access and modify file names completely. This could result in file corruption. Use at your own risk.

## Credits

Created by Jonathan McLatcher

[Material Design in XAML](http://materialdesigninxaml.net/, "Material Design in XAML") was used for UI styling.
