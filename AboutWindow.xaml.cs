/*
* FILE : AboutWindow.xaml.cs
* PROJECT : PROG2121 - Assignment #2
* PROGRAMMER : Chris Wickens
* FIRST VERSION : 2024-09-23
* FINAL VERSION : 2024-09-26
* DESCRIPTION : This file contains code to handle the simple About window for CloneyPad.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CloneyPad
{
    // Name: AboutWindow
    //
    // Purpose:
    // This class is used to populate the About window with text, and handle the close button logic.
    // 
    // This class inherits from the Window class, to create a proper WPF window.
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();

            txtBxAboutText.Text = $"This program was coded by:\n" +
                $"Name: Chris Wickens\n" +
                $"Student Number: XXXXXXX\n" +
                $"Email: chriswickens@gmail.com\n" +
                $"\n" +
                $"This is a simple Notepad clone called CloneyPad\n" +
                $"This was coded for Assignment #02\n" +
                $"Course: Windows Programming\n" +
                $"Course Code: PROG2121-24F-Sec1\n";
        }

        // METHOD: btnAbout_Close_Click()
        //
        // DESCRIPTION: This method handles the code for the Close button in the AboutWindow.xaml file.
        //
        // PARAMETERS:
        // object sender (default)
        // RoutedEventArgs e (default)
        //
        // RETURNS: None
        private void btnAbout_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
