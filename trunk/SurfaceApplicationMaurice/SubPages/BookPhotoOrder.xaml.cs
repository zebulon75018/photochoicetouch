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
using SurfaceApplicationMaurice.Utility;

namespace SurfaceApplicationMaurice.SubPages
{
    /// <summary>
    /// Interaction logic for BookPhotoOrder.xaml
    /// </summary>
    public partial class BookPhotoOrder : UserControl
    {
        protected PhotoSelectionPage father;

        public bool userClickOnOk = false;
        public BookPhotoOrder(PhotoSelectionPage f)
        {
            InitializeComponent();

            Utility.gradientManager gm = new gradientManager();
            gm.setgradient(gridBackground);
            gm.setgradient(ifCheckedGrid);            

            father = f;

            this.father.StartModal();

            for (int n = 20; n < 60; n = n + 2)
            {
                comboBoxNumberOfPages.Items.Add(n);
            }

            comboBoxNumberOfPages.SelectedIndex = 0;                


            LoginManager m = LoginControler.getLoginManager();
            if (m != null)
            {
                if (m.isBookOrder())
                {
                    checkBox1.IsChecked = true;
                    comboBoxNumberOfPages.SelectedValue = m.getNbPageOkBook().ToString();
                    comboBoxNumberOfPages.Text = m.getNbPageOkBook().ToString();                    
                }
                else
                {
                    checkBox1.IsChecked = false;
                }
            }
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            ifCheckedGrid.Visibility = System.Windows.Visibility.Visible;
        }

        private void checkBox1_TouchUp(object sender, TouchEventArgs e)
        {
            
        }

        private void checkBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            ifCheckedGrid.Visibility = System.Windows.Visibility.Hidden;
        }

        private void surfaceButtonOk_Click(object sender, RoutedEventArgs e)
        {
            this.father.updateBookPhoto(checkBox1.IsChecked, surfaceCheckBoxAllOnBook.IsChecked,comboBoxNumberOfPages.Text);
            userClickOnOk = true;
            TransformationUtil.HideDialog((Grid)this.Parent);
            this.father.StopModal();
        }

        private void surfaceButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            userClickOnOk = false;
            TransformationUtil.HideDialog((Grid)this.Parent);
            this.father.StopModal();
        }
    }
}
