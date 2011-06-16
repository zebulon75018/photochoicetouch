using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml;
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
    /// Interaction logic for PhotoPage.xaml
    /// </summary>
    public partial class PhotoSelectionPage : UserControl
    {

        int nbItemInRow = 4;
        Viewbox LastViewBoxTouch = null;
        DispatcherTimer _timer = null;
        List<UserImgSelected> imgList = null;
        int cursor = 0;

        public PhotoSelectionPage()
        {
            InitializeComponent();

            SurfaceWindow1.changeMessagePage("  ");

            SurfaceWindow1.StatiChangeDeconnectionToValidate();

            Utility.gradientManager gm = new gradientManager();
            gm.setgradient(dockPanel1);            

            LoadPuzzleList();
            SurfaceWindow1.IsphotoSelection = true;         
            SurfaceWindow1.StaticShowButton();

            UpdateShowPrice();
        }

        //---------------------------------------------------------//
        /// <summary>
        /// Load all the images/movies in the sample images/movies directories into the puzzle list.
        /// </summary>
        private void LoadPuzzleList()
        {
            _timer = new DispatcherTimer();

            imgList = SelectionUser.getInstance().getImageSelection();
            cursor = 0;
            // Set the Interval to 2 seconds
            _timer.Interval = TimeSpan.FromMilliseconds(50);

            WaitingAnimation.IsAnimated = true;
            gridWating.Visibility = System.Windows.Visibility.Visible;
            SendTimerForLoading();
            // Load photo puzzles          
        }
        private void SendTimerForLoading()
        {
            // Set the callback to just show the time ticking away
            // NOTE: We are using a control so this has to run on 
            // the UI thread
            _timer.Tick += new EventHandler(delegate(object s, EventArgs a)
            {
                if (cursor >= imgList.Count)
                {
                    WaitingAnimation.IsAnimated = false;
                    gridWating.Visibility = System.Windows.Visibility.Hidden;
                    _timer.Stop();
                }
                else
                {
                    
                    /*
                    MyImage img = new MyImage();
                    img.OriginalPath = imgList[cursor].path;
                    //System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                    img.Source =  BitmapUtil.GetImageSource(imgList[cursor].path); //new BitmapImage(new Uri(imgList[cursor].path));
                    AddElementToPuzzleList(img);
                    
                    */                    

                    SurfaceListBoxItem il = new SurfaceListBoxItem();
                    StackPanel sp = new StackPanel();
                    sp.Orientation = Orientation.Horizontal;

                    MyImage img = new MyImage();
                    img.OriginalPath = imgList[cursor].path;
                    //System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                    img.Source = BitmapUtil.GetImageSource(imgList[cursor].path); //new BitmapImage(new Uri(imgList[cursor].path));

                    Viewbox vb = new Viewbox { Width = 200, Child = img };                    
                    vb.TouchDown += ViewBox_TouchDown;
                    //vb.MouseDown += ViewBox_MouseDown;
                    vb.Margin = new Thickness(2);
                    sp.Children.Add(vb);

                    int nbImageToInsert = 0;
                    while (nbImageToInsert < nbItemInRow-1)
                    {
                        if (cursor + 1 < imgList.Count)
                        {
                            cursor++;

                            MyImage img2 = new MyImage();
                            img2.OriginalPath = imgList[cursor].path;
                            //System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                            img2.Source = BitmapUtil.GetImageSource(imgList[cursor].path); //new BitmapImage(new Uri(imgList[cursor].path));

                            Viewbox vb2 = new Viewbox { Width = 200, Child = img2 };
                            vb2.TouchDown += ViewBox_TouchDown;                        
                            vb2.Margin = new Thickness(2);
                            sp.Children.Add(vb2);
                        }
                        nbImageToInsert++;
                    }
                    il.Content = sp;
                    surfaceListBox1.Items.Add(il);

                    cursor++;
                }                                    
            });

            // Start the timer
            _timer.Start();
        }

        //---------------------------------------------------------//
        /// <summary>
        /// Add an image or a movie into the puzzle list
        /// </summary>
        /// <param name="img"></param>
        private void AddElementToPuzzleList(MyImage img)
        {
            Viewbox b = new Viewbox { Width = 200, Child = img };            
            surfaceListBox1.Items.Insert(0, b);//random.Next(0, puzzles.Items.Count), b);
        }

        private void ViewBox_TouchDown(object sender, TouchEventArgs e)
        {
            LastViewBoxTouch = (Viewbox)sender;

            int selection = surfaceListBox1.SelectedIndex;
            surfaceListBox1.SelectedIndex = -1;
            surfaceListBox1.SelectedIndex = selection;
        }

        private void surfaceListBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (surfaceListBox1.SelectedIndex == -1)
            {
                return;
            }
            
            Viewbox selectedI = LastViewBoxTouch;
            if (selectedI == null) return;
            SurfaceApplicationMaurice.MyImage img = selectedI.Child as SurfaceApplicationMaurice.MyImage;            

            scatterViewItem.Width = (System.Windows.SystemParameters.PrimaryScreenWidth - surfaceListBox1.Width / 2 ) * 0.4;
            scatterViewItem.Height = (System.Windows.SystemParameters.PrimaryScreenWidth  - surfaceListBox1.Width/2) * 0.4;

            scatterViewItem.MaxWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            scatterViewItem.MaxHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            imgScatter.MaxWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            imgScatter.MaxHeight = System.Windows.SystemParameters.PrimaryScreenHeight;

            if (img.Source.Width > img.Source.Height)
            {
                scatterViewItem.Height *= img.Source.Height / img.Source.Width;
            }
            else
            {
                scatterViewItem.Width *= img.Source.Width / img.Source.Height;
            }
            imgScatter.Source = img.Source;
            imgScatter.OriginalPath = img.OriginalPath;            

            SelectionUser._ImageSelected = img;

            TransformationUtil.AppearControl(imageFormatPanel);
            TransformationUtil.AppearControl(stackPanel1);
            int widthScreenMoinwidthLst = (int)(System.Windows.SystemParameters.PrimaryScreenHeight - surfaceListBox1.Width)/2;
            int xImageScatter = (int)scatterViewItem.Width / 2 -50;
            scatterViewItem.Center = new System.Windows.Point(xImageScatter, (scatterViewItem.Height/2.0)+ 50);
            scatterViewItem.Visibility = System.Windows.Visibility.Visible;

            /*
            TransformationUtil.AppearControl(stackPanel1);
            var eog = new EaseObjectGroup();
            var eo = ArtefactAnimator.AddEase(surfaceListBox1, new[] { AnimationTypes.Width }, new object[] { 10 }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);
            eog.Complete += delegate(EaseObjectGroup easeObjectGroup) 
            {
                             
            };
             */
            UpdateShowPrice();
        }

        public void ClearList()
        {
            surfaceListBox1.Items.Clear();                    
        }

        
        private void ReplaceImageAfterEffect()
        {
            SelectionUser._ImageSelected = null;
            MyImage img = new MyImage();
            img.OriginalPath = imgScatter.OriginalPath;
            img.Source = imgScatter.Source;
            SelectionUser.getInstance().AddImage(img);


            Viewbox b = null;
            if (img.Source.Width > img.Source.Height)
            {
                b = new Viewbox { Width = 200, Child = img };
            }
            else
            {
                b = new Viewbox { Width = (int)((float)200 * ((float)img.Source.Width / (float)img.Source.Height)), Child = img };
            }
            

            b.TouchDown += ViewBox_TouchDown;
            b.Margin = new Thickness(2);

            addImageFromListView(b);
            gridEffect.Visibility = System.Windows.Visibility.Hidden;
            
            imgScatter.Source = null;
            imgScatter.OriginalPath = "";
            GC.Collect();
            scatterViewItem.Visibility = System.Windows.Visibility.Hidden;
            UpdateShowPrice();
            
        }

        public void ConvertSepia(bool deleteOriginal)
        {            
            this.Cursor = Cursors.Wait;

            if (deleteOriginal)
            {
                SelectionUser.getInstance().DeleteImage(imgScatter.OriginalPath);
                removeImageFromListView(imgScatter.OriginalPath);
            }
            imgScatter.Source = EffectBitmap.convertBitmapToBitmapSource(EffectBitmap.ConvertSepiaTone(EffectBitmap.GetBitmap(imgScatter)));
            this.Cursor = Cursors.Arrow;
            ReplaceImageAfterEffect();            
        }

        public void ConvertBlackAndWhite(bool deleteOriginal)
        {
            this.Cursor = Cursors.Wait;
            //imgScatter.Source = null;
            //imgScatter.OriginalPath = "";
            //GC.Collect();
            if (deleteOriginal)
            {
                SelectionUser.getInstance().DeleteImage(imgScatter.OriginalPath);
                removeImageFromListView(imgScatter.OriginalPath);                
            }
            imgScatter.Source = EffectBitmap.convertBitmapToBitmapSource(EffectBitmap.ConvertBlackAndWhite(EffectBitmap.GetBitmap(imgScatter)));
            this.Cursor = Cursors.Arrow;
                ReplaceImageAfterEffect();            
        }               
          
        private void imageSepia_TouchUp(object sender, TouchEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            
            EffectPage ep = new EffectPage(imgScatter,this);
            gridEffect.Children.Clear();
            gridEffect.Children.Add(ep);
            gridEffect.Margin = new Thickness(System.Windows.SystemParameters.PrimaryScreenWidth / 2 - 500, System.Windows.SystemParameters.PrimaryScreenHeight, 0, 0);

            TransformationUtil.ShowDialog(gridEffect, 600);
            gridEffect.Visibility = System.Windows.Visibility.Visible;

            this.Cursor = Cursors.Arrow;    
            
        }

        // Delete.
        private void image1_TouchUp(object sender, TouchEventArgs e)
        {          
            var eog = new EaseObjectGroup();
            stackPanelQuestionDelete.Opacity = 0;
            var eo = ArtefactAnimator.AddEase(stackPanelQuestionDelete, new[] { AnimationTypes.Height,AnimationTypes.Alpha }, new object[] { 120,1 }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            var eo2 = ArtefactAnimator.AddEase(rectanglePoubelle, new[] { AnimationTypes.Height}, new object[] { 1 }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);
            eog.AddEaseObject(eo2);         
        }

        private void CloseQuestionPanel(ref StackPanel p)
        {
            var eog = new EaseObjectGroup();
            var eo = ArtefactAnimator.AddEase(stackPanelQuestionDelete, new[] { AnimationTypes.Height,AnimationTypes.Alpha }, new object[] { 0,0 }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            var eo2 = ArtefactAnimator.AddEase(rectanglePoubelle, new[] { AnimationTypes.Height }, new object[] { 25 }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);
            eog.AddEaseObject(eo2);          
        }

        private void buttonDeleteImage_Click(object sender, RoutedEventArgs e)
        {
            if (SelectionUser._ImageSelected == null) return;

            String FilenameImage = SelectionUser._ImageSelected.OriginalPath;
            SelectionUser._ImageSelected = null;
            //Viewbox vbSelected = (Viewbox)surfaceListBox1.SelectedItem;
            Viewbox vbSelected = LastViewBoxTouch;
            if (vbSelected == null) return;

            removeImageFromListView(FilenameImage);
            imgScatter.Source = null;
            imgScatter.OriginalPath = "";
            GC.Collect();
            scatterViewItem.Visibility = System.Windows.Visibility.Hidden;            
            /* TODO
            surfaceListBox1.Items.Remove(surfaceListBox1.SelectedItem);            
             * 
            
             * */
            CloseQuestionPanel(ref stackPanelQuestionDelete);
            SelectionUser.getInstance().DeleteImage(FilenameImage);
            TransformationUtil.DisAppearControl(imageFormatPanel);
            TransformationUtil.DisAppearControl(stackPanel1);

            UpdateShowPrice();
           // textPrice.Text = "";     
        }

        private void buttonCancelDelete_Click(object sender, RoutedEventArgs e)
        {
            CloseQuestionPanel(ref stackPanelQuestionDelete);
        }
        

        private void imageSepia_MouseUp(object sender, MouseButtonEventArgs e)
        {            
            EffectPage ep = new EffectPage(imgScatter,this);
            gridEffect.Children.Clear();
            gridEffect.Children.Add(ep);
            gridEffect.Margin = new Thickness(System.Windows.SystemParameters.PrimaryScreenWidth / 2 - 500, System.Windows.SystemParameters.PrimaryScreenHeight, 0, 0);

            TransformationUtil.ShowDialog(gridEffect, 600);
            gridEffect.Visibility = System.Windows.Visibility.Visible;
        }

        private void ShowFormat(bool goody=false,bool onbook=false)
        {
            FormatDialogImage ep = new FormatDialogImage(imgScatter.OriginalPath, this, SelectionUser.getInstance().getXmlUserNode(imgScatter.OriginalPath), goody,onbook);            
            gridEffect.Children.Clear();
            gridEffect.Children.Add(ep);
            gridEffect.Margin = new Thickness(System.Windows.SystemParameters.PrimaryScreenWidth / 2 - 500, System.Windows.SystemParameters.PrimaryScreenHeight, 0, 0);

            TransformationUtil.ShowDialog(gridEffect, 600);
            gridEffect.Visibility = System.Windows.Visibility.Visible;
            UpdateShowPrice();
        }

        private void bookOrder()
        {
            BookPhotoOrder oioc = new BookPhotoOrder(this);
            gridEffect.Children.Clear();
            gridEffect.Children.Add(oioc);
            gridEffect.Margin = new Thickness(System.Windows.SystemParameters.PrimaryScreenWidth / 2 - 500, System.Windows.SystemParameters.PrimaryScreenHeight, 0, 0);

            TransformationUtil.ShowDialog(gridEffect, 600);
            gridEffect.Visibility = System.Windows.Visibility.Visible;
        }

        private void ShowOrderCD()
        {
            orderImageOnCd oioc = new orderImageOnCd(this);
            gridEffect.Children.Clear();
            gridEffect.Children.Add(oioc);
            gridEffect.Margin = new Thickness(System.Windows.SystemParameters.PrimaryScreenWidth / 2 - 500, System.Windows.SystemParameters.PrimaryScreenHeight, 0, 0);

            TransformationUtil.ShowDialog(gridEffect, 600);
            gridEffect.Visibility = System.Windows.Visibility.Visible;
        }


        public void StartModal()
        {
            foreach (UIElement elm in this.gridBackground.Children)
            {
                elm.IsEnabled = false;
            }
            gridEffect.IsEnabled = true;

            SurfaceWindow1.startModal();
                        
        }

        public void StopModal()
        {
            foreach (UIElement elm in this.gridBackground.Children)
            {
                elm.IsEnabled = true;
            }
            SurfaceWindow1.stopModal();
        }

        public void updateBookPhoto(bool ?bookPhoto,bool ?allPhotoOnBook,String TxtNbPages)
        {
            LoginManager m = LoginControler.getLoginManager();
            if (m != null)
            {
                if (bookPhoto == true)
                {
                    m.SetBookOrder(true);
                    Int32 nbPage = 20;
                    Int32.TryParse(TxtNbPages, out nbPage);
                    m.SetNbPageOnBook(nbPage);
                    if (allPhotoOnBook == true)
                    {
                        m.setAllPhotoBookBoolean(true);
                    }
                }
                else
                {
                    m.SetBookOrder(false);
                    m.SetNbPageOnBook(0);
                    m.setAllPhotoBookBoolean(false);
                }
                UpdateShowPrice();
            }
        }

        public void updateImageOnCd(bool? allImageOnCd, bool? onCdWithoutPaper,bool? noPhotoOnCd)
        {
            LoginManager m = LoginControler.getLoginManager();
            if (m != null)
            {
                if (allImageOnCd == true)
                {
                    m.setAllImageOnCD(true);
                }
                if (onCdWithoutPaper == true)
                {
                    m.resetAllAcceptCD();
                }
                if (noPhotoOnCd == true)
                {
                    m.setAllImageOnCD(false);
                }
                // L'utlisateur ne voulait que des photos sur cd et a changer d'avis.
                if (m.isCDWithoutPrint() == true && onCdWithoutPaper == false)
                {
                    m.unResetAllCD();
                }
                m.Save();
                UpdateShowPrice();
            }
        }

        private void imageFormat_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ShowFormat();
        }

        private void imageFormat_TouchUp(object sender, TouchEventArgs e)
        {
            ShowFormat();
        }

        public void UpdateShowPrice()
        {
            int nbPhotoOnCd = 0;
            int nbPhotoOnBook = 0;
            LoginManager.ResultPrice total = new LoginManager.ResultPrice();
            LoginManager m = LoginControler.getLoginManager();
            if (m != null)
            {                   
               numberOfTotalPhoto.Content = m.getNbDifferentPicture().ToString();               

             List<String> lstImg = m.listImageUserSelection();
              foreach(String filenameImage in lstImg)
              {               
                XmlNode nodePhoto = m.getNodeImageUser(filenameImage);
                LoginManager.ResultPrice result = m.getPriceOnPicture(nodePhoto);
                total.nbFormatPhoto += result.nbFormatPhoto;
                total.TotalPriceFormatPhoto += result.TotalPriceFormatPhoto;
                total.TotalPriceObjetPhoto += result.TotalPriceObjetPhoto;
                total.nbObjetPhoto += result.nbObjetPhoto;                  
                if (result.putPIctureInCD == true)
                {
                    nbPhotoOnCd += 1;
                }

                if (result.putPIctureOnBook == true)
                {
                    nbPhotoOnBook += 1;
                }
                
              }

              

                // Affichage
              UInt64 pourcentage = productConfig.getInstance().getPourcentageReduction(total.nbFormatPhoto);
              numberOfPrintPhoto.Content = total.nbFormatPhoto.ToString();
              numberOfPhotoOnBook.Content = m.getNbPageOkBook().ToString();
              TotalPricePhoto.Content = total.TotalPriceFormatPhoto.ToString() + " Mur";
                numberOfPhotoOnObjet.Content = total.nbObjetPhoto;
                TotalPricePhotoObject.Content = total.TotalPriceObjetPhoto.ToString() + " Mur";

                UInt64 pricepictureOnBook = 0;
                if (m.isBookOrder())
                {
                    pricepictureOnBook =(UInt64) productConfig.getInstance().getPrixPrestigeBook();
                    UInt64 pageMore = (UInt64)m.getNbPageOkBook() - 20;
                    pricepictureOnBook += pageMore * (UInt64)productConfig.getInstance().getPrixPrestigePageMore();
                }

                TotalPricePictureBook.Content = pricepictureOnBook.ToString() + " Mur";


                // Calcul pour les images sur CD

                UInt64 pricePcitureOnCd = 0;
                if (m.isCDWithoutPrint())
                {
                    if (m.getNbDifferentPicture() == 50)
                    {
                        pricePcitureOnCd = (UInt64)productConfig.getInstance().getForfait50CD();
                    }
                    else
                    {
                        if (m.getNbDifferentPicture() > 50)
                        {
                            pricePcitureOnCd = (UInt64)productConfig.getInstance().getForfait100CD();
                        }
                    }
                }
                else
                {
                    pricePcitureOnCd = (UInt64)(nbPhotoOnCd * productConfig.getInstance().getCDUnitPrice());
                }
                
                
                TotalPricePictureCD.Content = pricePcitureOnCd.ToString() + " Mur";

              if (pourcentage > 0)
              {                  
                  PromotionatinalPrice.Visibility = System.Windows.Visibility.Visible;                  
                  LabelPromotionalPrice.Visibility = System.Windows.Visibility.Visible;
                  //Pourcentage.Content = pourcentage.ToString() + "%";                  
                  UInt64 PromotionalPriceglobal = total.TotalPriceFormatPhoto - (pourcentage * total.TotalPriceFormatPhoto) / 100;
                  PromotionatinalPrice.Content = PromotionalPriceglobal.ToString() + " Mur ( -"+ pourcentage.ToString()+" % )" ;
              }
              else
              {                  
                  PromotionatinalPrice.Visibility = System.Windows.Visibility.Hidden;                                    
                  LabelPromotionalPrice.Visibility = System.Windows.Visibility.Hidden;                  
                  PromotionatinalPrice.Content = "";                  
              }
              //PromotionatinalPricetextPrice.Text = " Price " + total.TotalPriceFormatPhoto.ToString() + " " + total.TotalPriceObjetPhoto.ToString() + " " + pourcentage.ToString() + "%";
              numberOfPhotoOnCd.Content = nbPhotoOnCd.ToString();

             // On calcule le prix total.

                UInt64 calculDuprixTotal =0;
            if (pourcentage > 0)
            {
               calculDuprixTotal = total.TotalPriceFormatPhoto - (total.TotalPriceFormatPhoto*pourcentage)/100;
            }
            else
            {
                calculDuprixTotal = total.TotalPriceFormatPhoto;
            }

            calculDuprixTotal += total.TotalPriceObjetPhoto;
            calculDuprixTotal += pricePcitureOnCd + pricepictureOnBook;
            TotalPrice.Content = calculDuprixTotal.ToString() + " Mur";
            }
            
            /*
            LoginManager m = LoginControler.getLoginManager();
            if (m != null)
            {
                textFormat.Text = m.getFormatImage(imgScatter.OriginalPath);
            }
             * */
        }

        private void openSelectionImages()
        {
            var eog = new EaseObjectGroup();
            var eo = ArtefactAnimator.AddEase(surfaceListBox1, new[] { AnimationTypes.Width }, new object[] { 1000 }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);
            eog.Complete += delegate(EaseObjectGroup easeObjectGroup)
            {
                TransformationUtil.DisAppearControl(imageFormatPanel);
                TransformationUtil.DisAppearControl(stackPanel1);
                /*surfaceListBox1.Visibility = System.Windows.Visibility.Hidden; */
            };
        }

        private void imageCD_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ShowOrderCD();
        }      

        private void imageOpenListSelection_TouchDown(object sender, TouchEventArgs e)
        {
            openSelectionImages();
        }

        private void imageOpenListSelection_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //openSelectionImages();
        }

        private void imageCD_TouchUp(object sender, TouchEventArgs e)
        {
            ShowOrderCD();
        }

        private void removeImageFromListView(String filename)
        {
            for (int n = 0; n < surfaceListBox1.Items.Count; n++)
            {
                SurfaceListBoxItem li = surfaceListBox1.Items.GetItemAt(n) as SurfaceListBoxItem;
                StackPanel sp = li.Content as StackPanel;
                int itemToDelete = -1;
                for (int i = 0; i < sp.Children.Count; i++)
                {
                    Viewbox vb = sp.Children[i] as Viewbox;
                    MyImage img = vb.Child as MyImage;
                    if (img.OriginalPath == filename)
                    {
                        itemToDelete = i;
                        vb.TouchDown -= ViewBox_TouchDown;
                    }
                }
                if (itemToDelete != -1)
                {
                    Viewbox vb = sp.Children[itemToDelete] as Viewbox;
                    sp.Children.RemoveAt(itemToDelete);
                    vb = null;
                    GC.Collect();
                }
            }
            removeListBoxItemEmpty();
            UpdateShowPrice();
        }

        /*
         *  Elimine les SurfaceListBoxItem qui sont vides.
         */
        private void removeListBoxItemEmpty()
        {
            int indexToDelete = -1;
            for (int n = 0; n < surfaceListBox1.Items.Count; n++)
            {
                SurfaceListBoxItem li = surfaceListBox1.Items.GetItemAt(n) as SurfaceListBoxItem;
                StackPanel sp = li.Content as StackPanel;
                if (sp.Children.Count == 0)
                {
                    indexToDelete = n;
                }
            }
            if (indexToDelete != -1)
            {
                surfaceListBox1.Items.RemoveAt(indexToDelete);
            }
        }

        
        /*
        private void updatePriceAndImage()
        {
             LoginManager m = LoginControler.getLoginManager();
            if (m != null)
            {
               numberOfTotalPhoto.Content = m.getNbDifferentPicture().ToString();
               numberOfPrintPhoto.Content = m.nbNbPhotoOnCD().ToString();               
               UpdateShowPrice();
            }
        } 
         * */


        private void addImageFromListView(Viewbox img)
        {
            for (int n = 0; n < surfaceListBox1.Items.Count; n++)
            {
                SurfaceListBoxItem li = surfaceListBox1.Items.GetItemAt(n) as SurfaceListBoxItem;
                StackPanel sp = li.Content as StackPanel;

                if (sp.Children.Count < nbItemInRow)
                {
                    sp.Children.Add(img);
                    return;
                }                
            }

            SurfaceListBoxItem il = new SurfaceListBoxItem();
            StackPanel sp2 = new StackPanel();
            sp2.Orientation = Orientation.Horizontal;
            il.Content = sp2;
            surfaceListBox1.Items.Add(il);
        }        

        private void imageGoodie_TouchUp(object sender, TouchEventArgs e)
        {
            ShowFormat(true);
        }

        private void imageGoodie_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ShowFormat(true);
        }

        private void imageLivreFormat_TouchUp(object sender, TouchEventArgs e)
        {
            bookOrder();
            //ShowFormat(false, true);
        }

        private void imageLivreFormat_MouseUp(object sender, MouseButtonEventArgs e)
        {
            bookOrder();
            //ShowFormat(false, true);
        }                 
    }
}
