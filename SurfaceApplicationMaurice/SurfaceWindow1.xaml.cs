using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using Artefact.Animation;
using System.Diagnostics;
using SurfaceApplicationMaurice.SubPages;
using WPFLocalization;
using SurfaceApplicationMaurice.Utility;
using SurfaceApplicationMaurice.NetWork;
using System.Threading;

namespace SurfaceApplicationMaurice
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class SurfaceWindow1 : SurfaceWindow
    {
        public delegate void changeXmlFileWatcher(); 
        //Pages...
        InformationCategory infoPage = new InformationCategory();
        MeteoPage meteoPage = new MeteoPage();
        MapPage mapPage = new MapPage();
        LanguagePage languagePage = new LanguagePage();
        ListCategoryPhotoPage photoPage = new ListCategoryPhotoPage();
        static bool IsphotoMenu = false;
        public static bool IsphotoSelection = false;

        static SurfaceWindow1 _instance = null;
        private Stack<UserControl> _backStack = null;

        private int versionMajor = 0;
        private int versionMineur = 95;

        // Variable pour fermer l'application en cliquant sur le logo de l'hotel
        DispatcherTimer _timerClose = null;
        int nbClickForClosingApplication = 0;

        // Un timer pour checker si le screensaver est actif ou non .
        DispatcherTimer _timerScreenSaver = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            Splasher.Splash = new SplashScreen(versionMajor.ToString() + "." + versionMineur.ToString());
            Splasher.ShowSplash();

            Thread.Sleep(5);
            LocalizationManager.UICulture = new CultureInfo("fr-FR");

            string netresult="";
            try
            {                
                Thread.Sleep(1);

                VersionChecker vc = new VersionChecker();
                netresult = vc.GetNetworkVersion();               

                MessageListener.Instance.ReceiveMessage("Update Check");
                Thread.Sleep(1); 
                
                char[] separator = new char[1];
                separator[0] = '.';
                string[] splitString = netresult.Split(separator);

                if (splitString.Length == 2)
                {
                    if (Int32.Parse(splitString[0].ToString()) >= versionMajor && Int32.Parse(splitString[1].ToString()) > versionMineur)
                    {
                        if (MessageBox.Show("New version is available , do you want to download it ? ", "Update", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            vc.DownloadFile();
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
            Splasher.Splash.Close();            
            
            InitializeComponent();

            info.Label = LocalizationManager.ResourceManager.GetString("Information");
            info.ImageSource = "pack://application:,,,/Resources/news.jpg";

            
            //MeteoButton.Label = "Meteo";
            //MeteoButton.ImageSource = @"C:\Users\c\Pictures\meteo.jpg";
            //MapButton.Label = "Map";
            //MapButton.ImageSource = @"C:\Users\c\Pictures\map_maurice.jpg";
            PhotoButton.Label = LocalizationManager.ResourceManager.GetString("Photo");
            PhotoButton.ImageSource = "pack://application:,,,/Resources/photo.jpg";
            languageButton.Label = LocalizationManager.ResourceManager.GetString("Language");
            languageButton.ImageSource = "pack://application:,,,/Resources/drapeau.jpg";

                        
            try
            {
                if (CommunConfig.getInstance().InfoGuiImage != "")
                {
                    FileInfo fi = new FileInfo(CommunConfig.getInstance().InfoGuiImage);
                    if (fi.Exists)
                    {
                        //  imageBackground.Source = new BitmapImage(new Uri(fi.FullName));
                        info.ImageSource = fi.FullName;
                    }
                }

                if (CommunConfig.getInstance().PhotoGuiImage != "")
                {
                    FileInfo fi = new FileInfo(CommunConfig.getInstance().PhotoGuiImage);
                    if (fi.Exists)
                    {
                        if (fi.Extension.ToLower() == ".gif")
                        {
                            PhotoButton.ImageGifSource = fi.FullName;
                        }
                        else
                        {
                            PhotoButton.ImageSource = fi.FullName;
                        }
                    }
                }
                
                /*
                fi = new FileInfo(CommunConfig.getInstance().LanguageImage);
                if (fi.Exists) { languageButton.ImageSource = fi.FullName; }
                 * */
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            _instance = this;
            _backStack = new Stack<UserControl>();            

            
            Utility.gradientManager gm = new gradientManager();
            gm.setgradient(gridBackground);
            gm.setgradient(grid1);
            gm.setgradient(gridLogin);
            gm.setgradient(gridCreationLogin);

            _timerScreenSaver = new DispatcherTimer();
            // Set the Interval to 2 seconds
            _timerScreenSaver.Interval = TimeSpan.FromSeconds(15);

            _timerScreenSaver.Tick += new EventHandler(delegate(object s, EventArgs a)
            {
               // System.Console.WriteLine("TEST SCREENSAVER");
                if (Utility.ScreenSaverController.GetScreenSaverRunning())
                {
                    LoginControler.Disconnect();
                    GotoMenu();
                    //System.Console.WriteLine("SCREENSAVER ACTIF");
                }
            });

            _timerScreenSaver.Start();

            //gridLogin.Margin = new Thickness(this.Width / 2 - 250, this.Height,0,0);
            //gridCreationLogin.Margin = new Thickness(this.Width / 2 - 250, this.Height,0,0);
        }

        private static Grid getGridPage()
        {
            return _instance.grid1;
        }

       

        private void animationPageIn()
        {            
            var eog = new EaseObjectGroup();            
            var eo = ArtefactAnimator.AddEase(grid1, new[] { AnimationTypes.Width, AnimationTypes.Height, AnimationTypes.MarginLeft, AnimationTypes.MarginTop }, new object[] { this.Width, this.Height, 10, 10 }, 0.3, AnimationTransitions.CubicEaseIn, 0);
            eog.AddEaseObject(eo);            
        }

        private void RemoveLastPage()
        {
            if (conteneur.Children.Count > 1)
            {
                conteneur.Children.Remove(conteneur.Children[1]);
                //dockPanel1.Children.Remove(dockPanel1.Children[1]);
            }                                  
        }

        private void info_MouseUp(object sender, MouseButtonEventArgs e)
        {
            IsphotoMenu = false;
            if (conteneur.Children.Count == 0)
            {
                conteneur.Children.Add(infoPage);
            }
            animationPageIn();
            ShowButton();
        }

        //
        // C'est l'action du bouton retour.
        //
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            buttonDisconnect.Content = LocalizationManager.ResourceManager.GetString("Disconnect");
            if (IsCategoryInfoPageShow())
            {
                SurfaceWindow1.HideLoginButton();
                setMessagePage("");
                //  TransformationUtil.HideDialog(gridCreationLogin);                 
                var eog = new EaseObjectGroup();
                var eo = ArtefactAnimator.AddEase(grid1, new[] { AnimationTypes.MarginLeft, AnimationTypes.MarginTop }, new object[] { this.Width, this.Height }, 1.5, AnimationTransitions.CubicEaseOut, 0);
                eog.AddEaseObject(eo);
                IsphotoMenu = false;
                IsphotoSelection = false;
                SurfaceWindow1._instance.ShowButton();
                conteneur.Children.Remove(conteneur.Children[0]);                
                return;
            }

                
                if (IsCategoryPageShow())
                {                    
                    ListCategoryPhotoPage catPage = (ListCategoryPhotoPage)getPageCategory();
                    if (catPage.GoBackInParentCategory() ==false)
                    {
                        if (LoginControler.IsLogged())
                        {

                            catPage.ShowQuestionLogOut();
                            return;                            
                        }

                        SurfaceWindow1.HideLoginButton();
                        //  TransformationUtil.HideDialog(gridCreationLogin);                 
                        var eog = new EaseObjectGroup();
                        var eo = ArtefactAnimator.AddEase(grid1, new[] { AnimationTypes.MarginLeft, AnimationTypes.MarginTop }, new object[] { this.Width, this.Height }, 1.5, AnimationTransitions.CubicEaseOut, 0);
                        eog.AddEaseObject(eo);
                        IsphotoSelection = false;
                        IsphotoMenu = false;
                        conteneur.Children.Remove(conteneur.Children[0]);                        
                    }
                }
                else
                {
                    if (conteneur.Children.Count == 0)
                    {
                        if (GlobalConfig.debug) MessageBox.Show("Error can't go back");
                        return;
                    }
                    UIElement last = conteneur.Children[conteneur.Children.Count - 1];
                    conteneur.Children.RemoveAt(conteneur.Children.Count - 1);
                    last = null;

                    if (_backStack.Count > 0)
                    {
                        conteneur.Children.Add(_backStack.Pop());
                        if (IsCategoryPageShow())
                        {
                            ListCategoryPhotoPage catPage = (ListCategoryPhotoPage)getPageCategory();
                            setMessagePage( LocalizationManager.ResourceManager.GetString("ChoiceACategory"));
                            catPage.UpdateList();
                        }
                    }
                    else
                    {
                        setMessagePage(" ");
                    }
                }
                SurfaceWindow1._instance.ShowButton();
                //if (GlobalConfig.debug) MessageBox.Show("backStack Page " +_backStack.Count.ToString());
        }

        private void MeteoButton_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void MeteoButton_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RemoveLastPage();
            conteneur.Children.Add(meteoPage);
            animationPageIn();         
        }

        private void MapButton_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void MapButton_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RemoveLastPage();
            conteneur.Children.Add(mapPage);
            animationPageIn();         
        }

        private void PhotoPage_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EnterInPhotoScreen(); 
        }

        private void LanguagePage_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            IsphotoMenu = false;
            RemoveLastPage();
            conteneur.Children.Add(languagePage);
            animationPageIn();
            ShowButton();
        }

        private void languageButton_TouchUp(object sender, TouchEventArgs e)
        {
            IsphotoMenu = false;
            RemoveLastPage();
            conteneur.Children.Add(languagePage);
            animationPageIn();
            ShowButton();
        }

        private void EnterInPhotoScreen()
        {
            IsphotoMenu = true;
            //GotoMenu();
            photoPage.resetSelectionList();
            if (conteneur.Children.Count == 0)
            {
                //_backStack.Push(photoPage);
                conteneur.Children.Add(photoPage);                
            }
            animationPageIn();
            IsphotoMenu = true;
            setMessagePage(LocalizationManager.ResourceManager.GetString("ChoiceACategory"));
            ShowButton();
        }

        private void PhotoButton_TouchUp(object sender, TouchEventArgs e)
        {
            EnterInPhotoScreen();        
        }

        private void MeteoButton_TouchUp(object sender, TouchEventArgs e)
        {
            RemoveLastPage();
            conteneur.Children.Add(meteoPage);
            animationPageIn();         
        }

        private void MapButton_TouchUp(object sender, TouchEventArgs e)
        {
            RemoveLastPage();
            conteneur.Children.Add(mapPage);
            animationPageIn();         
        }

        private void ShowLoginPage()
        {
            gridLogin.Children.Clear();
            LoginPage lp = new LoginPage();
            gridLogin.Children.Add(lp);
            TransformationUtil.ShowDialog(gridLogin, 350);

            setModal(true);
            //var eog = new EaseObjectGroup();
            //eog.Complete += g => Debug.WriteLine("COMPLETE");
            //var eo = ArtefactAnimator.AddEase(gridLogin, new[] {  AnimationTypes.MarginTop }, new object[] { this.Height / 2 - 350 }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            //eog.AddEaseObject(eo);              
        }
         
    
        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            if (LoginControler.IsLogged() == false)
            {
                LoginControler.wantToAccessToSelection = true;                
                ShowLoginPage();
            }
            else
            {
                ShowSelection();
            }

        }

        private void ShowSelection()
        {
            if (IsSelectionPageShow() == true) return;

            PhotoSelectionPage psp = new PhotoSelectionPage();
            SurfaceWindow1.staticSetBackPage((UserControl)conteneur.Children[conteneur.Children.Count - 1], psp); 
        }

        private void GotoMenu()
        {            
            _backStack.Clear();
            SurfaceWindow1.HideLoginButton();
            //  TransformationUtil.HideDialog(gridCreationLogin);                 
            var eog = new EaseObjectGroup();
            var eo = ArtefactAnimator.AddEase(grid1, new[] { AnimationTypes.MarginLeft, AnimationTypes.MarginTop }, new object[] { this.Width, this.Height }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);
            IsphotoSelection = false;
            IsphotoMenu = false;
            conteneur.Children.Clear();   
            ShowButton();                       
        }


        private void HideLogin()
        {            
            TransformationUtil.HideDialog(gridLogin);
            setModal(false);
            //var eog = new EaseObjectGroup();
            //var eo = ArtefactAnimator.AddEase(gridLogin, new[] {  AnimationTypes.MarginTop }, new object[] { this.Height }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            //eog.AddEaseObject(eo);          
        }

        private void HideCreationLogin()
        {            
            TransformationUtil.HideDialog(gridCreationLogin);
            setModal(false);
            //var eog = new EaseObjectGroup();
            //var eo = ArtefactAnimator.AddEase(gridCreationLogin, new[] { AnimationTypes.MarginTop }, new object[] { this.Height }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            //eog.AddEaseObject(eo);
        }

        private void showButtonHomeCategory(bool b)
        {
            if (b == false)
            {
                TransformationUtil.DisAppearControl(buttonHomeCategory);
            }
            else
            {
                TransformationUtil.AppearControl(buttonHomeCategory);
            }
        }
      
        private void ShowCreationLoginPage()
        {
            HideLogin();
            this.gridCreationLogin.Children.Clear();
            CreationLoginPage lp = new CreationLoginPage();
            lp.Opacity = 1;            
            gridCreationLogin.Children.Add(lp);

            TransformationUtil.ShowDialog(gridCreationLogin, 350);
            setModal(true);
        }

        private void setBackPage(UserControl backpage, UserControl nextPage)
        {
            _backStack.Push(backpage);
            if (conteneur.Children.Contains(backpage))
            {
                conteneur.Children.Remove(backpage);
            }
            conteneur.Children.Add(nextPage);            
        }

        private bool isListPhotoInsert()
        {
            foreach (UIElement elm in conteneur.Children)
            {
                if (elm.GetType().ToString() == "SurfaceApplicationMaurice.SubPages.ListPhotosPage")
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsCategoryPageShow()
        {
            foreach (UIElement elm in conteneur.Children)
            {
                if (elm.GetType().ToString() == "SurfaceApplicationMaurice.SubPages.ListCategoryPhotoPage")
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsSelectionPageShow()
        {
            foreach (UIElement elm in conteneur.Children)
            {
                if (elm.GetType().ToString() == "SurfaceApplicationMaurice.SubPages.PhotoSelectionPage")
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsCategoryInfoPageShow()
        {
            foreach (UIElement elm in conteneur.Children)
            {
                if (elm.GetType().ToString() == "SurfaceApplicationMaurice.SubPages.InformationCategory")
                {
                    return true;
                }
            }
            return false;
        }        

        private bool IsCategoryPageHome()
        {
            foreach (UIElement elm in conteneur.Children)
            {
                if (elm.GetType().ToString() == "SurfaceApplicationMaurice.SubPages.ListCategoryPhotoPage")
                {
                    ListCategoryPhotoPage lcpp = (ListCategoryPhotoPage)elm;
                    return lcpp.CurrentElmIsNull();                    
                }
            }
            return false;
        }


        private UIElement getPageCategory()
        {
            foreach (UIElement elm in conteneur.Children)
            {
                if (elm.GetType().ToString() == "SurfaceApplicationMaurice.SubPages.ListCategoryPhotoPage")
                {
                    return elm;
                }
            }
            return null;
        }

        public void ChangeButtonText()
        {
            info.Label = LocalizationManager.ResourceManager.GetString("Information");
            PhotoButton.Label = LocalizationManager.ResourceManager.GetString("Photo");            
            languageButton.Label = LocalizationManager.ResourceManager.GetString("Language");            
            
            if (infoPage != null)            
            {
                infoPage.UpdateList();
            }

            if (photoPage != null)
            {
                photoPage.UpdateList();
            }

        }

        public void GotoMenuCategory()
        {
            buttonDisconnect.Content = LocalizationManager.ResourceManager.GetString("Disconnect");
            ListCategoryPhotoPage catPage = (ListCategoryPhotoPage)getPageCategory();
            if (catPage != null)
            {
                catPage.resetSelectionList();
                catPage.UpdateList();
                return;
            }
            
            while (_backStack.Count> 0)
            {
                if (conteneur.Children.Count == 0)
                {
                    conteneur.Children.Add(photoPage);
                }

                UIElement last = conteneur.Children[conteneur.Children.Count - 1];
                conteneur.Children.RemoveAt(conteneur.Children.Count - 1);
                last = null;

                last = _backStack.Pop();
                if (last.GetType().ToString() == "SurfaceApplicationMaurice.SubPages.ListCategoryPhotoPage")
                {
                    catPage = (ListCategoryPhotoPage)last;
                    conteneur.Children.Add(last);
                    catPage.resetSelectionList();
                    catPage.UpdateList();
                    return;
                }
            }            
        }

        private void setModal(bool modal)
        {
            if (modal)
            {
                stackPanel1.IsEnabled = false;
            }
            else
            {
                stackPanel1.IsEnabled = true;
            }

            if (conteneur.Children.Count == 0) return;

            UIElement last = conteneur.Children[conteneur.Children.Count - 1];
            if (last.GetType().ToString() == "SurfaceApplicationMaurice.SubPages.ListCategoryPhotoPage")
            {
                ListCategoryPhotoPage catPage = (ListCategoryPhotoPage)last;
                catPage.setModal(modal);
            }
            if (last.GetType().ToString() == "SurfaceApplicationMaurice.SubPages.ListPhotosPage")
            {
                ListPhotosPage page = (ListPhotosPage)last;
                page.setModal(modal);
            }

            if (last.GetType().ToString() == "SurfaceApplicationMaurice.SubPages.onePhotoPage")
            {
                onePhotoPage page = (onePhotoPage)last;
                page.setModal(modal);
            }
        }       

        public void changeDeconnectionToValidate()
        {
            buttonDisconnect.Content = LocalizationManager.ResourceManager.GetString("Validate");
        }

#region STATIC

        public static void StatiChangeDeconnectionToValidate()
        {
            SurfaceWindow1._instance.changeDeconnectionToValidate();            
        }

        public static void StaticHideLoginPage()
        {
            SurfaceWindow1._instance.HideLogin();            
        }

        public static void StaticShowCreationLoginPage()
        {
            SurfaceWindow1._instance.ShowCreationLoginPage();           
        }
        public static void StaticHideCreationLoginPage()
        {
            SurfaceWindow1._instance.HideCreationLogin();            
        }

        public static void StaticShowLoginPage()
        {
            SurfaceWindow1._instance.ShowLoginPage();            

        }

        public static void StaticGotoMenu()
        {
            SurfaceWindow1._instance.GotoMenu();
        }

        public static void StaticGotoSelection()
        {
            SurfaceWindow1._instance.ShowSelection();
        }

        public static void StaticShowButton()
        {
            SurfaceWindow1._instance.ShowButton();
        }

        public static bool StaticIsListPhotoInsert()
        {
            return SurfaceWindow1._instance.isListPhotoInsert();
        }

        // Modification par FileWatcher
        public static void CategoryChanged()
        {
            SurfaceWindow1._instance.Dispatcher.BeginInvoke(new changeXmlFileWatcher(changeCategoryXml));
        }

        public static void InfoChanged()
        {
            SurfaceWindow1._instance.Dispatcher.BeginInvoke(new changeXmlFileWatcher(changeInfoXml));
        }

            
        
        public static void closePage()
        {
            var eog = new EaseObjectGroup();
            //eog.Complete += g => Debug.WriteLine("COMPLETE");

            var eo = ArtefactAnimator.AddEase(SurfaceWindow1.getGridPage(), new[] { AnimationTypes.MarginLeft, AnimationTypes.MarginTop }, new object[] { SurfaceWindow1._instance.Width, SurfaceWindow1._instance.Height }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);
        }

        public static void staticSetBackPage(UserControl backpage,UserControl nextPage)
        {
            _instance.setBackPage(backpage, nextPage);
        }

        public static void HideLoginButton()
        {
            StaticShowButton();
        }

        public static void ShowLoginButton()
        {
            StaticShowButton();
        }


        public static void UpdateLists()
        {
            SurfaceWindow1._instance.photoPage.UpdateList();
            SurfaceWindow1._instance.infoPage.UpdateList();            
        }

        static public void changeInfoXml()
        {
            SurfaceWindow1._instance.infoPage.UpdateList();            
        }

        static public void changeCategoryXml()
        {
            SurfaceWindow1._instance.photoPage.UpdateList();
        }

        static public void changeLanguage()
        {
            SurfaceWindow1._instance.ChangeButtonText();
        }

        static public void gotoHomeCategory()
        {
            SurfaceWindow1._instance.GotoMenuCategory();
        }

        static public void hideHomeCategoryButton()
        {
            if (SurfaceWindow1._instance != null)
            {
                SurfaceWindow1._instance.showButtonHomeCategory(false);
            }
        }

        static public void showHomeCategoryButton()
        {
            if (SurfaceWindow1._instance != null)
            {
                SurfaceWindow1._instance.showButtonHomeCategory(true);
            }
        }
        static public void startModal()
        {
            if (SurfaceWindow1._instance != null)
            {
                SurfaceWindow1._instance.setModal(true);
            }
        }

        static public void stopModal()
        {
            if (SurfaceWindow1._instance != null)
            {
                SurfaceWindow1._instance.setModal(false);
            }
        }

        static public void changeMessagePage(String m)
        {
            if (SurfaceWindow1._instance != null)
            {
                SurfaceWindow1._instance.setMessagePage(m);
            }
        }
        

#endregion

        

        public void ShowButton()
        {
            if (IsphotoMenu)
            {
                //TransformationUtil.AppearControl(buttonLogin);
                buttonLogin.Visibility = System.Windows.Visibility.Visible;                
                buttonHomeCategory.Visibility = System.Windows.Visibility.Visible;                
                
                if (LoginControler.IsLogged() == false)
                {                    
                    TransformationUtil.DisAppearControl(buttonDisconnect);
                    
                    //_instance.buttonLogin.Visibility = Visibility.Visible;
                    //_instance.buttonAccesSelection.Visibility = Visibility.Hidden;
                    //_instance.buttonDisconnect.Visibility = Visibility.Hidden;
                }
                else
                {                    
                    if (IsphotoSelection)
                    {
                        //TransformationUtil.DisAppearControl(_instance.buttonAccesSelection);
                        //_instance.buttonAccesSelection.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        //TransformationUtil.AppearControl(_instance.buttonAccesSelection); 
                        //_instance.buttonAccesSelection.Visibility = Visibility.Visible;
                    }

                    TransformationUtil.AppearControl(buttonDisconnect);
                    //_instance.buttonDisconnect.Visibility = Visibility.Visible;
                }
            }
            else
            {
                TransformationUtil.AppearControl(buttonBackInfo);                
                //TransformationUtil.DisAppearControl(buttonLogin);
                buttonLogin.Visibility = System.Windows.Visibility.Hidden;
                System.Console.WriteLine(" button login Hide ");
                TransformationUtil.DisAppearControl(buttonDisconnect);                
                buttonHomeCategory.Visibility = System.Windows.Visibility.Hidden;
            }
        }
        

        private void buttonDisconnect_Click(object sender, RoutedEventArgs e)
        {
            LoginControler.Disconnect();
            GotoMenu();
        }

        private void viewbox4_TouchEnter(object sender, TouchEventArgs e)
        {
            Viewbox vb = (Viewbox)sender;
            var eog = new EaseObjectGroup();
            var eo = ArtefactAnimator.AddEase(vb, new[] { AnimationTypes.Width, AnimationTypes.Height}, new object[] { 800,800}, 0.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);
        }

        private void viewbox4_TouchLeave(object sender, TouchEventArgs e)
        {
            Viewbox vb = (Viewbox)sender;

            var eog = new EaseObjectGroup();            
            var eo = ArtefactAnimator.AddEase(vb, new[] { AnimationTypes.Width, AnimationTypes.Height }, new object[] { 600, 600 }, 0.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);
        }

        private void image1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_timerClose == null)
            {
                _timerClose = new DispatcherTimer();
                // Set the Interval to 2 seconds
                _timerClose.Interval = TimeSpan.FromSeconds(20);

                _timerClose.Tick += new EventHandler(delegate(object s, EventArgs a)
                {
                    nbClickForClosingApplication = 0;
                    _timerClose.Stop();
                    _timerClose = null;                
                });

                _timerClose.Start();
            }
            nbClickForClosingApplication++;
            if (nbClickForClosingApplication == 8)
            {
                Application.Current.Shutdown();
            }
        }

        private void languageButton_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            IsphotoMenu = false;
            RemoveLastPage();
            conteneur.Children.Add(languagePage);
            animationPageIn();
            ShowButton();
        }

        private void buttonHomeCategory_Click(object sender, RoutedEventArgs e)
        {
            GotoMenuCategory();
        }


        private void effectLanguageButton(Image elm)
        {
            elm.Visibility = System.Windows.Visibility.Hidden;
            TransformationUtil.AppearControl(elm);           
        }

        private void setMessagePage(String message)
        {
            MessagePage.Text = message;
        }      
       

        private void imageFrench_MouseUp(object sender, MouseButtonEventArgs e)
        {
            effectLanguageButton(imageFrench);
            LocalizationManager.UICulture = new CultureInfo("fr-FR");
            globalDatasingleton.getInstance().language = globalDatasingleton.LANGUAGETYPE.FRENCH;
            ChangeButtonText();
        }

        private void imageFrench_TouchUp(object sender, TouchEventArgs e)
        {
            effectLanguageButton(imageFrench);
            LocalizationManager.UICulture = new CultureInfo("fr-FR");
            globalDatasingleton.getInstance().language = globalDatasingleton.LANGUAGETYPE.FRENCH;
            ChangeButtonText();
        }

        private void imageEnglish_TouchUp(object sender, TouchEventArgs e)
        {
            effectLanguageButton(imageEnglish);
            LocalizationManager.UICulture = new CultureInfo("en-US");
            globalDatasingleton.getInstance().language = globalDatasingleton.LANGUAGETYPE.ENGLISH;
            ChangeButtonText();            
        }

        private void imageItalian_TouchUp(object sender, TouchEventArgs e)
        {
            effectLanguageButton(imageItalian);
            LocalizationManager.UICulture = new CultureInfo("it-IT");
            globalDatasingleton.getInstance().language = globalDatasingleton.LANGUAGETYPE.ITALIAN;
            ChangeButtonText();            
        }

        private void imageDeutch_TouchUp(object sender, TouchEventArgs e)
        {
            effectLanguageButton(imageDeutch);
            LocalizationManager.UICulture = new CultureInfo("de-DE");
             globalDatasingleton.getInstance().language = globalDatasingleton.LANGUAGETYPE.DEUTCH;
             ChangeButtonText();            
            //EnterInPhotoScreen();
        }

        private void imageSpanish_TouchUp(object sender, TouchEventArgs e)
        {
            //effectLanguageButton(imageItalian);
            //LocalizationManager.UICulture = new CultureInfo("es");
            //globalDatasingleton.getInstance().language = globalDatasingleton.LANGUAGETYPE.SPANISH;
            //ChangeButtonText();
        }

        private void imageEnglish_MouseUp(object sender, MouseButtonEventArgs e)
        {
            effectLanguageButton(imageEnglish);
            LocalizationManager.UICulture = new CultureInfo("en-US");
            globalDatasingleton.getInstance().language = globalDatasingleton.LANGUAGETYPE.ENGLISH;
            ChangeButtonText();            
        }

        private void imageDeutch_MouseUp(object sender, MouseButtonEventArgs e)
        {
            effectLanguageButton(imageDeutch);
            LocalizationManager.UICulture = new CultureInfo("de-DE");
            globalDatasingleton.getInstance().language = globalDatasingleton.LANGUAGETYPE.DEUTCH;
            ChangeButtonText();            
        }

        private void imageItalian_MouseUp(object sender, MouseButtonEventArgs e)
        {
            effectLanguageButton(imageItalian);
            LocalizationManager.UICulture = new CultureInfo("it-IT");
            globalDatasingleton.getInstance().language = globalDatasingleton.LANGUAGETYPE.ITALIAN;
            ChangeButtonText();        
        }

        private void imageSpanish_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void image2_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}