/*
 * Jonathan McLatcher
 * File Explorer Helper
 * 2020
 */

using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FileExplorerHelper
{
    public partial class MainWindow : Window
    {
        #region INIT
        Util util; // ref to util class
        private StackPanel scrollViewerBG; // ref to canvas for scroll viewer
        public MainWindow()
        {
            InitializeComponent(); // create xaml
            Init_OutputBox();
            util = new Util(); // create instance of utility class
        }

        // called to create canvas under scroll viewer
        public void Init_OutputBox()
        {
            scrollViewerBG = new StackPanel(); // set to new black stack panel
            scrollViewer_output.Content = scrollViewerBG; // make it child fo scroll viewer
        }
        #endregion

        #region Helper Methods
        private void EnableContent()
        {
            button_cleanupFolder.IsEnabled = true;
            button_findAndReplace.IsEnabled = true;
            button_findAndRemove.IsEnabled = true;
            input_find.IsEnabled = true;
            input_remove.IsEnabled = true;
            input_replace.IsEnabled = true;
            //button_imageRename.IsEnabled = true;
            //input_imageRenameChoice.IsEnabled = true;
            listView_filesInFolder.IsEnabled = true;
            //button_printDetails.IsEnabled = true;
        }

        private void UpdateTexts()
        {
            util.CountFilesAndFolders();
            PopulateListView();
            text_folderName.Text = util.GetRootFolder().Name; // set text to name of folder
            // set files and subfolders number
            text_filesNum.Text = util.GetNumFiles().ToString();
            text_subFoldersNum.Text = util.GetNumSubFolders().ToString();
        }

        //private void InitRenameChoices()
        //{
        //    input_imageRenameChoice.Items.Clear(); // clear all the choices before adding again
        //    // add all the choices to the combo box
        //    input_imageRenameChoice.Items.Add("MM.DD.YYYY");                     // 0
        //    input_imageRenameChoice.Items.Add("[FILETYPE] - MM.DD.YYYY");        // 1
        //    input_imageRenameChoice.Items.Add("MMDDYYYY");                       // 2
        //    input_imageRenameChoice.Items.Add("[FILETYPE] - MMDDYYYY");          // 3
        //    input_imageRenameChoice.Items.Add("MMDDYYYY - HHMM");                // 4
        //    input_imageRenameChoice.Items.Add("[FILETYPE] - MMDDYYYY - HHMM");   // 5
        //    input_imageRenameChoice.Items.Add("MM.DD.YYYY - HHMM");              // 6
        //    input_imageRenameChoice.Items.Add("[FILETYPE] - MM.DD.YYYY - HHMM"); // 7
        //}

        public void AddMessageWindow(string message, int code)
        {
            // change text color and background color based on message severity
            // 1 = green (MESSAGE), 2 = yellow (WARNING), 3 = red (ERROR)
            TextBlock textBlock = new TextBlock(); // make a new textblock 
            // scrollViewerGrid.Children.Add(textBlock); // make child of stack panel
            scrollViewerBG.Children.Add(textBlock);

            textBlock.Text = DateTime.Now.ToString("hh:mm:ss tt:"); // first part of message is the time
            switch (code)
            {
                case 1:
                    textBlock.Foreground = Brushes.Green;
                    textBlock.Text += " [MESSAGE] "; // add the error code
                    break;
                case 2:
                    textBlock.Foreground = Brushes.Orange;
                    textBlock.Text += " [WARNING] ";
                    break;
                case 3:
                    textBlock.Foreground = Brushes.Red;
                    textBlock.Text += " [ERROR!] ";
                    break;
                default:
                    textBlock.Foreground = Brushes.Black;
                    break;
            }

            // change font
            textBlock.FontFamily = new FontFamily("Consolas");
            textBlock.Text += message;  // set text to message (plus the time)
            textBlock.TextWrapping = TextWrapping.Wrap; // wrap text in case it is too long
            scrollViewer_output.ScrollToEnd(); // auto scroll to latest addition
        }

        // function that will clear all text in the output log
        public void ClearOutput()
        {
            scrollViewerBG.Children.Clear(); // remove all the textblock children in the BG
        }
        #endregion

        #region OnClick Methods
        private void Click_BrowseForFolder(object sender, RoutedEventArgs e)
        {
            util.BrowseAndSelectFolder(); // open dialog to browse

            if (util.GetCanUseFunctions())
            {
                UpdateTexts(); // update text boxes of data
                EnableContent(); // enable the default disbaled buttons and content
                // InitRenameChoices(); // set up the renaming choices for image 
                ClearOutput();
            }
            else
            {
                // provide message
                AddMessageWindow("No folder was selected. Please select a folder to continue.", 2);
            }
        }

        // called from cleanup folder button
        private void Click_CleanupFolder(object sender, RoutedEventArgs e)
        {
            util.GetRootFolder().Refresh(); // refresh info of root folder
            if (!util.GetRootFolder().Exists)
            {
                Console.WriteLine("Folder was moved, deleted, or edited. Please browse for a new folder.");
                AddMessageWindow("Folder was moved, deleted, or edited. Please browse for a new folder.", 3);
            }
            else
            {
                // create instance of the Cleanup Folder class and call function on it
                CleanupFolder folderCleanup = new CleanupFolder(util); // pass in the util class
                folderCleanup.Cleanup();
                UpdateTexts();
                Console.WriteLine("Cleanup complete. All valid files were sorted.");
                AddMessageWindow("Cleanup complete. All valid files were sorted.", 1);
                util.SetCleanupLast(true);
                // button_undo.IsEnabled = true;
            }

            UpdateTexts(); // update count of files and subfolders
        }

        // called from find and replace button
        private void Click_FindAndReplace(object sender, RoutedEventArgs e)
        {
            FindAndReplace findAndReplace = new FindAndReplace(util);
            // check if there is input, and distribute it accordingly
            if (input_find.Text.Equals(null) || input_replace.Text.Equals(null) || input_find.Text.Equals("") || input_replace.Text.Equals(""))
            {
                Console.WriteLine("Please provide input");
                AddMessageWindow("No input provided. Please enter text to find and replace.", 3);
            }
            else
            {
                findAndReplace.FindAndReplaceFiles(input_find.Text, input_replace.Text);
                // then clear the text fields
                input_find.Clear();
                input_replace.Clear();
                util.SetCleanupLast(false);
                button_undo.IsEnabled = true; // re-enable the button
            }
            UpdateTexts(); // update count of files and subfolders
        }

        private void Click_FindAndRemove(object sender, RoutedEventArgs e)
        {
            FindAndRemove findAndRemove = new FindAndRemove(util);
            if (input_remove.Text.Equals(null) || input_remove.Text.Equals(""))
            {
                Console.WriteLine("Please provide input");
                AddMessageWindow("No input provided. Please enter text to find and remove.", 3);
            }
            else
            {
                findAndRemove.FindAndRemoveFiles(input_remove.Text);
                // then clear the text field
                input_remove.Clear();
                util.SetCleanupLast(false);
                button_undo.IsEnabled = true; // re-enable the button
            }
            UpdateTexts(); // update count of files and subfolders
        }

        //private void Click_RenameImages(object sender, RoutedEventArgs e)
        //{
        //    ImageRename imageRenamer = new ImageRename(util);
        //    int choice = input_imageRenameChoice.SelectedIndex;
        //    if (choice == -1)
        //    {
        //        Console.WriteLine("No option selected. Please select an option.");
        //        AddMessageWindow("No option selected. Please select an option.", 3);
        //    }
        //    else
        //    {
        //        imageRenamer.RenameImages(choice);
        //        if (imageRenamer.GetNumChanged() == 0)
        //        {
        //            Console.WriteLine("No files were renamed.");
        //            AddMessageWindow("Rename complete. No files were renamed.", 1);
        //        }
        //        else
        //        {
        //            Console.WriteLine(imageRenamer.GetNumChanged() + " file(s) were renamed.");
        //            AddMessageWindow("Rename complete. " + imageRenamer.GetNumChanged() + " file(s) were renamed.", 1);
        //            util.SetCleanupLast(false);
        //            button_undo.IsEnabled = true; // re-enable the button
        //        }
        //    }
        //    UpdateTexts(); // update count of files and subfolders
        //}

        // function to call to undo previous action
        private void Click_Undo(object sender, RoutedEventArgs e)
        {
            // disable the button again
            button_undo.IsEnabled = false;

            // check to determine if last action was the cleanup folder function
            if (util.GetCleanupLast())
            {
                Console.WriteLine("Performing a cleanup restore...");
                util.RestoreCleanup();
            }
            else
            {
                // normal restore if not
                Console.WriteLine("Performing a rename restore...");
                util.RestoreRename();
            }
            UpdateTexts(); // update count of files and subfolders
        }

        //clear console output
        private void Click_ClearOutput(object sender, RoutedEventArgs e)
        {
            ClearOutput();
        }
        #endregion

        #region Special Function
        // drag and drop folder functionality
        public void Drop_Folder(object sender, DragEventArgs e)
        {
            // if a file was dropped in
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // extract args
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                FileAttributes file = File.GetAttributes(files[0]); // extract file details
                if (file.HasFlag(FileAttributes.Directory))
                {
                    // set root folder of util class
                    util.SetRootFolder(new DirectoryInfo(files[0]));
                    // perform init functions
                    UpdateTexts(); // update text boxes of data
                    EnableContent(); // enable the default disbaled buttons and content
                    // InitRenameChoices(); // set up the renaming choices for image 
                    ClearOutput(); // reset console
                }
                else
                {
                    AddMessageWindow("File dragged in is not a folder. Please drag in a folder.", 3);
                }

            }
        }
        #endregion

        private void PopulateListView()
        {
            if (util.GetListOfFiles().Count == 0)
            {
                listView_filesInFolder.ItemsSource = new string[] { "NO FILES FOUND" };
            }
            else
            {
                listView_filesInFolder.ItemsSource = util.GetListOfFiles();
            }
        }
    }
} 
