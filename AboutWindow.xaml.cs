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
using System.Windows.Shapes;

namespace CloneyPad
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();

            txtBxAboutText.Text = $"This program was coded by:\n" +
                $"Name: Chris Wickens\n" +
                $"Student Number: 8827595\n" +
                $"Email: cwickens7595@conestogac.on.ca\n" +
                $"\n" +
                $"This is a simple Notepad clone called CloneyPad\n" +
                $"This was coded for Assignment #02\n" +
                $"Course: Windows Programming\n" +
                $"Course Code: PROG2121-24F-Sec1\n" +
                $"Teacher: Norbert Mika\n\n" ;
        }

        private void btnAbout_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
