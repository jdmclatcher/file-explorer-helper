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
        private StackPanel scrollViewerBG; // ref to canvas for scroll viewer
        public MainWindow()
        {
            InitializeComponent(); // create xaml
            Init_OutputBox();
            utilClass = new Util(); // create instance of utility class
        }

        private void Click_BrowseForFolder(object sender, RoutedEventArgs e)
        {
            utilClass.BrowseAndSelectFolder(); // open dialog to browse

            if (utilClass.GetCanUseFunctions())
            {
                UpdateTexts(); // update text boxes of data
                EnableContent(); // enable the default disbaled buttons and content
                InitRenameChoices(); // set up the renaming choices for image 
                ClearOutput();
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
            // TODO - finalize these options
            input_imageRenameChoice.Items.Clear(); // clear all the choices before adding again
            // add all the choices to the combo box
            input_imageRenameChoice.Items.Add("MM.DD.YYYY");                     // 0
            input_imageRenameChoice.Items.Add("[FILETYPE] - MM.DD.YYYY");        // 1
            input_imageRenameChoice.Items.Add("MMDDYYYY");                       // 2
            input_imageRenameChoice.Items.Add("[FILETYPE] - MMDDYYYY");          // 3
            input_imageRenameChoice.Items.Add("MMDDYYYY - HHMM");                // 4
            input_imageRenameChoice.Items.Add("[FILETYPE] - MMDDYYYY - HHMM");   // 5
            input_imageRenameChoice.Items.Add("MM.DD.YYYY - HHMM");              // 6
            input_imageRenameChoice.Items.Add("[FILETYPE] - MM.DD.YYYY - HHMM"); // 7
        }

        // called from cleanup folder button
        private void Click_CleanupFolder(object sender, RoutedEventArgs e)
        {
            utilClass.GetRootFolder().Refresh(); // refresh info of root folder
            if (!utilClass.GetRootFolder().Exists)
            {
                Console.WriteLine("ERROR: Folder was moved, deleted, or edited. Please browse for a new folder.");
                AddMessageWindow("ERROR: Folder was moved, deleted, or edited. Please browse for a new folder.", 3);
            }
            else
            {
                // create instance of the Cleanup Folder class and call function on it
                CleanupFolder folderCleanup = new CleanupFolder(utilClass); // pass in the util class
                folderCleanup.Cleanup();
                UpdateTexts();
                Console.WriteLine("MESSAGE: Cleanup complete. All valid files were sorted.");
                AddMessageWindow("MESSAGE: Cleanup complete. All valid files were sorted.", 1);
                // button_undo.IsEnabled = true;
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
                AddMessageWindow("ERROR: No input provided. Please enter text to find and replace.", 3);
            }
            else
            {
                findAndReplace.FindAndReplaceFiles(input_find.Text, input_replace.Text);
                // then clear the text fields
                input_find.Clear();
                input_replace.Clear();
                // button_undo.IsEnabled = true; // re-enable the button
            }
            
        }

        private void Click_FindAndRemove(object sender, RoutedEventArgs e)
        {
            FindAndRemove findAndRemove = new FindAndRemove(utilClass);
            if (input_remove.Text.Equals(null) || input_remove.Text.Equals(""))
            {
                Console.WriteLine("Please provide input");
                AddMessageWindow("ERROR: No input provided. Please enter text to find and remove.", 3);
            }
            else
            {
                findAndRemove.FindAndRemoveFiles(input_remove.Text);
                // then clear the text field
                input_remove.Clear();
                // button_undo.IsEnabled = true; // re-enable the button
            }

        }

        private void Click_RenameImages(object sender, RoutedEventArgs e)
        {
            ImageRename imageRenamer = new ImageRename(utilClass);
            int choice = input_imageRenameChoice.SelectedIndex;
            if(choice == -1)
            {
                Console.WriteLine("No option selected. Please select an option.");
                AddMessageWindow("ERROR: No option selected. Please select an option.", 3);
            }
            else
            {
                imageRenamer.RenameImages(choice);
                if (imageRenamer.GetNumChanged() == 0)
                {
                    Console.WriteLine("No files were renamed.");
                    AddMessageWindow("MESSAGE: Rename complete. No files were renamed.", 1);
                }
                else
                {
                    Console.WriteLine(imageRenamer.GetNumChanged() + " file(s) were renamed.");
                    AddMessageWindow("MESSAGE: Rename complete. " + imageRenamer.GetNumChanged() + " file(s) were renamed.", 1);
                    // button_undo.IsEnabled = true; // re-enable the button
                }
            }

        }

        public void Click_PrintDetails(object sender, RoutedEventArgs e)
        {
            // check if that file already exits
            if (new FileInfo(utilClass.GetRootFolder().FullName + "/" + utilClass.detailsFilesName + ".txt").Exists)
            {
                // and prompt an override
                if (MessageBox.Show("File \"" + utilClass.detailsFilesName +  "\" already exists. Would you like to overwrite it.", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    // cancel
                    return;
                }
                else
                {
                    // proceed
                    // function that will print out all the files to a .txt file
                    utilClass.PrintDetails();
                    // AddMessageWindow("MESSAGE: Details printed to \"" + utilClass.GetRootFolder() + "\\" + utilClass.detailsFilesName + ".txt\"", 1);
                }
            }
            else
            {
                // proceed normally if file doesnt exists
                utilClass.PrintDetails();
                // AddMessageWindow("MESSAGE: Details printed to \"" + utilClass.GetRootFolder() + "\\" + utilClass.detailsFilesName + ".txt\"", 1);
            }

        }

        // TODO - fix undo button
        // function to call to undo previous action
        //public void Click_Undo(object sender, RoutedEventArgs e)
        //{
        //    // TODO - add a check to determine if need to use restore cleanup or normal restore
        //    // utilClass.RestoreCleanup();
        //    button_undo.IsEnabled = false;


        //    // call util function
        //    utilClass.RestoreRename();

        //}

        // called to create canvas under scroll viewer
        public void Init_OutputBox()
        {
            scrollViewerBG = new StackPanel(); // set to new black stack panel
            scrollViewer_output.Content = scrollViewerBG; // make it child fo scroll viewer
        }

        public void AddMessageWindow(string message, int severity)
        {
            // change text color and background color based on message severity
            // 1 = green (MESSAGE), 2 = yellow (WARNING), 3 = red (ERROR)
            TextBlock textBlock = new TextBlock(); // make a new textblock 
            // scrollViewerGrid.Children.Add(textBlock); // make child of stack panel
            scrollViewerBG.Children.Add(textBlock);
            switch (severity)
            {
                case 1:
                    textBlock.Foreground = Brushes.Green;
                    // textBlock.Background = Brushes.LightGreen;
                    break;
                case 2:
                    textBlock.Foreground = Brushes.Orange;
                    // textBlock.Background = Brushes.LightYellow;
                    break;
                case 3:
                    textBlock.Foreground = Brushes.Red;
                    // textBlock.Background = Brushes.Red; // TODO - maybe wrong color?
                    break;
                default:
                    textBlock.Foreground = Brushes.Black;
                    break;
            }

            // change font
            textBlock.FontFamily = new FontFamily("Consolas");
            textBlock.Text = DateTime.Now.ToString("hh:mm:ss tt") + ": " + message;  // set text to message (plus the time)
            textBlock.TextWrapping = TextWrapping.Wrap; // wrap text in case it is too long
            scrollViewer_output.ScrollToEnd(); // auto scroll to latest addition
        }

        // function that will clear all text in the output log
        public void ClearOutput()
        {
            scrollViewerBG.Children.Clear(); // remove all the textblock children in the BG
        }

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
                    utilClass.SetRootFolder(new DirectoryInfo(files[0]));
                    // perform init functions
                    UpdateTexts(); // update text boxes of data
                    EnableContent(); // enable the default disbaled buttons and content
                    InitRenameChoices(); // set up the renaming choices for image 
                    ClearOutput(); // reset console
                }
                else
                {
                    AddMessageWindow("ERROR: File dragged in is not a folder. Please drag in a folder.", 3);
                }
                
            }
        }
    }

} 
