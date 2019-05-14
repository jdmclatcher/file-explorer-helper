/*
 * Jonathan McLatcher
 * File Explorer Helper
 * 2019
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorerHelper
{
    class FindAndRemove
    {
        private Util util;
        private int numChanged;
        public FindAndRemove(Util utilClass)
        {
            util = utilClass;
        }

        private int GetNumChanged()
        {
            return this.numChanged;
        }

        private void SetNumChanged(int numChanged)
        {
            this.numChanged = numChanged;
        }

        public void FindAndRemoveFiles(string toRemove)
        {
            List<FileInfo> files = util.GetListOfFiles(); // get all files
            util.BackupFiles(); // set the values of the current state of files in folder
            // loop through and apply changes/replacements
            for (int i = 0; i < files.Count; i++)
            {
                // apply replacements to moved file
                // by replacing any instances of the string with another string
                // (excluding the file extension itself)

                string newName = util.GetRootFolder() + "/" + files[i].Name.Substring(0, files[i].Name.LastIndexOf(".")).Replace(toRemove, "") + files[i].Extension;

                // if they will be the same file regardless (no replacements were made)
                if ((util.GetRootFolder() + "/" + files[i].Name).Equals(newName))
                {
                    Console.WriteLine(files[i] + " skipped.");
                }
                // check ahead of time if going to cause same file name error
                else if (File.Exists(newName))
                {
                    // send message and dont replace
                    util.AddMessage("WARNING: File \"" + files[i].Name.Substring(0, files[i].Name.LastIndexOf(".")).Replace(toRemove, "") + files[i].Extension + "\" already exists. No changes made to \"" + files[i].Name + "\"", 2);
                }
                else
                {
                    // increase count of files replace
                    SetNumChanged(GetNumChanged() + 1);
                    files[i].MoveTo(newName);
                }
            }
            util.AddMessage("MESSAGE: All instances of \"" + toRemove + "\" removed in all files. " + GetNumChanged() + " file(s) modified.", 1);
        }

    }
}
