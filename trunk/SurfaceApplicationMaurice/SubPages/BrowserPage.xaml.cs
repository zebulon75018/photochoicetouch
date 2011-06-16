using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SurfaceApplicationMaurice.SubPages
{
    /// <summary>
    /// Interaction logic for BrowserPage.xaml
    /// </summary>
    public partial class BrowserPage : UserControl
    {        
        public BrowserPage()
        {            
            InitializeComponent();
        }

        public void Navigate(string url)
        {
            if (url == "") return;
            try
            {
                webBrowser1.Navigate(url);
            }
            catch (Exception e)
            {
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
