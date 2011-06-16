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
using SurfaceApplicationMaurice.Utility;
using WPFLocalization;

namespace SurfaceApplicationMaurice.SubPages
{
    /// <summary>
    /// Interaction logic for onePhotoPage.xaml
    /// </summary>
    public partial class onePhotoPage : UserControl
    {
        public onePhotoPage(string filename)
        {
            InitializeComponent();            
            //globalDatasingleton.getInstance().InitCursor(

            SurfaceWindow1.changeMessagePage(" ");

            MyImage img = new MyImage();
            if (filename != "")
            {
                img.Source = BitmapUtil.GetImageSource(filename);
                img.OriginalPath = filename;
            }
            LoadImg(img);
            Utility.gradientManager gm = new gradientManager();
            gm.setgradient(gridPhoto);                       

            UpdateGUI();            
        }
       
        public void UpdateGUI()
        {
          /*  if (LoginControler.IsLogged())
            {
                borderSelection.Visibility = System.Windows.Visibility.Visible;                                
            }
            else
            {
                borderSelection.Visibility = System.Windows.Visibility.Hidden;
            }
           * */
        }

        private void LoadImg(MyImage img)
        {           
            scatterView.Width = System.Windows.SystemParameters.PrimaryScreenWidth * 0.6;
            scatterView.Height = System.Windows.SystemParameters.PrimaryScreenWidth * 0.6;

            //scatterView.CanRotate = CommunConfig.getInstance().ImageRotate;            
            
            if (img.Source == null) return;            
            
            if (img.Source.Width > img.Source.Height)
            {
                scatterView.Height *= img.Source.Height/img.Source.Width;               
            }
            else
            {
                scatterView.Width*= img.Source.Width/ img.Source.Height;                   
            }
            imgScatter.OriginalPath = img.OriginalPath;
            imgScatter.Source = img.Source;           
            
            //Point originPoint = this.PointToScreen(new Point(0, 0));
            scatterView.Center = new Point(System.Windows.SystemParameters.PrimaryScreenWidth / 2, System.Windows.SystemParameters.PrimaryScreenHeight / 2 -50);            
            // System.Windows.SystemParameters.PrimaryScreenWidth/2;
            // System.Windows.SystemParameters.PrimaryScreenHeight/2;            

            scatterView.MaxWidth = System.Windows.SystemParameters.PrimaryScreenWidth * 2;
            scatterView.MaxHeight = System.Windows.SystemParameters.PrimaryScreenHeight * 2;
            imgScatter.MaxWidth= System.Windows.SystemParameters.PrimaryScreenWidth*2;
            imgScatter.MaxHeight = System.Windows.SystemParameters.PrimaryScreenHeight*2;

            scatterView.Opacity = 0;
            var eog = new EaseObjectGroup();
            var eo = ArtefactAnimator.AddEase(scatterView, new[] { AnimationTypes.Alpha}, new object[] { 1 }, 1, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);          

        }

        private void LoadVBox(ref Viewbox vb)
        {
            MyImage img = vb.Child as MyImage;
            if (img == null) return;
            LoadImg(img);
        }

        private void goPrevious()
        {
            string previousFilename = globalDatasingleton.getInstance().getPrevious();
            if (previousFilename == "") return;
            if (previousFilename == null) return;
            MyImage img = new MyImage();

            img.OriginalPath = previousFilename;
            img.Source = new BitmapImage(new Uri(previousFilename));
            Viewbox vb = new Viewbox { Width = 200, Child = img };

            LoadVBox(ref vb);
        }

        private void goNext()        
        {
            string next = globalDatasingleton.getInstance().getNext();
            if (next == null) return;
            if (next == "") return;

            MyImage img = new MyImage();
            img.OriginalPath = next;
            img.Source = new BitmapImage(new Uri(next));
            Viewbox vb = new Viewbox { Width = 200, Child = img };

            LoadVBox(ref vb);
            //LoadVBox(ref next);
         }

        private void surfaceButton1_Click(object sender, RoutedEventArgs e)
        {
            goPrevious();
            /*
            string previousFilename = globalDatasingleton.getInstance().getPrevious();
            if (previousFilename == "") return;
            if (previousFilename == null) return;
            MyImage img = new MyImage();

            img.OriginalPath = previousFilename;
            img.Source = new BitmapImage(new Uri(previousFilename));
            Viewbox vb = new Viewbox { Width = 200, Child = img };
           
            LoadVBox(ref vb);
             * */
        }

        private void surfaceButtonNext_Click(object sender, RoutedEventArgs e)
        {
            goNext();
        }

        private void ShowMessage(string msg)
        {
            textBlockMessage.Text = msg;            
            textBlockMessage.Opacity = 0;
            textBlockMessage.Visibility = Visibility.Visible;
            var eog = new EaseObjectGroup();
            //eog.Complete += g => Debug.WriteLine("COMPLETE");
            var eo = ArtefactAnimator.AddEase( textBlockMessage, new[] { AnimationTypes.Alpha }, new object[] { 1 }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);             
             eog.Complete += delegate(EaseObjectGroup easeObjectGroup) {
                 var eog2 = new EaseObjectGroup();
                 //eog.Complete += g => Debug.WriteLine("COMPLETE");
                 var eo2 = ArtefactAnimator.AddEase(textBlockMessage, new[] { AnimationTypes.Alpha }, new object[] { 0 }, 1.5, AnimationTransitions.CubicEaseOut, 1);
                 eog2.AddEaseObject(eo2);
                 eog2.Complete += delegate(EaseObjectGroup easeObjectGroup2) { textBlockMessage.Visibility = System.Windows.Visibility.Hidden; };
                };
        }  
   
        private void surfaceButtonAddToSelection_Click(object sender, RoutedEventArgs e)
        {
            if (LoginControler.IsLogged())
            {
                SelectionUser.getInstance().AddImage(imgScatter);
                ShowMessage(LocalizationManager.ResourceManager.GetString("Imageaddedtoyourselection"));
            }
            else
            {
                LoginControler.WantToAddImageAfterLogin = true;
                LoginControler.addImage = imgScatter;
                SurfaceWindow1.StaticShowLoginPage();                               
            }            
        }

        private void viewbox4_TouchEnter(object sender, TouchEventArgs e)
        {
            Viewbox vb = (Viewbox)sender;
            var eog = new EaseObjectGroup();
            var eo = ArtefactAnimator.AddEase(vb, new[] { AnimationTypes.Width, AnimationTypes.Height }, new object[] { 150, 150 }, 0.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);
        }

        private void viewbox4_TouchLeave(object sender, TouchEventArgs e)
        {
            Viewbox vb = (Viewbox)sender;

            var eog = new EaseObjectGroup();
            var eo = ArtefactAnimator.AddEase(vb, new[] { AnimationTypes.Width, AnimationTypes.Height }, new object[] { 100, 100 }, 0.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);
        }

        public void setModal(bool b)
        {
            dockPanel1.IsEnabled = !b;
        }
    }
}
