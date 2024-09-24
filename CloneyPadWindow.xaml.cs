using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
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
        private bool anySaveSuccess = false;
        private string fileNameOnly = "";
        private string fullPathFileName = "";

        // Supported file types (default .txt, and *.*)
        string fileDialogTypes = "Text file (*.txt)|*.txt|All files *|*.*";

        public CloneyPadWindow()
        {
            InitializeComponent();
        }

        // Used to update the title
        private void UpdateTitle()
        {
            Title = "CloneyPad - " + fileNameOnly;
        }

        private void txtBxMainTextView_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblCharCount.Content = txtBxMainTextView.Text.Length.ToString();
            hasTextBeenEdited = true;
        }
        private MessageBoxResult AskAboutUnsavedChanges(/*object sender*//*, ExecutedRoutedEventArgs e*/)
        {
            if (!hasFileBeenSaved && hasTextBeenEdited)
            {
                // Ask user if they want to save the file, because it has been edited:
                MessageBoxResult askForSave = MessageBox.Show("Warning: This file has not been saved, would you like to save first?",
                "WARNING: File has not been saved yet!", MessageBoxButton.YesNoCancel, icon: MessageBoxImage.Exclamation);
                if (askForSave == MessageBoxResult.No)
                {
                    return askForSave;
                }

                if (askForSave == MessageBoxResult.Yes)
                {
                    cmdSave_Executed(null, null);
                    if (anySaveSuccess)
                    {
                        return MessageBoxResult.Yes;
                    }
                    return MessageBoxResult.Cancel;
                }
            }
            return MessageBoxResult.Cancel;
        }

        private void cmdNew_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (!hasFileBeenSaved && hasTextBeenEdited)
            {
                MessageBoxResult createNew = AskAboutUnsavedChanges();
                if (createNew == MessageBoxResult.No || createNew == MessageBoxResult.Yes)
                {
                    txtBxMainTextView.Text = "";
                    hasFileBeenSaved = false;
                    hasTextBeenEdited = false;
                    fileNameOnly = "";
                    fullPathFileName = "";
                }

                if (createNew == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            else
            {
                txtBxMainTextView.Text = "";
                hasFileBeenSaved = false;
                hasTextBeenEdited = false;
                fileNameOnly = "";
                fullPathFileName = "";
            }
        }


        private void cmdOpen_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Default to CANCEL
            MessageBoxResult saveBeforeOpen = MessageBoxResult.No;
            if (!hasFileBeenSaved && hasTextBeenEdited)
            {
                // If the user is asked about changes
                saveBeforeOpen = AskAboutUnsavedChanges();
            }

            // Show the OpenFileDialog if the user clicked anything other than Cancel
            if (saveBeforeOpen == MessageBoxResult.No || saveBeforeOpen == MessageBoxResult.Yes)
            {
                // Create and open the OpenFileDialog
                OpenFileDialog fileToOpen = new OpenFileDialog();
                if (fileToOpen.ShowDialog() == true) // If the user clicked OK in the open dialog
                {
                    fullPathFileName = fileToOpen.FileName; // Get the full path/name of file

                    try
                    {
                        using (StreamReader fileContents = new StreamReader(fullPathFileName))
                        {
                            txtBxMainTextView.Text = fileContents.ReadToEnd(); // Put file contents into main text view
                            fileNameOnly = fileToOpen.SafeFileName; // Store the fileNameOnly (example.txt)
                            fullPathFileName = fileToOpen.FileName; // Store the full filename and path
                            hasTextBeenEdited = false;
                        }

                        UpdateTitle();
                    }
                    catch (FileNotFoundException eX)
                    {
                        MessageBox.Show($"The file was not found: '{eX}'", "ERROR", MessageBoxButton.OK, icon: MessageBoxImage.Warning);
                    }
                    catch (DirectoryNotFoundException eX)
                    {
                        MessageBox.Show($"The directory was not found: '{eX}'", "ERROR", MessageBoxButton.OK, icon: MessageBoxImage.Warning);
                    }
                    catch (IOException eX)
                    {
                        MessageBox.Show($"The file could not be opened: '{eX}'", "ERROR", MessageBoxButton.OK, icon: MessageBoxImage.Warning);
                    }
                    catch (Exception eX)
                    {
                        MessageBox.Show($"Uncaught exception: '{eX}'", "ERROR", MessageBoxButton.OK, icon: MessageBoxImage.Warning);
                    }
                }
            }

            else
            {
                return;
            }
        }

        private void cmdSave_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (!hasFileBeenSaved && fullPathFileName == "")
            {
                cmdSaveAs_Executed(sender, e);
                return;
            }

            try
            {
                using (StreamWriter writeText = new StreamWriter(fullPathFileName))
                {
                    writeText.Write(txtBxMainTextView.Text);
                }

                hasFileBeenSaved = true;
                hasTextBeenEdited = false;
                anySaveSuccess = true;
                UpdateTitle();
            }
            catch (FileNotFoundException eX)
            {
                MessageBox.Show($"The file was not found: '{eX}'", "ERROR", MessageBoxButton.OK, icon: MessageBoxImage.Warning);
            }
            catch (DirectoryNotFoundException eX)
            {
                MessageBox.Show($"The directory was not found: '{eX}'", "ERROR", MessageBoxButton.OK, icon: MessageBoxImage.Warning);
            }
            catch (IOException eX)
            {
                MessageBox.Show($"The file could not be opened: '{eX}'", "ERROR", MessageBoxButton.OK, icon: MessageBoxImage.Warning);
            }
            catch (Exception eX)
            {
                MessageBox.Show($"Uncaught exception: '{eX}'", "ERROR", MessageBoxButton.OK, icon: MessageBoxImage.Warning);
            }

        }

        //private void cmdSave_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        //{
        //    e.CanExecute = true; // Is this needed?
        //}

        private void cmdSaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFileAs = new SaveFileDialog();
            saveFileAs.Filter = fileDialogTypes;
            saveFileAs.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // Default to MyDocuments

            if (saveFileAs.ShowDialog() == true)
            {
                try
                {
                    using (StreamWriter writeText = new StreamWriter(saveFileAs.FileName))
                    {
                        writeText.Write(txtBxMainTextView.Text);
                    }

                    fullPathFileName = saveFileAs.FileName;
                    fileNameOnly = saveFileAs.SafeFileName;
                    hasFileBeenSaved = true;
                    hasTextBeenEdited = false;
                    anySaveSuccess = true;
                    UpdateTitle();
                }

                catch (FileNotFoundException eX)
                {
                    MessageBox.Show($"The file was not found: '{eX}'", "ERROR", MessageBoxButton.OK, icon: MessageBoxImage.Warning);
                }
                catch (DirectoryNotFoundException eX)
                {
                    MessageBox.Show($"The directory was not found: '{eX}'", "ERROR", MessageBoxButton.OK, icon: MessageBoxImage.Warning);
                }
                catch (IOException eX)
                {
                    MessageBox.Show($"The file could not be opened: '{eX}'", "ERROR", MessageBoxButton.OK, icon: MessageBoxImage.Warning);
                }
                catch (Exception eX)
                {
                    MessageBox.Show($"Uncaught exception: '{eX}'", "ERROR", MessageBoxButton.OK, icon: MessageBoxImage.Warning);
                }

            }
            else
            {
                anySaveSuccess = false;
            }
        }

        private void cmdSaveAs_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (anySaveSuccess)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
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

        private void cloneyPadMainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
