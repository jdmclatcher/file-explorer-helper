/*
 * Jonathan McLatcher
 * File Explorer Helper
 * 2020
 */

using System;
using System.Collections.Generic;
using System.IO;

namespace FileExplorerHelper
{
    /// class that handles the cleanup of the folder functionality
    class CleanupFolder : Util
    {
        private Util util; // ref to class
        public CleanupFolder(Util utilClass)
        {
            // create instance of util class
            util = utilClass;
        }

        public void Cleanup()
        {
            util.BackupFiles();
            // list out all files in the root folder
            List<FileInfo> files = util.GetListOfFiles();

            // check for if there are files in the folder
            bool foundAFile = false;
            if (files.Count > 0)
            {
                foundAFile = true;
            }
            // loop through all the files and apply changes using helper method
            for (int i = 0; i < files.Count; i++)
            {
                // get extension of current file
                string extension = util.ReturnExtension(files[i]);

                // sort based on type of file
                if (Enum.IsDefined(typeof(Audio), extension.ToUpper()))
                {
                    Console.WriteLine("An Audio was found.");
                    CleanupHelper(files[i], "Audio");
                }
                else if (Enum.IsDefined(typeof(Documents), extension.ToUpper()))
                {
                    Console.WriteLine("A Document was found.");
                    CleanupHelper(files[i], "Documents");
                    
                }
                else if (Enum.IsDefined(typeof(Executables), extension.ToUpper()))
                {
                    Console.WriteLine("An Executable was found.");
                    CleanupHelper(files[i], "Executables");
                }
                else if (Enum.IsDefined(typeof(Images), extension.ToUpper()))
                {
                    Console.WriteLine("An Image was found.");
                    CleanupHelper(files[i], "Images");
                }
                else if (Enum.IsDefined(typeof(Shortcuts), extension.ToUpper()))
                {
                    Console.WriteLine("A Shortcut was found.");
                    CleanupHelper(files[i], "Shortcuts");
                }
                else if (Enum.IsDefined(typeof(Videos), extension.ToUpper()))
                {
                    Console.WriteLine("A Video was found.");
                    CleanupHelper(files[i], "Videos");
                }
                else
                {
                    // leave in original folder
                }

            }

            if (!foundAFile)
            {
                Console.WriteLine("No files were found.");
            }
            
        }

        // helper method to the Cleanup() method
        private void CleanupHelper(FileInfo file, string folderName)
        {
            // change the path of the file to be path with new folder
            // create new folder (Directory) to place file in (if not already created)
            if (!Directory.Exists(util.GetRootFolder() + "/" + folderName))
            {
                Directory.CreateDirectory(util.GetRootFolder() + "/" + folderName);
                Console.WriteLine("Successfully created folder \"" + folderName + "\".");
            }
            else
            {
                Console.WriteLine("Destination Folder \"" + folderName + "\" already exists.");
            }


            // check if destination file already exists
            if (!File.Exists(util.GetRootFolder() + "/" + folderName + "/" + file.Name))
            {
                // move file to new location
                Console.Write("Successfully moved " + file.FullName + " to ");
                file.MoveTo(util.GetRootFolder() + "/" + folderName + "/" + file.Name);
                Console.Write(file.FullName);
                Console.WriteLine();
            } else
            {
                // dont move file
                Console.WriteLine("File already exists in destination. File not moved");

                // send message, 2 for a yellow warning
                Console.WriteLine("WARNING: A file \"" + file.Name + "\" already exists in destination. \"" + file.Name + "\" not moved.", 2);
                util.AddMessage("A file \"" + file.Name + "\" already exists in destination. \"" + file.Name + "\" not moved.", 2);
            }
        }

    }
}
