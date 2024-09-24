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
        private string fileNameOnly = "";
        private string fullPathFileName = "";

        public CloneyPadWindow()
        {
            InitializeComponent();
        }

        private void txtBxMainTextView_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblCharCount.Content = txtBxMainTextView.Text.Length.ToString();
            hasFileBeenSaved = false;
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
            if (!hasFileBeenSaved)
            {
                MessageBox.Show("This is where you ask the user if they want to save the file!");
            }

            // Create and open the OpenFileDialog
            OpenFileDialog fileToOpen = new OpenFileDialog();
            if (fileToOpen.ShowDialog() == true) // If the user clicked OK in the open dialog
            {
                fullPathFileName = fileToOpen.FileName; // Get the full path/name of file
                StreamReader fileContents = new StreamReader(fullPathFileName); // Open a StreamReader
                txtBxMainTextView.Text = fileContents.ReadToEnd(); // Put file contents into main text view
                fileNameOnly = fileToOpen.SafeFileName; // Store the fileNameOnly (example.txt)
                fullPathFileName = fileToOpen.FileName; // Store the full filename and path
                fileContents.Close();

                // Add the filename to the Title
                Title = Title.Substring(0, 12) + fileNameOnly;

            }
            //MessageBox.Show($"File: {fullPathFileName}");
        }

        private void cmdSave_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (!hasFileBeenSaved && fullPathFileName == "")
            {
                cmdSaveAs_Executed(sender, e);
                return;
            }

            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Text file (*.txt)|*.txt|All files *|*.*";
            saveFile.FileName = fullPathFileName;
            File.WriteAllText(saveFile.FileName, txtBxMainTextView.Text);
            hasFileBeenSaved = true;
        }

        private void cmdSave_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void cmdSaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFileAs = new SaveFileDialog();
            saveFileAs.Filter = "Text file (*.txt)|*.txt|All files *|*.*";
            saveFileAs.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (saveFileAs.ShowDialog() == true)
            {
                File.WriteAllText(saveFileAs.FileName, txtBxMainTextView.Text);
                fullPathFileName = saveFileAs.FileName;
                hasFileBeenSaved = true;
            }
        }

        private void cmdSaveAs_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true; // Should this ever not be selectable?
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
