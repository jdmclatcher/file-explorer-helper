/// DEPRECATED ///

/*
 * Jonathan McLatcher
 * File Explorer Helper
 * 2020
 

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace FileExplorerHelper
{
    // handles the custom renaming of the common image naming convention
    class ImageRename
    {
        private Util util;
        private int duplicateCount; // number of duplicate files (append a ({num}) to )
        private int numChanged;

        public ImageRename(Util utilClass)
        {
            util = utilClass;
        }

        private int GetDuplicateCount()
        {
            return duplicateCount;
        }
        private void SetDuplicateCount(int duplicateCount)
        {
            this.duplicateCount = duplicateCount;
        }

        public int GetNumChanged()
        {
            return this.numChanged;
        }
        private void SetNumChanged(int numChanged)
        {
            this.numChanged = numChanged;
        }

        public void RenameImages(int choice)
        {
            util.BackupFiles();
            SetDuplicateCount(0);
            // get a list of the files
            List<FileInfo> files = util.GetListOfFiles();
            for(int i = 0; i < files.Count; i++)
            {
                if (files[i].Name.Substring(0, 4).Equals("IMG_") && checkNumeric(files[i], 4))
                {
                    RenameHelper(files[i], choice, "IMG");
                    SetNumChanged(GetNumChanged() + 1);
                }
                else if(files[i].Name.Substring(0, 4).Equals("VID_") && checkNumeric(files[i], 4))
                {
                    RenameHelper(files[i], choice, "VID");
                    SetNumChanged(GetNumChanged() + 1);
                }
                else if(files[i].Name.Substring(0, 5).Equals("PANO_") && checkNumeric(files[i], 5))
                {
                    RenameHelper(files[i], choice, "PANO");
                    SetNumChanged(GetNumChanged() + 1);
                }
            }
        }

        private bool checkNumeric(FileInfo file, int startIndex)
        {
            // return true if the exact scheme - so only modifies files using convention
            return file.Name.Substring(startIndex, 8).All(char.IsDigit) && file.Name.Substring(startIndex + 9, 4).All(char.IsDigit);
        }

        private void RenameHelper(FileInfo file, int choice, string type)
        {
            // check if the file will be the same name as a file that already exists
            // if so, append a duplicate indicator on the end of the file name
            if(File.Exists(util.GetRootFolder() + "/" + NewFileName(file, choice, type) + file.Extension))
            {
                // update duplicate count
                SetDuplicateCount(GetDuplicateCount() + 1); // add 1 to current duplicate count
                // then rename file
                file.MoveTo(util.GetRootFolder() + "/" + NewFileName(file, choice, type) + " (" + GetDuplicateCount() + ")" + file.Extension);
                Console.WriteLine("File already exists in destination.");
            }
            // if not, rename normally
            else
            {
                SetDuplicateCount(0); // reset duplicate count
                // rename file
                file.MoveTo(util.GetRootFolder() + "/" + NewFileName(file, choice, type) + file.Extension);
            }
        }

        // does NOT include the .extension
        private string NewFileName(FileInfo file, int choice, string type)
        {
            // use the old file to create the new file name
            // extract the data from the passed in file
            int datePos = file.Name.IndexOf("_") + 1;
            string date = file.Name.Substring(datePos, 8); // parse the complete date

            // modify the displaying of the date
            string year = date.Substring(0, 4);
            string month = date.Substring(4, 2);
            string day = date.Substring(6, 2);

            // get the data after the second underscore
            int timePos = file.Name.IndexOf("_", datePos) + 1;
            string time = file.Name.Substring(timePos, 6);

            // parse the time
            string hour = time.Substring(0, 2);
            string min = time.Substring(2, 2);

            // convert to standard american time
            string newHourStr;
            if (Convert.ToInt32(hour) > 12)
            {
                newHourStr = (Convert.ToInt32(hour) - 12).ToString(); // subtract 12 from time
            }
            // if not a 2 digit number...
            else if ((Convert.ToInt32(hour) != 10) && (Convert.ToInt32(hour) != 11) && (Convert.ToInt32(hour) != 12))
            {
                newHourStr = "0" + Convert.ToInt32(hour); // ...add 0 because its AM
            }
            else
            {
                newHourStr = Convert.ToInt32(hour).ToString();
            }

            // based on the XAML selection, return different file naming schemes
            switch (choice)
            {
                case 0:
                    // change to MM.DD.YYYY
                    return month + "." + day + "." + year;
                case 1:
                    // change to [FILETYPE] - MM.DD.YYY
                    return type + " - " + month + "." + day + "." + year;
                case 2:
                    // change to MMDDYYYY
                    return month + day + year;
                case 3:
                    // change to [FILETYPE] - MMDDYYYY
                    return type + " - " + month + day + year;
                case 4:
                    // change to MMDDYYYY - HHMM
                    return month + day + year + " - " + newHourStr + min;
                case 5:
                    // change to [FILETYPE] - MMDDYYYY - HHMM
                    return type + " - " + month + day + year + " - " + newHourStr + min;
                case 6:
                    // change to MM.DD.YYYY - HH MM
                    return month + "." + day + "." + year + " - " + newHourStr + min;
                case 7:
                    // change to [FILETYPE] - MM.DD.YYYY - HHMM
                    return type + " - " + month + "." + day + "." + year + " - " + newHourStr + min;
                default:
                    // return default name if anything else (without the extension) 
                    return Path.GetFileNameWithoutExtension(file.Name);
            }
        }
    }
}
*/
