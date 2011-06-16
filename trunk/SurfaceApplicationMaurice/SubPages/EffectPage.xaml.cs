using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Surface.Presentation.Controls;

using Artefact.Animation;
using SurfaceApplicationMaurice.Utility;

namespace SurfaceApplicationMaurice.SubPages
{
    /// <summary>
    /// Interaction logic for EffectPage.xaml
    /// </summary>
    public partial class EffectPage : UserControl
    {
        PhotoSelectionPage father = null;
        public bool deleteOriginal = false;
        public EffectPage(System.Windows.Controls.Image img,PhotoSelectionPage fsp)
        {

            father = fsp;
            father.StartModal();
            InitializeComponent();

          
            if (img != null)
            {
               
              this.Cursor = Cursors.Wait;
              imageBlackAndWhite.Source = EffectBitmap.convertBitmapToBitmapSource(EffectBitmap.ConvertBlackAndWhite(EffectBitmap.GetBitmap(img)));
              //this.Cursor = Cursors.Wait;
              imageSepia.Source = EffectBitmap.convertBitmapToBitmapSource(EffectBitmap.ConvertSepiaTone(EffectBitmap.GetBitmap(img)));


              if (img.Source.Width > img.Source.Height)
              {
                  imageBlackAndWhite.Height *= img.Source.Height / img.Source.Width;
                  imageSepia.Height *= img.Source.Height / img.Source.Width;
              }
              else
              {
                  imageBlackAndWhite.Width *= img.Source.Width / img.Source.Height;
                  imageSepia.Width *= img.Source.Width / img.Source.Height;
              }

            this.Cursor = Cursors.Arrow;        
            }
            Utility.gradientManager gm = new gradientManager();
           gm.setgradient(gridEffect);           

        }

      
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            TransformationUtil.HideDialog((Grid)this.Parent);
            father.StopModal();
        }

      

        private void buttonBAWOK_Click(object sender, RoutedEventArgs e)
        {
            TransformationUtil.AppearControl(stackPanelBWDeleteSource);
        }

        private void buttonBAWOKDeleteOriginal_Click(object sender, RoutedEventArgs e)
        {
            deleteOriginal = true;
            blackandWhiteOk();
        }

        private void blackandWhiteOk()
        {
            TransformationUtil.HideDialog((Grid)this.Parent);
            father.StopModal();

            imageBlackAndWhite.Source = null;
            father.ConvertBlackAndWhite(deleteOriginal);
            //SelectionUser.getInstance().ReplaceImage(imageBlackAndWhite.Source);           
        }

        private void buttonSepiaOK_Click(object sender, RoutedEventArgs e)
        {
            TransformationUtil.AppearControl(stackPanelSepiaDeleteSource);
        }

        private void buttonSepiaDeleteOriginalOK_Click(object sender, RoutedEventArgs e)
        {
            deleteOriginal = true;
            SepiaOk();
        }

        private void SepiaOk()
        {
            TransformationUtil.HideDialog((Grid)this.Parent);
            father.StopModal();
             imageSepia.Source = null;
             father.ConvertSepia(deleteOriginal);
        }

        private void imageBlackAndWhite_TouchUp(object sender, TouchEventArgs e)
        {
            TransformationUtil.AppearControl(buttonBAWOK);            
        }

        private void imageSepia_TouchUp(object sender, TouchEventArgs e)
        {
            TransformationUtil.AppearControl(buttonSepiaOK);
        }

        private void imageBlackAndWhite_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void buttonSepiaDontDeleteOriginal_Click(object sender, RoutedEventArgs e)
        {
            deleteOriginal = false;
            SepiaOk();
        }

        private void buttonBWDontDeleteORiginal_Click(object sender, RoutedEventArgs e)
        {
            deleteOriginal = false;
            blackandWhiteOk();
        }
    }
}
