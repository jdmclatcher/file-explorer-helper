using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

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

        public void ReplaceAllFiles(string toRemove, string toReplace)
        {
            List<FileInfo> files = GetListOfFiles(); // get all files
            // loop through and apply changes/replacements
            for(int i = 0; i < files.Count; i++)
            {
                // apply replacements to moved file
                // by replacing any instances of the string with another string
                // (excluding the file extension itself)
                files[i].MoveTo(GetRootFolder() + "/" + files[i].Name.Substring(0, files[i].Name.LastIndexOf(".")).Replace(toRemove, toReplace) + files[i].Extension);
            }
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

        // adds a message to the output window
        public void AddMessage(string message, int severity)
        {
            // TODO - fix this
            // create ref to main window xaml script and call function
            MainWindow window = new MainWindow();
            window.AddMessageWindow(message, severity);
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
    }
}
