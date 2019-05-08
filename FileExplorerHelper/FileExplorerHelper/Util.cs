using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace FileExplorerHelper
{
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
        public Util()
        {
            rootFolder = null;
        }
        public FileInfo getRootFolder()
        {
            return this.rootFolder;
        }

        public void setRootFolder(FileInfo rootFolder)
        {
            this.rootFolder = rootFolder;
        }

        public int getNumSubFolders()
        {
            return this.subFolderCount;
        }
        private void setNumSubFolders(int subFolderCount)
        {
            this.subFolderCount = subFolderCount;
        }

        public int getNumFiles()
        {
            return this.fileCount;
        }
        private void setNumFiles(int fileCount)
        {
            this.fileCount = fileCount;
        }
        #endregion

        #region Helper Methods
        // function that opens dialog to prompt input of desired folder
        public void browseAndSetFolder()
        {       
            FolderBrowserDialog Dialog = new FolderBrowserDialog();
            while (Dialog.ShowDialog() != DialogResult.OK)
            {
                Dialog.Reset();
            }
            ;
            // sets the root folder to the selected folder
            setRootFolder(new FileInfo(Dialog.SelectedPath));
            countFilesAndFolders(); // update file and subfolder count
        }

        // count of files and folders in the folder
        private void countFilesAndFolders()
        {
            FileInfo[] files = new DirectoryInfo(rootFolder.FullName).GetFiles();
            Console.WriteLine(files);
            int fileCount = 0;
            int folderCount = 0;
            for (int i = 0; i < files.Length; i++)
            {
                // TODO - check if file or folder
                // increase count based on if file or folder
                if (File.Exists(files[i].FullName))
                {
                    Console.WriteLine("File found.");
                    fileCount++;
                }
                else if (Directory.Exists(files[i].FullName))
                {
                    Console.WriteLine("Sub-Folder found.");
                    folderCount++;
                }
            }
            setNumFiles(fileCount);
            setNumSubFolders(folderCount);
        }

        #endregion
    }
}
