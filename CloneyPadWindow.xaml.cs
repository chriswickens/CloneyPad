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

namespace CloneyPad
{
    /// <summary>
    /// Interaction logic for CloneyPadWindow.xaml
    /// </summary>
    public partial class CloneyPadWindow : Window
    {
        public CloneyPadWindow()
        {
            InitializeComponent();
        }

        private void txtBxMainTextView_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblCharCount.Content = txtBxMainTextView.Text.Length.ToString();
        }
    }
}
