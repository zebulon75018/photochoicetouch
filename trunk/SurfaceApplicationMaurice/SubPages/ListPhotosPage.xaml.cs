using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
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
using WPFLocalization;
using SurfaceApplicationMaurice.Utility;
using Artefact.Animation;

namespace SurfaceApplicationMaurice.SubPages
{
    /// <summary>
    /// Interaction logic for ListPhotosPage.xaml
    /// </summary>
    public partial class ListPhotosPage : UserControl
    {
        Viewbox LastViewBoxTouch= null;

        const int nbRow = 100;
        private int nbColumn = 6;
        private int sizeViewBox = 300;
        const int stepLoading = 1000;
        int nbItem = 0;
        int nbLoadedImage;
        XmlElement currentCat = null;
      
        bool loadFinished = false; // Il a fini de charger, si toujours en chargement alors probleme si l'on agrandit l'image

        DispatcherTimer _timer = null;

        Stack<String> listFile = new Stack<string>();
        DispatcherTimer _timerDoubleTouch = null;

        public ListPhotosPage(XmlElement elm)
        {
            InitializeComponent();
            // Create a Timer with a Normal Priority           

            SurfaceWindow1.changeMessagePage("");
            Utility.gradientManager gm = new gradientManager();
            gm.setgradient(this.gridBackground);            

            Init();

            currentCat = elm;
            if (currentCat == null)
            {
                chargementLabel.Content = "Error Loading Category";
            }
            else
            {
                InsertPhoto();
            }
        }

        public ListPhotosPage(List<FileInfo> listFile)
        {
            InitializeComponent();
            Init();
            InsertPhotoDate(listFile);
        }

        public void Init()
        {
            nbColumn = GlobalConfig.getInstance().getNbCase;
            sizeViewBox = GlobalConfig.getInstance().sizeViewBox;
        }

      

        public void SetCategory()
        {

        }

        private void InsertPhotoDate(List<FileInfo> listFiles)
        {
            ShowWaiting();
            nbLoadedImage = 0;
            InsertButtonCategory();

            foreach (FileInfo fi in listFiles)
            {
                if (File.Exists(fi.DirectoryName + "\\miniatures\\" + fi.Name))
                {
                    listFile.Push(fi.DirectoryName + "\\miniatures\\" + fi.Name);
                }
                else
                {
                    listFile.Push(fi.FullName);
                }
            }
            
            LaunchTimer();
        }

        private void InsertButtonCategory()
        {
            if (currentCat == null) return;
            CategoriesXml xmlcat = globalDatasingleton.getInstance().getCategories();
            List<String> children = xmlcat.getListChildCurrent(ref currentCat);

            if (children.Count == 0) listChildCategory.Height = 0;

            foreach (String child in children)
            {
                Button b = new Button();
                b.Content = "  " + child + " >>    ";
                b.Padding = new Thickness(10);                                                                
                b.FontSize = 20;
                b.OpacityMask = new LinearGradientBrush(Colors.Black, Colors.Transparent, 0);
                 //Background="#00000000" BorderBrush="#00000000"
                b.FontWeight = System.Windows.FontWeights.ExtraBold;
                b.TouchUp += buttonChild_TouchUp;
                listChildCategory.Children.Add(b);
            }

        }

        private void ShowWaiting()
        {
            gridWating.Width = this.Width;
            gridWating.Height = this.Height;
            gridWating.Visibility = System.Windows.Visibility.Visible;
            WaitingAnimation.Visibility = System.Windows.Visibility.Visible;
            WaitingAnimation.IsAnimated = true;
        }
        private void LaunchTimer()
        {
            nbItem = listFile.Count;
            _timer = new DispatcherTimer();
            // Set the Interval to 2 seconds
            _timer.Interval = TimeSpan.FromMilliseconds(50);
            SendTimerForLoading();  
        }

