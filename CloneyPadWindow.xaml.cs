using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace CloneyPad
{
    /// <summary>
    /// Interaction logic for CloneyPadWindow.xaml
    /// </summary>

    public partial class CloneyPadWindow : Window
    {
        // Fields
        private bool hasFileBeenSaved = false;
        private string fileName = "untitled.txt";

        public CloneyPadWindow()
        {
            InitializeComponent();
        }

        private void txtBxMainTextView_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblCharCount.Content = txtBxMainTextView.Text.Length.ToString();
        }

        private void cmdNew_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void cmdNew_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // Do you need this? I don't think so, remove it later.
        }

        private void cmdOpen_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string filePath = "";
            //if (!hasFileBeenSaved)
            //{
            //    MessageBox.Show("This is where you ask the user if they want to save the file!");
            //}

            // Create and open the OpenFileDialog
            OpenFileDialog openFile = new OpenFileDialog();
            if(openFile.ShowDialog() == true) // If the user clicked OK in the open dialog
            {
                filePath = openFile.FileName; // Get the full path/name of file

                StreamReader fileContents = new StreamReader(filePath);
                txtBxMainTextView.Text = fileContents.ReadToEnd();

                fileContents.Close();
                // 12 characters before filename in title

            }
            //MessageBox.Show($"File: {filePath}");
        }

        private void cmdSave_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void cmdSave_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        private void cmdSaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void cmdSaveAs_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        private void mnuFile_Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void mnuAbout_Click(object sender, RoutedEventArgs e)
        {
            Window aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }
    }
}
