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
    /// Interaction logic for orderImageOnCd.xaml
    /// </summary>
    public partial class orderImageOnCd : UserControl
    {
        PhotoSelectionPage father = null;
        public orderImageOnCd(PhotoSelectionPage f)
        {
            InitializeComponent();
            father = f;
            this.father.StartModal();

            Utility.gradientManager gm = new gradientManager();
            gm.setgradient(gridBackground);            

              LoginManager m = LoginControler.getLoginManager();
              if (m != null)
              {
                  if (m.getNbDifferentPicture() < 10)
                  {
                      panelCheckPhotoOnCD.IsEnabled = false;
                      labelWarningMinimun.Visibility = System.Windows.Visibility.Visible;
                  }
                  else
                  {
                      panelCheckPhotoOnCD.IsEnabled = true;
                      labelWarningMinimun.Visibility = System.Windows.Visibility.Hidden;
                  }

                  if (m.getNbDifferentPicture() < 50)
                  {
                      surfaceCheckBoxWithoutPaper.IsEnabled = false;
                  }
                  else
                  {
                      surfaceCheckBoxWithoutPaper.IsEnabled = true;
                  }
              }
        }

        private void surfaceButtonOk_Click(object sender, RoutedEventArgs e)
        {              
            //father.updateImageOnCd(surfaceCheckBoxWithoutPaper.IsChecked,surfaceCheckBoxWithoutPaper.IsChecked);
            TransformationUtil.HideDialog((Grid)this.Parent);
            this.father.updateImageOnCd(surfaceCheckBoxAllOnCD.IsChecked, surfaceCheckBoxWithoutPaper.IsChecked, surfaceCheckBoxNoPictureOnCD.IsChecked);
            this.father.StopModal();

            // Sauvegarde 
            //LoginManager lm = LoginControler.getLoginManager();
            //lm.Save();
        }

        private void surfaceCheckBoxAllOnCD_Checked(object sender, RoutedEventArgs e)
        {
            surfaceCheckBoxNoPictureOnCD.IsChecked = false;
        }

        private void surfaceCheckBoxWithoutPaper_Checked(object sender, RoutedEventArgs e)
        {
            surfaceCheckBoxNoPictureOnCD.IsChecked = false;            
        }

        private void surfaceButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            TransformationUtil.HideDialog((Grid)this.Parent);          
            this.father.StopModal();
        }

        private void surfaceCheckBoxNoPictureOnCD_Checked(object sender, RoutedEventArgs e)
        {
            surfaceCheckBoxAllOnCD.IsChecked = false;
            surfaceCheckBoxWithoutPaper.IsChecked = false;
        }

        private void surfaceCheckBoxWithoutPaper_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}