        private void InsertPhoto()
        {
            ShowWaiting();
            nbLoadedImage = 0;
            InsertButtonCategory();
            CategoriesXml xmlcat = globalDatasingleton.getInstance().getCategories();
            string path = xmlcat.getCurrentElemDirectory(ref currentCat);

            if (Directory.Exists(path) == false)
            {
                MessageBox.Show("Error PATH NOT EXISTS " + path);
                return;
            }

            if (Directory.Exists(path + "\\miniatures\\"))
            {
                path = path + "\\miniatures\\";
            }            
            
            foreach (string file in Directory.GetFiles(path, "*.jpg"))           
            {
                listFile.Push(file);
            }

            LaunchTimer();
        }

        private void SendTimerForLoading()
        {
            // Set the callback to just show the time ticking away
            // NOTE: We are using a control so this has to run on 
            // the UI thread
            _timer.Tick += new EventHandler(delegate(object s, EventArgs a)
            {
                SurfaceListBoxItem il;
                StackPanel sp;

                if (listFile.Count == 0 || nbLoadedImage >= stepLoading)
                {
                    //surfaceSlider1.Minimum = 0;
                    //surfaceSlider1.Maximum = nbItem / nbColumn;
                    //surfaceSlider1.Value = nbItem / nbColumn;
                    loadFinished = true;
                    chargementLabel.Content = "Loading finish ";
                    _timer.Stop();
                    WaitingAnimation.IsAnimated = false;
                    WaitingAnimation.Visibility = System.Windows.Visibility.Hidden;
                    gridWating.Visibility = System.Windows.Visibility.Hidden;

                    SurfaceWindow1.changeMessagePage(LocalizationManager.ResourceManager.GetString("TouchTwoTimeToSelect"));
                    return;
                }

                il = new SurfaceListBoxItem();
                sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                for (int i = 0; i < nbColumn; i++)
                {
                    if (listFile.Count == 0 || nbLoadedImage >= stepLoading) break;
                    AddImageToStackPanel(ref sp);
                }
                il.Content = sp;
                surfaceListBox1.Items.Add(il);
            });

            // Start the timer
            _timer.Start();
        }

        
        private void AddImageToStackPanel(ref StackPanel sp)
        {          
            try
            {
                string filename = listFile.Pop();                
//                img.Source = new BitmapImage();

                MyImage img = new MyImage();
                img.OriginalPath = filename;
                //Image img = new Image();
                img.Source = BitmapUtil.GetImageSource(filename);
                

                Viewbox vb = null;
                if (img.Source.Width > img.Source.Height)
                {
                    vb = new Viewbox { Width = sizeViewBox, Child = img };
                }
                else
                {
                    vb = new Viewbox { Width = (int)((float)sizeViewBox * ((float)img.Source.Width/(float)img.Source.Height)), Child = img };
                }
                //img.Source = new BitmapImage(new Uri(filename));
                //DrawingContext dc = 
                //Viewbox vb = new Viewbox { Width = sizeViewBox, Child = img };
                globalDatasingleton.getInstance().AddViewBox(ref vb,filename);
                vb.TouchDown += ViewBox_TouchDown;
                vb.MouseDown += ViewBox_MouseDown;
                vb.Margin = new Thickness(3);
                sp.Children.Add(vb);
                chargementLabel.Content = "Loading " + nbLoadedImage.ToString() + " / " + nbItem.ToString();
                nbLoadedImage++;
            }
            catch (Exception e)
            {
             //   MessageBox.Show("Error Adding Image " + e.Message);
            }
        }


