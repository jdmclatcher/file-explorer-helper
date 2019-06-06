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
        private bool cleanupLast; // true if most recent action was the cleanup folder function
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

        public bool GetCleanupLast()
        {
            return this.cleanupLast;
        }

        public void SetCleanupLast(bool cleanupLast)
        {
            this.cleanupLast = cleanupLast;
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
            // if folder still exists, do code
            if(new DirectoryInfo(GetRootFolder().FullName).Exists)
            {
                // print out all paths of all files in folder to one .txt file
                // new blank string array of the same size as the number of files
                string[] paths = new string[GetListOfFiles().Count];
                for (int i = 0; i < GetListOfFiles().Count; i++)
                {
                    paths[i] = GetListOfFiles()[i].FullName;
                }
                File.WriteAllLines(GetRootFolder().FullName + "/" + detailsFilesName + ".txt", paths);
                AddMessage("Details printed to \"" + GetRootFolder() + "\\" + detailsFilesName + ".txt\"", 1);
            }
            // if not, print out error
            else
            {
                AddMessage("Folder moved/edited/deleted. Please browse of a new folder and try again.", 3);
            }
            
        }

        public void AddMessage(string message, int code)
        {
            MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
            mainWindow.AddMessageWindow(message, code);
        }

        // called at every major function, automatically sets
        // array to values currently in folder
        public void BackupFiles()
        {
            // save current state of files
            SetBackupFiles(GetListOfFiles()); // set the backup files value to current files in folder
        }

        #region Undo Actions
        // broken -- files getting corrupted - arrays not lined up correctly
        // general restore for all basic functions (that are not cleanup folder)
        public void RestoreRename()
        {

            // create and sort both arrays (by length because it is a constant) 
            List<FileInfo> mod = GetListOfFiles().OrderBy(f => f.Length).ToList(); // modified files - current files
            List<FileInfo> backup = GetBackupFiles().OrderBy(f => f.Length).ToList(); // backup files


            // FOR TESTING
            //Console.WriteLine();
            //Console.WriteLine("MOD\n");
            //for (int i = 0; i < mod.Count; i++)
            //{
            //    Console.WriteLine(mod[i]);

            //}
            //Console.WriteLine();
            //Console.WriteLine("BACKUP\n");
            //for (int i = 0; i < backup.Count; i++)
            //{
            //    Console.WriteLine(backup[i]);
            //}
            //Console.WriteLine();


            if (mod.Count != backup.Count)
            {
                AddMessage("Cannot undo. File(s) modified externally.", 3);
            }
            else
            {
                int numChanged = 0; // track num files actually reverted
                // loop through current files and replace them with the backed-up files
                for (int i = 0; i < mod.Count; i++)
                {
                    // check if same file, if not, add to count changed
                    // and perform the change
                    if (!mod[i].FullName.Equals(backup[i].FullName))
                    {
                        // move back to original location
                        mod[i].MoveTo(backup[i].FullName);
                        numChanged++;
                    }
                }
                // print out status message
                AddMessage("Action successfully undone. " + numChanged + " file(s) renamed to original name.", 1);
            }
        }

        public void RestoreCleanup()
        {
            // loop through each sub folder 
            // extract each file from the specified subfolders, and add to new list
            // drop all those files from the new list back into the root folder

            List<DirectoryInfo> folders = GetRootFolder().GetDirectories().ToList(); // convert to list
            List<FileInfo> files = new List<FileInfo>(); // make new list to store final files
            List<FileInfo> oldFiles = GetBackupFiles();
            List<DirectoryInfo> foldersToPurge = new List<DirectoryInfo>();



            //// run length of backup files
            //for (int i = 0; i < oldFiles.Count; i++)
            //{

            //    // check if there is a normal file still in the folder that didnt get organized
            //    // then check if file was not moved (is same) and add to list
            //    if (GetListOfFiles().Count != 0)
            //    {




            //        //if (oldFiles.Find(GetListOfFiles()[i]).FullName.Equals(GetListOfFiles()[i].FullName))
            //        //{
            //        //    files.Add(oldFiles[i]); // add to list
            //        //}
            //    }
            //}

            

            // loop through each folder
            foreach (DirectoryInfo folder in folders)
            {
                switch (folder.Name)
                {
                    case "Documents":
                        // then loop throuhh each file in the folder
                        foreach (FileInfo file in folder.GetFiles())
                        {
                            Console.WriteLine("Doc added.");
                            files.Add(file); // add to root file
                        }
                        foldersToPurge.Add(folder);
                        break;
                    case "Images":
                        // then loop throuhh each file in the folder
                        foreach (FileInfo file in folder.GetFiles())
                        {
                            Console.WriteLine("Image added.");
                            files.Add(file); // add to root file
                        }
                        foldersToPurge.Add(folder);
                        break;
                    case "Audio":
                        // then loop through each file in the folder
                        foreach (FileInfo file in folder.GetFiles())
                        {
                            files.Add(file); // add to root file
                        }
                        foldersToPurge.Add(folder);
                        break;
                    case "Videos":
                        // then loop through each file in the folder
                        foreach (FileInfo file in folder.GetFiles())
                        {
                            files.Add(file); // add to root file
                        }
                        foldersToPurge.Add(folder);
                        break;
                    case "Shortcuts":
                        // then loop through each file in the folder
                        foreach (FileInfo file in folder.GetFiles())
                        {
                            files.Add(file); // add to root file
                        }
                        foldersToPurge.Add(folder);
                        break;
                    case "Executables":
                        // then loop through each file in the folder
                        foreach (FileInfo file in folder.GetFiles())
                        {
                            files.Add(file); // add to root file
                        }
                        foldersToPurge.Add(folder);
                        break;
                }
            }

            // sort both arrays by length before the move
            files = files.OrderBy(f => f.Length).ToList();
            oldFiles = oldFiles.OrderBy(f => f.Length).ToList();

            // check if new file was created and is the same name as file in subfolders, 
            // error out saying file(s) mofified externally
            for(int i = 0; i < GetListOfFiles().Count; i++)
            {
                for(int x = 0; x < files.Count; x++)
                {
                    if (GetListOfFiles()[i].Name.Equals(files[x].Name))
                    {
                        AddMessage("Undo failed. File(s) modified externally.", 3);
                        return;
                    }
                }
                
            }

            // then compensate for file that didnt get moved
            foreach (FileInfo file in GetRootFolder().GetFiles().ToList())
            {
                // add each file that didnt get moved to have control over every file
                files.Add(file);
            }

            // then sort both arrays again 
            files = files.OrderBy(f => f.Length).ToList();
            oldFiles = oldFiles.OrderBy(f => f.Length).ToList();

            // if the counts are the same, execute the move
            if (oldFiles.Count == files.Count)
            {
                for (int i = 0; i < oldFiles.Count; i++)
                {
                    Console.WriteLine(oldFiles[i] + " old");
                    Console.WriteLine(files[i] + " files");
                    if (!files[i].FullName.Equals(oldFiles[i].FullName))
                    {
                        files[i].MoveTo(oldFiles[i].FullName); // move back to original 
                    }
                }
                AddMessage("Action successfully undone. " + oldFiles.Count + " files were reverted.", 1);
                foreach (DirectoryInfo folder in foldersToPurge)
                {
                    // only purge folder if has no contents
                    if(folder.GetFiles().Length == 0)
                    {
                        folder.Delete();
                    }
                }
            }
            else
            {
                // show error
                AddMessage("Undo failed. File(s) modified externally.", 3);
            }

        }

        #endregion

    }
}
