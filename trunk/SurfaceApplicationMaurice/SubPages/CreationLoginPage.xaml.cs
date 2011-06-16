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
    /// Interaction logic for CreationLoginPage.xaml
    /// </summary>
    public partial class CreationLoginPage : UserControl
    {
        public CreationLoginPage()
        {
            InitializeComponent();
            UpdateLanguageDateSelection();

            Utility.gradientManager gm = new gradientManager();            
            gm.setgradient(gridBackground);            
        }

        private void textBox2_GotFocus(object sender, RoutedEventArgs e)
        {
            numkeyboard.SetTextEntry((Control)sender);    
        }

        private void textBox1_GotFocus(object sender, RoutedEventArgs e)
        {
            numkeyboard.SetTextEntry((Control)sender);    
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            SurfaceWindow1.StaticHideCreationLoginPage();
            LoginControler.WantToAddImageAfterLogin = false;
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxNumberChamber.Text.Length == 0 || textBoxPassword.Text.Length == 0 || textBoxName.Text.Length == 0)
            {
                TransformationUtil.ShowAndDisappearText(textBlockFillEntry);
                return;
            }
            DateTime d = dateSelector.SelectedDate;
            string dateString = d.Day.ToString() + "-"+d.Month.ToString() + "-" + d.Year.ToString();
            if (LoginControler.createLogin(textBoxNumberChamber.Text, textBoxPassword.Text, dateString, textBoxName.Text,textBoxFirstName.Text))
            {
              textBlockErrorLogin.Visibility = System.Windows.Visibility.Hidden;
              SurfaceWindow1.StaticHideCreationLoginPage();
              LoginControler.Login(textBoxNumberChamber.Text, textBoxPassword.Text);

            }
            else
            {
                TransformationUtil.ShowAndDisappearText(textBlockErrorLogin);
                //textBlockErrorLogin.Visibility= System.Windows.Visibility.Visible;
                //textBlockErrorLogin.Opacity = 0;
                //var eog = new EaseObjectGroup();
                //eog.Complete += g => Debug.WriteLine("COMPLETE");
                //var eo = ArtefactAnimator.AddEase(textBlockErrorLogin, new[] { AnimationTypes.Alpha}, new object[] { 1}, 1.5, AnimationTransitions.CubicEaseOut, 0);
                //eog.AddEaseObject(eo);                          
            }
        }

        public void UpdateLanguageDateSelection()
        {                                    
                     
            switch (globalDatasingleton.getInstance().language)
            {
                case globalDatasingleton.LANGUAGETYPE.ENGLISH: dateSelector.LocaleName = "en-US"; break;
                case globalDatasingleton.LANGUAGETYPE.FRENCH: dateSelector.LocaleName = "fr-FR"; break;
                case globalDatasingleton.LANGUAGETYPE.DEUTCH: dateSelector.LocaleName = "de-DE"; break;
                case globalDatasingleton.LANGUAGETYPE.ITALIAN: dateSelector.LocaleName = "it-IT"; break;
                case globalDatasingleton.LANGUAGETYPE.SPANISH: dateSelector.LocaleName = "es-ES"; break;
            }

            dateSelector.MinimumYear = DateTime.Today.Year;
            dateSelector.MaximumYear = DateTime.Today.Year + 1;
            dateSelector.SelectedDate = DateTime.Today;

            return;
        }
    }
}
