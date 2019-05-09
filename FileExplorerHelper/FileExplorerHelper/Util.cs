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
        private FileInfo rootFolder;

        private int subFolderCount;
        private int fileCount;
        #region Constructor and Getters/Setters
        public FileInfo GetRootFolder()
        {
            return this.rootFolder;
        }

        public void SetRootFolder(FileInfo rootFolder)
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
            while (Dialog.ShowDialog() != DialogResult.OK)
            {
                Dialog.Reset();
            }
            ;
            // sets the root folder to the selected folder
            SetRootFolder(new FileInfo(Dialog.SelectedPath));
            CountFilesAndFolders(); // update file and subfolder count
        }
    }
}
