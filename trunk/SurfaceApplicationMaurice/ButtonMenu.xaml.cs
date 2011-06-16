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
using Artefact.Animation;

namespace SurfaceApplicationMaurice
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ButtonMenu : UserControl
    {
        private string imageSource="";
        private string label = "";
        const int GrowingStep = 50;
        public ButtonMenu()
        {
            InitializeComponent();            
        }

        public string ImageSource {
            get { return imageSource; }
            set { imageSource = value;

            myImage.Source = new BitmapImage(new Uri(imageSource));
            }            
    }

        public string ImageGifSource
        {
            set
            {
                myImage.setGifSource(value);
            }
        }
        public void onlyBitmapHeight(int h)
        {
            borderText.Height = 0;
            borderImage.Height = h;
            reflect.Height = h;
            myImage.Height = h;            
        }
        public string Label
        {
            get { return label; }
            set { label = value; UIlabel.Text = value; }
        }

        private void GrowBorderText()
        {            
            var eog = new EaseObjectGroup();
            var eo = ArtefactAnimator.AddEase(rectangle1, new[] { AnimationTypes.Alpha}, new object[] { 1}, 0.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);                                  
        }        

        private void UnGrowBorderText()
        {
            var eog = new EaseObjectGroup();
            var eo = ArtefactAnimator.AddEase(rectangle1, new[] { AnimationTypes.Alpha }, new object[] { 0 }, 0.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);          
        }

        private void UserControl_TouchLeave(object sender, TouchEventArgs e)
        {
            UnGrowBorderText();
        }

        private void UserControl_TouchUp(object sender, TouchEventArgs e)
        {
            UnGrowBorderText();
        }

        private void UserControl_TouchDown(object sender, TouchEventArgs e)
        {
            GrowBorderText();
        }

        private void UserControl_TouchEnter(object sender, TouchEventArgs e)
        {
            GrowBorderText();
        }
    }
}
