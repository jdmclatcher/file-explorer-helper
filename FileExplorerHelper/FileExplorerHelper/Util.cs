/*
 * Jonathan McLatcher
 * File Explorer Helper
 * 2019
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows;

namespace FileExplorerHelper
{
    /// <summary>
    /// utility class for helpful tools
    /// </summary>

    // for all helper methods
    // also for storing root path to folder
    // and other constant data
    public class Util
    { 
        // create a variable to hold data about root folder
        private DirectoryInfo rootFolder;

        private int subFolderCount;
        private int fileCount;
        private bool canUseFunctions = true; // can use the functions of the program
        public string detailsFilesName = "Files_Details";
        private List<FileInfo> backupFiles; // list of current files and info in folder
        #region Constructor and Getters/Setters
        public DirectoryInfo GetRootFolder()
        {
            return this.rootFolder;
        }

        public void SetRootFolder(DirectoryInfo rootFolder)
        {
            this.rootFolder = rootFolder;
        }

        public int GetNumSubFolders()
        {
            return this.subFolderCount;
        }
        private void SetNumSubFolders(int subFolderCount)
        {
            this.subFolderCount = subFolderCount;
        }

        public int GetNumFiles()
        {
            return this.fileCount;
        }
        private void SetNumFiles(int fileCount)
        {
            this.fileCount = fileCount;
        }

        public bool GetCanUseFunctions()
        {
            return this.canUseFunctions;
        }

        // return a list of the previously backed-up files
        public List<FileInfo> GetBackupFiles()
        {
            return this.backupFiles;
        }

        // actually does the setting
        private void SetBackupFiles(List<FileInfo> files)
        {
            this.backupFiles = files;
        }
        #endregion

        #region Helper Methods
        // sets count of files and folders in the root folder
        public void CountFilesAndFolders()
        { 
            // create a list for the folders
            List<DirectoryInfo> folders = new DirectoryInfo(GetRootFolder().FullName).GetDirectories().ToList<DirectoryInfo>();
            SetNumSubFolders(folders.Count);

            SetNumFiles(GetListOfFiles().Count); // from the helper method
        }

        // return the file extension without the "."
        public string ReturnExtension(FileInfo file)
        {
            // find last index of the "." for the extension
            return file.FullName.Substring(file.FullName.LastIndexOf('.') + 1);
        }

        public List<FileInfo> GetListOfFiles()
        {
            // returns all the files in the root folder
            List<FileInfo> files = new DirectoryInfo(GetRootFolder().FullName).GetFiles().ToList<FileInfo>();
            
            // loop through each file and check if it has a .ini file and ignore it
            for (int i = 0; i < files.Count; i++)
            {
                // check if .ini desktop file
                if (ReturnExtension(files[i]).Equals("ini", StringComparison.InvariantCultureIgnoreCase))
                {
                    files.RemoveAt(i);
                }
            }
            return files; 
        }

        #endregion

        // function that opens dialog to prompt input of desired folder
        public void BrowseAndSelectFolder()
        {
            FolderBrowserDialog Dialog = new FolderBrowserDialog();


            Dialog.ShowDialog(); // pop up window

            if((Dialog.SelectedPath == null) || (Dialog.SelectedPath == ""))
            {
                this.canUseFunctions = false; // restrict access to functions until proper folder selected
            }
            else
            {
                this.canUseFunctions = true; // dont restrict user access
                // sets the root folder to the selected folder
                SetRootFolder(new DirectoryInfo(Dialog.SelectedPath));
                CountFilesAndFolders(); // update file and subfolder count
            }

            Dialog.Reset();
        }

        public void PrintDetails()
        {
            // print out all paths of all files in folder to one .txt file
            // new blank string array of the same size as the number of files
            string[] paths = new string[GetListOfFiles().Count]; 
            for(int i = 0; i < GetListOfFiles().Count; i++)
            {
                paths[i] = GetListOfFiles()[i].FullName;
            }
            File.WriteAllLines(GetRootFolder().FullName + "/" + detailsFilesName + ".txt", paths);
        }

        public void AddMessage(string message, int severity)
        {
            MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
            mainWindow.AddMessageWindow(message, severity);
        }

        // called at every major function, automatically sets
        // array to values currently in folder
        public void BackupFiles()
        {
            // save current state of files
            SetBackupFiles(GetListOfFiles()); // set the backup files value to current files in folder
        }

        public void RestoreCleanup()
        {
            // loop through each sub folder (if called one of the specified folders and extract and add to list)
            // TOFIX - "Second path fragment must not be a drive or UNC name."
            List<DirectoryInfo> folders = GetRootFolder().GetDirectories(GetRootFolder().FullName).ToList<DirectoryInfo>(); // convert to list
            List<FileInfo> files = new List<FileInfo>(); // make new list to store final files

            // run length of backup files
            for (int i = 0; i < GetBackupFiles().Count; i++)
            {
                // check if file was not moved (is same) and add to list
                if (GetBackupFiles()[i].FullName.Equals(files[i].FullName))
                {
                    files.Add(GetBackupFiles()[i]); // add to list
                }

                // loop through each folder
                foreach(DirectoryInfo folder in folders)
                {
                    if (folder.Name.Equals("Documents"))
                    {
                        // then loop throuhh each file in the folder
                        foreach(FileInfo file in folder.GetFiles())
                        {
                            files.Add(file); // add to root file
                        }
                    }
                }
            }

            // if the counts are the same, execute the move
            if(GetBackupFiles().Count() == files.Count)
            {
                for(int i = 0; i < GetBackupFiles().Count; i++)
                {
                    files[i].MoveTo(GetBackupFiles()[i].FullName); // move back to original position
                }
            }
            else
            {
                // show error
                AddMessage("ERROR: Cannot undo. File(s) modified externally.", 3);
            }
        }

    }
}
