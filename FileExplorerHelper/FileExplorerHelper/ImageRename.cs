using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileExplorerHelper
{
    // handles the custom renaming of the common image naming convention
    class ImageRename
    {
        private Util util;
        private int duplicateCount; // number of duplicate files (append a ({num}) to )
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

        public void RenameImages(int choice)
        {
            int numChanged = 0;
            SetDuplicateCount(0);
            // get a list of the files
            List<FileInfo> files = util.GetListOfFiles();
            for(int i = 0; i < files.Count; i++)
            {
                if(files[i].Name.Substring(0, 3).Equals("IMG"))
                {
                    RenameHelper(files[i], choice, "IMG");
                    numChanged++;
                }
                else if(files[i].Name.Substring(0, 3).Equals("VID"))
                {
                    RenameHelper(files[i], choice, "VID");
                    numChanged++;
                }
                else if(files[i].Name.Substring(0, 4).Equals("PANO"))
                {
                    RenameHelper(files[i], choice, "PANO");
                    numChanged++;
                }
            }
            if(numChanged == 0)
            {
                Console.WriteLine("No files were renamed.");
            }
            else
            {
                Console.WriteLine(numChanged + " file(s) were renamed.");
            }

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
                file.MoveTo(util.GetRootFolder() + "/" + NewFileName(file, choice, type) + "(" + GetDuplicateCount() + ")" + file.Extension);
            }
            // if not, rename normally
            else
            {
                SetDuplicateCount(0); // reset duplicate count
                // rename file
                file.MoveTo(util.GetRootFolder() + "/" + NewFileName(file, choice, type) + file.Extension);
                Console.WriteLine("File already exists in destination.");
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
                    // change to [FILETYPE].MM.DD.YYY
                    return type + "." + month + "." + day + "." + year;
                case 2:
                    // change to MM_DD_YYYY
                    return month + "_" + day + "_" + year;
                case 3:
                    // change to [FILETYPE]_MM_DD_YYYY
                    return type + "_" + month + "_" + day + "_" + year;
                case 4:
                    // change to MM_DD_YYYY - HH MM
                    return month + "_" + day + "_" + year + " - " + newHourStr + min;
                case 5:
                    // change to [FILETYPE]_MM_DD_YYYY - HHMM
                    return type + "_" + month + "_" + day + "_" + year + " - " + newHourStr + min;
                case 6:
                    // change to MM.DD.YYYY - HH MM
                    return month + "." + day + "." + year + " - " + newHourStr + min;
                case 7:
                    // change to [FILETYPE].MM.DD.YYYY - HHMM
                    return type + "." + month + "." + day + "." + year + " - " + newHourStr + min;
                default:
                    // return default name if anything else (without the extension) 
                    return Path.GetFileNameWithoutExtension(file.Name);
            }
        }
    }
}