        private void StartTimerDoubleTouch()
        {
                _timerDoubleTouch = new DispatcherTimer();
                // Set the Interval to 2 seconds
                _timerDoubleTouch.Interval = TimeSpan.FromSeconds(20);

                _timerDoubleTouch.Tick += new EventHandler(delegate(object s, EventArgs a)
                {
                    _timerDoubleTouch.Stop();
                    _timerDoubleTouch = null;
                });

            _timerDoubleTouch.Start();

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

        private void openPagePhoto()
        {

            if (surfaceListBox1.ActualWidth != 500)
            {
                ShowArrow();
            }

            gridPhoto.Opacity = 1;
            gridListThumb.Width = 500;
            surfaceListBox1.Width = 500;
            //var eog = new EaseObjectGroup();
            //gridPhoto.Opacity = 0;
            //var eo = ArtefactAnimator.AddEase(gridPhoto, new[] { AnimationTypes.Width, AnimationTypes.Alpha }, new object[] { 700, 1 }, 1.5, AnimationTransitions.CubicEaseOut, 0);            
            //eog.AddEaseObject(eo);
            gridPhoto.Width = 1400;

            
            
            globalDatasingleton.getInstance().InitCursor(LastViewBoxTouch);
            
            if (globalDatasingleton.getInstance().getCursorFilename() != "")
            {
                String filename = globalDatasingleton.getInstance().getCursorFilename(); 
                Image img = LastViewBoxTouch.Child as Image;
                MyImage img2 = new MyImage();
                if (filename != "")
                {
                    img2.Source = BitmapUtil.GetImageSource(filename);
                    img2.OriginalPath = filename;
                }
                LoadImg(img2);
            }          
           //surfaceListBox1.SelectedIndex = -1;
            /*
           Image img = LastViewBoxTouch.Child as Image;
                    
             * */
        }

        private void LoadImg(MyImage img)
        {            

            //scatterView.CanRotate = CommunConfig.getInstance().ImageRotate;                        

            scatterView.Width = System.Windows.SystemParameters.PrimaryScreenWidth * 0.4;
            scatterView.Height = System.Windows.SystemParameters.PrimaryScreenWidth * 0.4;

            if (img.Source == null) return;

            if (img.Source.Width > img.Source.Height)
            {
                scatterView.Height *= img.Source.Height / img.Source.Width;
                imgScatter.Height *= scatterView.Height;
            }
            else
            {
                scatterView.Width *= img.Source.Width / img.Source.Height;
                imgScatter.Width *= scatterView.Width;
            }
            imgScatter.OriginalPath = img.OriginalPath;
            imgScatter.Source = img.Source;

            //Point originPoint = this.PointToScreen(new Point(0, 0));
            scatterView.Center = new Point(gridPhoto.Width / 2  -200 , gridPhoto.Height / 2 );
            // System.Windows.SystemParameters.PrimaryScreenWidth/2;
            // System.Windows.SystemParameters.PrimaryScreenHeight / 2;            

            scatterView.MaxWidth  = System.Windows.SystemParameters.PrimaryScreenWidth ;
            scatterView.MaxHeight = System.Windows.SystemParameters.PrimaryScreenHeight ;
            imgScatter.MaxWidth   = System.Windows.SystemParameters.PrimaryScreenWidth ;
            imgScatter.MaxHeight  = System.Windows.SystemParameters.PrimaryScreenHeight ;

            scatterView.Opacity = 0;
            var eog = new EaseObjectGroup();
            var eo = ArtefactAnimator.AddEase(scatterView, new[] { AnimationTypes.Alpha }, new object[] { 1 }, 1, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);
            eog.Complete += delegate(EaseObjectGroup easeObjectGroup)
            {
                surfaceButtonAddToSelection.Visibility = System.Windows.Visibility.Visible;
                surfaceButtonBackTopGallery.Visibility = System.Windows.Visibility.Visible;
            };

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

        private void surfaceButtonNext_Click(object sender, RoutedEventArgs e)
        {
            goNext();
        }

        private void ViewBox_TouchDown(object sender, TouchEventArgs e)
        {            
            if (LastViewBoxTouch ==(Viewbox) sender)
            {              
              if (_timerDoubleTouch == null)
            {           
                  StartTimerDoubleTouch();                
            }
            else
            {
                _timerDoubleTouch.Stop();
                _timerDoubleTouch = null;

               if (LastViewBoxTouch != null && loadFinished)
                {     
                   openPagePhoto();                
               }
            }
         } 
        else
         {
                if(_timerDoubleTouch!=null) _timerDoubleTouch.Stop();
                _timerDoubleTouch = null;
                LastViewBoxTouch =(Viewbox) sender;                
                StartTimerDoubleTouch();                
         }
             
            /*

            LastViewBoxTouch = (Viewbox)sender;                         
            int selection = surfaceListBox1.SelectedIndex;
            surfaceListBox1.SelectedIndex = -1;
            surfaceListBox1.SelectedIndex = selection;                      
             */             

            /*             
            globalDatasingleton.getInstance().InitCursor(LastViewBoxTouch);
            surfaceListBox1.SelectedIndex = -1;
            Image img = LastViewBoxTouch.Child as Image;
            if (globalDatasingleton.getInstance().getCursorFilename() != "")
            {
                onePhotoPage opp = new onePhotoPage(globalDatasingleton.getInstance().getCursorFilename());
                SurfaceWindow1.staticSetBackPage(this, opp);
            }           
             * */
        }

        private void ViewBox_MouseDown(object sender, MouseButtonEventArgs e)
        {            
            if (LastViewBoxTouch == (Viewbox)sender)
            {
                if (_timerDoubleTouch == null)
                {
                   StartTimerDoubleTouch();
                }
                else
                {
                    _timerDoubleTouch.Stop();
                    _timerDoubleTouch = null;

                    if (LastViewBoxTouch != null && loadFinished)
                    {
                     openPagePhoto();      
                    }
                }
            }
            else
            {
                if (_timerDoubleTouch != null) _timerDoubleTouch.Stop();                
                _timerDoubleTouch = null;
                LastViewBoxTouch = (Viewbox)sender;
                StartTimerDoubleTouch();              
            }
             
            /*
            LastViewBoxTouch = (Viewbox)sender;
            int selection = surfaceListBox1.SelectedIndex;
            surfaceListBox1.SelectedIndex = -1;            
            surfaceListBox1.SelectedIndex = selection;                     
             * */

        }

        private void surfaceListBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          /*  
            if (LastViewBoxTouch != null && surfaceListBox1.SelectedIndex != -1 && loadFinished)
            {
                globalDatasingleton.getInstance().InitCursor(LastViewBoxTouch);
                surfaceListBox1.SelectedIndex = -1;
                Image img = LastViewBoxTouch.Child as Image;
                if (globalDatasingleton.getInstance().getCursorFilename() != "")
                {
                    onePhotoPage opp = new onePhotoPage(globalDatasingleton.getInstance().getCursorFilename());
                    SurfaceWindow1.staticSetBackPage(this, opp);
                }
            }
            else
            {
                if (GlobalConfig.debug == true)
                {
                    if (LastViewBoxTouch == null) MessageBox.Show("LastViewBoxTouch  NULL");
                    if (loadFinished == false) MessageBox.Show(" loadFinish = false");
                }
            }            
           * */
        }
        
        private void surfaceListBox1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        
        }

