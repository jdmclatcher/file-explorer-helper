using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            utilClass.browseAndSetFolder(); // open dialog to browse
            folderNameText.Text = utilClass.getRootFolder().Name; // set text to name of folder
            // set files and subfolders number
            filesNumText.Text = utilClass.getNumFiles().ToString();
            subfoldersNumText.Text = utilClass.getNumSubFolders().ToString();
        }

    }
} 
