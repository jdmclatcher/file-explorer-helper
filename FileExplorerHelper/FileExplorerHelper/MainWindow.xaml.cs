using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FileExplorerHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Util utilClass; // ref to util class

        public MainWindow()
        {
            InitializeComponent(); // create xaml
            utilClass = new Util(); // create instance of utility class
        }

        public void SetText(string message)
        {
            text_output.Text = message;
        }

        private void Click_BrowseForFolder(object sender, RoutedEventArgs e)
        {
            utilClass.BrowseAndSelectFolder(); // open dialog to browse

            if (utilClass.GetCanUseFunctions())
            {
                UpdateTexts(); // update text boxes of data
                EnableContent(); // enable the default disbaled buttons and content
                InitRenameChoices(); // set up the renaming choices for image 
                ResetMessage();
            }
            else
            {
                // provide message
                AddMessageWindow("WARNING: No folder was selected. Please select a folder.", 2);
            }
        }

        private void EnableContent()
        {
            button_cleanupFolder.IsEnabled = true;
            button_findAndReplace.IsEnabled = true;
            button_findAndRemove.IsEnabled = true;
            input_find.IsEnabled = true;
            input_remove.IsEnabled = true;
            input_replace.IsEnabled = true;
            button_imageRename.IsEnabled = true;
            input_imageRenameChoice.IsEnabled = true;
            button_printDetails.IsEnabled = true;
        }

        private void UpdateTexts()
        {
            utilClass.CountFilesAndFolders();
            text_folderName.Text = utilClass.GetRootFolder().Name; // set text to name of folder
            // set files and subfolders number
            text_filesNum.Text = utilClass.GetNumFiles().ToString();
            text_subFoldersNum.Text = utilClass.GetNumSubFolders().ToString();
        }

        private void InitRenameChoices()
        {
            // TODO - fix bug where first underscore is omitted from choice
            input_imageRenameChoice.Items.Clear(); // clear all the choices before adding again
            // add all the choices to the combo box
            input_imageRenameChoice.Items.Add("MM.DD.YYYY");                     // 0
            input_imageRenameChoice.Items.Add("[FILETYPE].MM.DD.YYYY");          // 1
            input_imageRenameChoice.Items.Add("MM_DD_YYYY");                     // 2
            input_imageRenameChoice.Items.Add("[FILETYPE]_MM_DD_YYYY");          // 3
            input_imageRenameChoice.Items.Add("MM_DD_YYYY - HHMM");              // 4
            input_imageRenameChoice.Items.Add("[FILETYPE]_MM_DD_YYYY - HHMM");   // 5
            input_imageRenameChoice.Items.Add("MM.DD.YYYY - HHMM");              // 6
            input_imageRenameChoice.Items.Add("[FILETYPE].MM.DD.YYYY - HHMM");   // 7
        }

        // called from cleanup folder button
        private void Click_CleanupFolder(object sender, RoutedEventArgs e)
        {
            utilClass.GetRootFolder().Refresh(); // refresh info of root folder
            if (!utilClass.GetRootFolder().Exists)
            {
                Console.WriteLine("ERROR: Folder was moved, deleted, or edited. Please browse for a new folder.");
                AddMessageWindow("ERROR: Folder was moved, deleted, or edited. Please browse for a new folder.", 2);
                Console.WriteLine(utilClass.GetRootFolder());
            }
            else
            {
                // create instance of the Cleanup Folder class and call function on it
                CleanupFolder folderCleanup = new CleanupFolder(utilClass); // pass in the util class
                folderCleanup.Cleanup();
                UpdateTexts();
                Console.WriteLine(utilClass.GetRootFolder());
            }
            
        }

        // called from find and replace button
        private void Click_FindAndReplace(object sender, RoutedEventArgs e)
        {
            FindAndReplace findAndReplace = new FindAndReplace(utilClass);
            // check if there is input, and distribute it accordingly
            if (input_find.Text.Equals(null) || input_replace.Text.Equals(null) || input_find.Text.Equals("") || input_replace.Text.Equals(""))
            {
                Console.WriteLine("Please provide input");
                AddMessageWindow("ERROR: No input provided. Please enter text to find and replace.", 2);
            }
            else
            {
                findAndReplace.FindAndReplaceFiles(input_find.Text, input_replace.Text);
                // then clear the text fields
                input_find.Clear();
                input_replace.Clear();
            }
            
        }

        private void Click_FindAndRemove(object sender, RoutedEventArgs e)
        {
            FindAndRemove findAndRemove = new FindAndRemove(utilClass);
            if (input_remove.Text.Equals(null) || input_remove.Text.Equals(""))
            {
                Console.WriteLine("Please provide input");
                AddMessageWindow("ERROR: No input provided. Please enter text to find and remove.", 2);
            }
            else
            {
                findAndRemove.FindAndRemoveFiles(input_remove.Text);
                // then clear the text field
                input_remove.Clear();
            }

        }

        private void Click_RenameImages(object sender, RoutedEventArgs e)
        {
            ImageRename imageRenamer = new ImageRename(utilClass);
            int choice = input_imageRenameChoice.SelectedIndex;
            if(choice == -1)
            {
                Console.WriteLine("No option selected. Please select an option.");
                AddMessageWindow("ERROR: No option selected. Please select an option.", 2);
            }
            else
            {
                imageRenamer.RenameImages(choice);
            }
        }

        public void Click_PrintDetails(object sender, RoutedEventArgs e)
        {
            // function that will print out all the files to a .txt file
            utilClass.PrintDetails();
            AddMessageWindow("MESSAGE: Details printed to \"" + utilClass.GetRootFolder() + "/" + utilClass.detailsFilesName + ".txt\"", 1);
        }

        public void AddMessageWindow(string message, int severity)
        {
            // change text color based on message severity
            // 1 = green (MESSAGE), 2 = red (ERROR)
            switch (severity)
            {
                case 1:
                    text_output.Foreground = Brushes.Green;
                    break;
                case 2:
                    text_output.Foreground = Brushes.Red;
                    break;
                default:
                    text_output.Foreground = Brushes.Black;
                    break;
            }

            // change font
            text_output.FontFamily = new FontFamily("Consolas");
            text_output.Text = message; 
        }

        // set output text to nothing
        public void ResetMessage()
        {
            text_output.Text = "";
        }

    }
} 