        private void surfaceSlider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            /*
            int  val = ( int ) e.NewValue ;
            val = nbItem / nbColumn - val;
            if (val>0 && val< surfaceListBox1.Items.Count)
            {
              object obj=  surfaceListBox1.Items.GetItemAt(val);
              surfaceListBox1.ScrollIntoView(obj);
            } 
             * */
        }

        private void buttonNextSet_Click(object sender, RoutedEventArgs e)
        {
            foreach (SurfaceListBoxItem slbi in surfaceListBox1.Items)
            {
                slbi.Content = null;
            }

            nbLoadedImage = 0;
            SendTimerForLoading();
        }

        private void buttonChild_TouchUp(object sender, TouchEventArgs e)
        {
            int indexButton = listChildCategory.Children.IndexOf((UIElement)sender);
            if (indexButton != -1)
            {
                CategoriesXml xmlcat = globalDatasingleton.getInstance().getCategories();                
                ListPhotosPage lpp = new ListPhotosPage(xmlcat.GetChild(ref currentCat, indexButton));
                SurfaceWindow1.staticSetBackPage(this, lpp);
            }            
        }
        private void ShowMessage(string msg)
        {
            textBlockMessage.Text = msg;
            textBlockMessage.Opacity = 0;
            textBlockMessage.Visibility = Visibility.Visible;
            var eog = new EaseObjectGroup();
            //eog.Complete += g => Debug.WriteLine("COMPLETE");
            var eo = ArtefactAnimator.AddEase(textBlockMessage, new[] { AnimationTypes.Alpha }, new object[] { 1 }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);
            eog.Complete += delegate(EaseObjectGroup easeObjectGroup)
            {
                var eog2 = new EaseObjectGroup();
                //eog.Complete += g => Debug.WriteLine("COMPLETE");
                var eo2 = ArtefactAnimator.AddEase(textBlockMessage, new[] { AnimationTypes.Alpha }, new object[] { 0 }, 1.5, AnimationTransitions.CubicEaseOut, 1);
                eog2.AddEaseObject(eo2);
                eog2.Complete += delegate(EaseObjectGroup easeObjectGroup2) { textBlockMessage.Visibility = System.Windows.Visibility.Hidden; };
            };
        }

