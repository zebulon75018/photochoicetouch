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
    /// Interaction logic for ItemListCategory.xaml
    /// </summary>
    public partial class ItemListCategory : UserControl
    {      
        private string imageSource="";
        private string label = "";
        public int GrowingStep = 20;

        public ItemListCategory()
        {
            InitializeComponent();
        }
        public string ImageSource {
            get { return imageSource; }
            set {

                imageSource = value;
                if (imageSource.ToLower().Contains(".gif"))
                {
                    myImage.setGifSource(value);
                }
                else
                {
                    myImage.Source = new BitmapImage(new Uri(imageSource));
                }
            }            
        }
        public string Label
        {
            get { return label; }
            set { label = value; UIlabel.Text = value; }
        }

        private void TouchAnimation()
        {
            var eog = new EaseObjectGroup();
            var eo = ArtefactAnimator.AddEase(myImage, new[] { AnimationTypes.Height, AnimationTypes.Width }, new object[] { myImage.Height + GrowingStep, myImage.Width + GrowingStep }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);
        }

        private void NoTouchAnimation()
        {
            var eog = new EaseObjectGroup();
            var eo = ArtefactAnimator.AddEase(myImage, new[] { AnimationTypes.Height, AnimationTypes.Width }, new object[] { myImage.Height - GrowingStep, myImage.Width - GrowingStep }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);
        }

        private void stackPanel1_TouchDown(object sender, TouchEventArgs e)
        {
            TouchAnimation();
        }

        private void stackPanel1_TouchEnter(object sender, TouchEventArgs e)
        {
            TouchAnimation();
        }

        private void stackPanel1_TouchLeave(object sender, TouchEventArgs e)
        {
            NoTouchAnimation();
        }

        private void stackPanel1_TouchMove(object sender, TouchEventArgs e)
        {

        }

        private void stackPanel1_TouchUp(object sender, TouchEventArgs e)
        {
            NoTouchAnimation();
        }
    }
}