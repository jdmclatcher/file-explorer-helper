using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        private void Click_BrowseForFolder(object sender, RoutedEventArgs e)
        {
            utilClass.BrowseAndSelectFolder(); // open dialog to browse

            UpdateTexts(); // update text boxes of data
            EnableContent(); // enable the default disbaled buttons and content
            InitRenameChoices(); // set up the renaming choices for image rename
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
            input_imageRenameChoice.Items.Add("MM.DD.YYYY");                    // 0
            input_imageRenameChoice.Items.Add("[FILETYPE].MM.DD.YYYY");         // 1
            input_imageRenameChoice.Items.Add("MM_DD_YYYY");                    // 2
            input_imageRenameChoice.Items.Add("[FILETYPE]_MM_DD_YYYY");         // 3
            input_imageRenameChoice.Items.Add("MM_DD_YYYY - HHMM");             // 4
            input_imageRenameChoice.Items.Add("[FILETYPE]_MM_DD_YYYY - HHMM");  // 5
            input_imageRenameChoice.Items.Add("MM.DD.YYYY - HHMM");             // 6
            input_imageRenameChoice.Items.Add("[FILETYPE].MM.DD.YYYY - HHMM");  // 7
        }

        // called from cleanup folder button
        private void Click_CleanupFolder(object sender, RoutedEventArgs e)
        {
            if (!utilClass.GetRootFolder().Exists)
            {
                Console.WriteLine("ERROR: Folder was moved, deleted, or edited. Please browse for a new folder.");
            }
            else
            {
                // create instance of the Cleanup Folder class and call function on it
                CleanupFolder folderCleanup = new CleanupFolder(utilClass); // pass in the util class
                folderCleanup.Cleanup();
                UpdateTexts();
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
            if (input_remove.Text.Equals(null))
            {
                Console.WriteLine("Please provide input");
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
            }
            else
            {
                imageRenamer.RenameImages(choice);
            }
        }

    }
} 