        private void ShowArrow()
        {
            Arrow.Opacity = 0;
            Arrow.Visibility = Visibility.Visible;
            var eog = new EaseObjectGroup();
            //eog.Complete += g => Debug.WriteLine("COMPLETE");
            var eo = ArtefactAnimator.AddEase(Arrow, new[] { AnimationTypes.Alpha }, new object[] { 1 }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);
            eog.Complete += delegate(EaseObjectGroup easeObjectGroup)
            {
                var eog2 = new EaseObjectGroup();
                //eog.Complete += g => Debug.WriteLine("COMPLETE");
                var eo2 = ArtefactAnimator.AddEase(Arrow, new[] { AnimationTypes.Alpha }, new object[] { 0 }, 1.5, AnimationTransitions.CubicEaseOut, 1);
                eog2.AddEaseObject(eo2);
                eog2.Complete += delegate(EaseObjectGroup easeObjectGroup2) { Arrow.Visibility = System.Windows.Visibility.Hidden; };
            };
        }
        

        public void setModal(bool b)
        {
            dockPanel1.IsEnabled = !b;
        }

        private void surfaceButtonBackTopGallery_Click(object sender, RoutedEventArgs e)
        {
            
            var eog = new EaseObjectGroup();
            gridPhoto.Opacity = 0;
            var eo = ArtefactAnimator.AddEase(gridPhoto, new[] { AnimationTypes.Width, AnimationTypes.Alpha }, new object[] { 1, 0 }, 0.3, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);

            eog.Complete += delegate(EaseObjectGroup easeObjectGroup)
            {
                gridListThumb.Width = System.Windows.SystemParameters.PrimaryScreenWidth - 50;                
                surfaceListBox1.Width = System.Windows.SystemParameters.PrimaryScreenWidth - 50;                
                    surfaceButtonAddToSelection.Visibility = System.Windows.Visibility.Hidden;
                    surfaceButtonBackTopGallery.Visibility = System.Windows.Visibility.Hidden;                
            };

            

            globalDatasingleton.getInstance().InitCursor(LastViewBoxTouch);

            if (globalDatasingleton.getInstance().getCursorFilename() != "")
            {
                String filename = globalDatasingleton.getInstance().getCursorFilename();
                Image img = LastViewBoxTouch.Child as Image;
                MyImage img2 = new MyImage();
                if (filename != "")
                {
                    img2.Source = BitmapUtil.GetImageSource(filename);
                    img2.OriginalPath = filename;
                }
                LoadImg(img2);
            }          
        }

        private void surfaceButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            goPrevious();
        }
    }
}
