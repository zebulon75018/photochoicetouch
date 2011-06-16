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
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Artefact.Animation;
using System.Diagnostics;
using System.IO;
using SurfaceApplicationMaurice.Utility;

namespace SurfaceApplicationMaurice.SubPages
{
    /// <summary>
    /// Interaction logic for InformationCategory.xaml
    /// </summary>
    public partial class InformationCategory : UserControl
    {

        BrowserPage browser=null;
        public InformationCategory()
        {
            InitializeComponent();

            UpdateList();
            
            Utility.gradientManager gm = new gradientManager();
            gm.setgradient(stackPanel1);            
        }

        public void UpdateList()
        {
            SurfaceListBoxItem il;
            ItemListCategory ilc ;            
            CategoriesXml xmlcat = globalDatasingleton.getInstance().getInfoCategories();

            List<String> name = xmlcat.GetNameCategories();
            List<String> bitmapname = xmlcat.GetBitmapsCategories();

            surfaceListBox1.Items.Clear();
            int i = 0;

            foreach (String n in name)
            {
                il = new SurfaceListBoxItem();
                ilc = new ItemListCategory();
                if (File.Exists(bitmapname[i]))
                {
                    ilc.ImageSource = bitmapname[i];
                }
                ilc.Label = n;
                il.Content = ilc;
                surfaceListBox1.Items.Add(il);
                i++;
            }            
          
        }

        private void surfaceListBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (surfaceListBox1.SelectedIndex == -1) return;
            
            if (browser!=null) browser = null;

            CategoriesXml xmlcat = globalDatasingleton.getInstance().getInfoCategories();
            System.Xml.XmlElement elm = xmlcat.getCurrentElem(surfaceListBox1.SelectedIndex);
            string url  = elm.GetAttribute("directory");

            browser = new BrowserPage(); 
            stackPanel1.Children.Add(browser);
            browser.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            browser.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            browser.Navigate(url);
            surfaceButtonCloseWeb.Visibility = System.Windows.Visibility.Visible;

            surfaceListBox1.SelectedIndex = -1;
        }       

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            //var eog = new EaseObjectGroup();
            //var eo = ArtefactAnimator.AddEase(stackPanel1, new[] { AnimationTypes.Width, AnimationTypes.Height }, new object[] { 0,0 }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            //var eo2 = ArtefactAnimator.AddEase(buttonBack, new[] { AnimationTypes.Alpha }, new object[] { 0 }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            //eog.AddEaseObject(eo);           
        }

        private void surfaceButtonCloseWeb_Click(object sender, RoutedEventArgs e)
        {
            if (browser != null && stackPanel1.Children.Contains(browser))
            {
                stackPanel1.Children.Remove(browser);
                surfaceButtonCloseWeb.Visibility = System.Windows.Visibility.Hidden;
                UpdateList();
            }
        }
    }
}
