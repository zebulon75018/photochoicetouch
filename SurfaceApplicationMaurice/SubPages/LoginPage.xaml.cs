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

namespace SurfaceApplicationMaurice.SubPages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : UserControl
    {
        public LoginPage()
        {
            InitializeComponent();

            Utility.gradientManager gm = new gradientManager();
            gm.setgradient(gridlogin);            
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            SurfaceWindow1.StaticHideLoginPage();
            LoginControler.WantToAddImageAfterLogin = false;

            // 
            LoginControler.wantToAccessToSelection = false;

            SurfaceWindow1.stopModal();
            /*var eog = new EaseObjectGroup();
            //eog.Complete += g => Debug.WriteLine("COMPLETE");

            var eo = ArtefactAnimator.AddEase(this, new[] { AnimationTypes.Alpha, AnimationTypes.MarginTop }, new object[] { 0, -800 }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);          
             */
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            SurfaceWindow1.StaticShowCreationLoginPage();
        }

        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            if (LoginControler.Login(textBoxLogin.Text, passwordBox1.Password))
            {
                SurfaceWindow1.StaticHideLoginPage();
                textBlockErrorLogin.Visibility = System.Windows.Visibility.Hidden;
                globalDatasingleton.getInstance().SetLogged();

                if (LoginControler.wantToAccessToSelection == true)
                {
                    SurfaceWindow1.StaticGotoSelection();
                }                
            }
            else
            {
                textBlockErrorLogin.Visibility= System.Windows.Visibility.Visible;
                textBlockErrorLogin.Opacity = 0;
                var eog = new EaseObjectGroup();
                //eog.Complete += g => Debug.WriteLine("COMPLETE");
                var eo = ArtefactAnimator.AddEase(textBlockErrorLogin, new[] { AnimationTypes.Alpha}, new object[] { 1}, 1.5, AnimationTransitions.CubicEaseOut, 0);
                eog.AddEaseObject(eo);                
            }
            
        }  

        private void passwordBox1_GotFocus(object sender, RoutedEventArgs e)
        {
          //  numkeyboard.SetTextEntry((Control)sender);         
        }

        private void textBox1_GotFocus(object sender, RoutedEventArgs e)
        {
           // numkeyboard.SetTextEntry((Control)sender); 
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textBox1_TouchUp(object sender, TouchEventArgs e)
        {
            numkeyboard.SetTextEntry((Control)sender);         
        }

        private void passwordBox1_TouchUp(object sender, TouchEventArgs e)
        {
            numkeyboard.SetTextEntry((Control)sender);         
        }
    }
}
   