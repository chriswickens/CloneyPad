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
        private bool hasTextBeenEdited = false;
        private string originalText = "";
        private string fileName = "untitled.txt";
        private string fullPathFileName = "";

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

        //private void cmdNew_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        //{
        //    // Do you need this? I don't think so, remove it later.
        //}

        private void cmdOpen_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //if (!hasFileBeenSaved)
            //{
            //    MessageBox.Show("This is where you ask the user if they want to save the file!");
            //}

            // Create and open the OpenFileDialog
            OpenFileDialog fileToOpen = new OpenFileDialog();
            if(fileToOpen.ShowDialog() == true) // If the user clicked OK in the open dialog
            {
                fullPathFileName = fileToOpen.FileName; // Get the full path/name of file

                StreamReader fileContents = new StreamReader(fullPathFileName); // Open a StreamReader
                txtBxMainTextView.Text = fileContents.ReadToEnd(); // Put file contents into main text view
                fileName = fileToOpen.SafeFileName; // Store the fileName (example.txt)
                fullPathFileName = fileToOpen.FileName; // Store the full filename and path
                fileContents.Close();

                // Add the filename to the Title
                Title = Title.Substring(0, 12) + fileName;

            }
            //MessageBox.Show($"File: {fullPathFileName}");
        }

        private void cmdSave_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void cmdSave_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //if (!hasFileBeenSaved)
            //{
            //    MessageBox.Show("Show the user the SAVE AS dialog in this case!");
            //}
        }

        private void cmdSaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFileAs = new SaveFileDialog();
            if(saveFileAs.ShowDialog() == true)
            {
                File.WriteAllText(saveFileAs.FileName, txtBxMainTextView.Text);
            }
        }

        private void cmdSaveAs_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
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
