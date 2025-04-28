/*
* FILE : CloneyPadWindow.xaml.cs
* PROJECT : PROG2121 - Assignment #2
* PROGRAMMER : Chris Wickens
* FIRST VERSION : 2024-09-23
* FINAL VERSION : 2024-09-26
* DESCRIPTION : This file contains the methods necessary to create a basic functional clone of Notepad.
*/
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
    // Name: CloneyPadWindow
    //
    // Purpose:
    // This class contains the methods required to create a basic notepad clone using WPF
    // and some MVVM concepts.
    // 
    // This class inherits from the Window class, to create a proper WPF application.
    //
    // Fields:
    // private bool hasFileBeenSaved : Track if the current text has been saved
    // private bool hasTextBeenEdited : Track if txtBxMainTextView contents have changed
    // private string fileNameOnly : The current filename (if the file has been saved) (Ex: note.txt)
    // private string fullPathFileName : The current FULL path for the file being worked on
    // string fileDialogTypes : Used to edit the types of files the SaveAs dialog supports
    // 
    // Included methods:
    // UpdateTitle() : Used to update the Title bar of the CloneyPadWindow.xaml
    // txtBxMainTextView_TextChanged() : Tracks changes to the textbox in CloneyPadWindow.xaml
    // AskAboutUnsavedChanges() : Used to get the result of user input on dialog boxes
    // cmdNew_Executed() : Code for command binding ApplicationCommands.New
    // cmdOpen_Executed() : Code for command binding ApplicationCommands.Open
    // cmdSave_Executed() : Code for command binding ApplicationCommands.Save
    // cmdSaveAs_Executed() : Code for command binding ApplicationCommands.SaveAs
    // cmdSaveAs_CanExecute : Code to control if ApplicationCommands.SaveAs can be executed
    // mnuFile_Exit_Click : Used to control the Exit option in the File menu (calls cloneyPadMainWindow_Closing())
    // cloneyPadMainWindow_Closing() : Adds functionality when closing the program
    // mnuAbout_Click() : Used to display AboutWindow.xaml
    public partial class CloneyPadWindow : Window
    {
        // Fields
        private bool hasFileBeenSaved = false;
        private bool hasTextBeenEdited = false;
        private string fileNameOnly = "";
        private string fullPathFileName = "";

        // Supported file types (default .txt, and *.*)
        string fileDialogTypes = "Text file (*.txt)|*.txt|All files *|*.*";

        public CloneyPadWindow()
        {
            InitializeComponent();
        }

        // METHOD: UpdateTitle()
        //
        // DESCRIPTION: This method updates the Title of CloneyPadWindow.xaml, it will append the file name to the title.
        //
        // PARAMETERS: None
        // RETURNS: None
        internal void UpdateTitle()
        {
            Title = "CloneyPad - " + fileNameOnly;
        }

        // METHOD: txtBxMainTextView_TextChanged()
        //
        // DESCRIPTION: This method is called when text is changed in the TextBox txtBxMainTextView in CloneyPadWindow.xaml.
        // It will update the Label lblCharCount in CloneyPadWindow.xaml with the current character count
        // in txtBxMainTextView in CloneyPadWindow.xaml.
        //
        // PARAMETERS:
        // object sender (default)
        // TextChangedEventArgs e (default)
        // RETURNS: None
        internal void txtBxMainTextView_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblCharCount.Content = txtBxMainTextView.Text.Length.ToString();
            hasTextBeenEdited = true;
        }

        // METHOD: AskAboutUnsavedChanges()
        //
        // DESCRIPTION: This method is used to display a message box containing Yes/No/Cancel options and return their result to
        // the method that called this.
        // This should be called any time the current text has been changed, and NOT saved when the user
        // is trying to perform an action that will clear the unsaved data.
        // This method will call the cmdSave_Executed() method if the user wishes to save their work.
        //
        // PARAMETERS: None
        // RETURNS: MessageBoxResult - Returns which selection the user made in the MessageBox dialog (Yes/No/Cancel)
        internal MessageBoxResult AskAboutUnsavedChanges()
        {
            if (!hasFileBeenSaved && hasTextBeenEdited)
            {
                // Ask user if they want to save the file, because it has been edited
                MessageBoxResult askForSave = MessageBox.Show("Warning: This file has not been saved, would you like to save first?",
                "WARNING: File has not been saved yet!", MessageBoxButton.YesNoCancel, icon: MessageBoxImage.Exclamation);
                if (askForSave == MessageBoxResult.No)
                {
                    return askForSave;
                }

                if (askForSave == MessageBoxResult.Yes)
                {
                    cmdSave_Executed(null, null); // Allow the user to save
                    if (hasFileBeenSaved) // Check if they actually saved the file
                    {
                        return MessageBoxResult.Yes;
                    }
                    return MessageBoxResult.Cancel; // If the user clicked CANCEL in the save dialog
                }
            }
            return MessageBoxResult.Cancel;
        }

        // METHOD: cmdNew_Executed()
        //
        // DESCRIPTION: Uses command binding for ApplicationCommands.New in the menu item mnuFile_New.
        // If the user wants to make a new file, this checks if the current text needs to be saved first due to changes.
        // This will reset all fields to their defaults if they choose to make a new file, and reset the text in the main TextBox.
        //
        // PARAMETERS:
        // object sender (default)
        // ExecutedRoutedEventArgs e (default)
        // RETURNS: None
        internal void cmdNew_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (!hasFileBeenSaved && hasTextBeenEdited && txtBxMainTextView.Text.Length != 0)
            {
                MessageBoxResult createNew = AskAboutUnsavedChanges();
                if (createNew == MessageBoxResult.No || createNew == MessageBoxResult.Yes)
                {
                    txtBxMainTextView.Text = "";
                    hasFileBeenSaved = false;
                    hasTextBeenEdited = false;
                    fileNameOnly = "Untitled";
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
                fileNameOnly = "Untitled";
                fullPathFileName = "";
            }
            UpdateTitle();
        }

        // METHOD: cmdOpen_Executed()
        //
        // DESCRIPTION: Uses command binding for ApplicationCommands.Open in the menu item mnuFile_Open.
        // If the user wants to open an existing file, this checks if the current text needs to be saved first due to changes.
        //
        // PARAMETERS:
        // object sender (default)
        // ExecutedRoutedEventArgs e (default)
        // RETURNS: None
        internal void cmdOpen_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBoxResult saveBeforeOpen = MessageBoxResult.Yes;
            if (!hasFileBeenSaved && hasTextBeenEdited && txtBxMainTextView.Text.Length != 0)
            {
                // If the user is asked about changes
                saveBeforeOpen = AskAboutUnsavedChanges();
            }

            if (saveBeforeOpen == MessageBoxResult.No)
            {
                txtBxMainTextView.Text = "";
                hasFileBeenSaved = false;
                hasTextBeenEdited = false;
                fileNameOnly = "";
                fullPathFileName = "";
            }

            if (saveBeforeOpen == MessageBoxResult.Yes || saveBeforeOpen == MessageBoxResult.No)
            {
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

        // METHOD: cmdSave_Executed()
        //
        // DESCRIPTION: Uses command binding for ApplicationCommands.Save in the menu item mnuFile_Save.
        // This allows the main TextBox text to be saved to a file, and if the text has already been saved, to save
        // to the existing file name again. If the file has NOT been saved yet, it will execute cmdSaveAs_Executed() to open
        // the Save As dialog.
        //
        // PARAMETERS:
        // object sender (default)
        // ExecutedRoutedEventArgs e (default)
        // RETURNS: None
        internal void cmdSave_Executed(object sender, ExecutedRoutedEventArgs e)
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

        // METHOD: cmdSaveAs_Executed()
        //
        // DESCRIPTION: Uses command binding for ApplicationCommands.SaveAs in the menu item mnuFile_SaveAs.
        // Used to save text in the main TextBox to a specific file. This will be called in the first instance of the user
        // saving a file so they can provide a file name. The default directory provided in this method will open the dialog
        // in the My Documents folder of Windows.
        // File types are defined at the top of this file : string fileDialogTypes
        //
        // PARAMETERS:
        // object sender (default)
        // ExecutedRoutedEventArgs e (default)
        // RETURNS: None
        internal void cmdSaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
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
                hasFileBeenSaved = false;
            }
        }

        // METHOD: mnuFile_Exit_Click()
        //
        // DESCRIPTION: Provides functionality to the Exit option in the File menu.
        // Calling Close() will execute the method cloneyPadMainWindow_Closing() to handle exiting the application
        // after checking if the text needs to be saved.
        //
        // PARAMETERS:
        // object sender (default)
        // RoutedEventArgs e (default)
        // RETURNS: None
        internal void mnuFile_Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // METHOD: cloneyPadMainWindow_Closing()
        //
        // DESCRIPTION: Method to handle logic when the user is trying to close the program.
        // Used to check if the current text has been saved, or should be before allowing the program to be closed.
        //
        // PARAMETERS:
        // object sender (default)
        // System.ComponentModel.CancelEventArgs e (default)
        // RETURNS: None
        internal void cloneyPadMainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult exitResult = MessageBoxResult.No;
            if (!hasFileBeenSaved && hasTextBeenEdited && txtBxMainTextView.Text.Length != 0)
            {
                // Ask if the user wishes to save any unsaved data
                exitResult = AskAboutUnsavedChanges();
            }

            // The user canceled and wishes to exit without saving
            if (exitResult == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        // METHOD: mnuAbout_Click()
        //
        // DESCRIPTION: Opens AboutWindow.xaml when the About menu option is selected.
        //
        // PARAMETERS:
        // object sender (default)
        // RoutedEventArgs e (default)
        // RETURNS: None
        internal void mnuAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }
    }
}