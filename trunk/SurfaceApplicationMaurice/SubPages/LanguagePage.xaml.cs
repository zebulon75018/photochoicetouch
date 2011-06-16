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
using System.Globalization;
using WPFLocalization;

namespace SurfaceApplicationMaurice.SubPages
{
    /// <summary>
    /// Interaction logic for LanguagePage.xaml
    /// </summary>
    public partial class LanguagePage : UserControl
    {
        public LanguagePage()
        {
            InitializeComponent();
            
            EnglishButton.ImageSource = "pack://application:,,,/Resources/anglais.gif";
            EnglishButton.Label = "";
            EnglishButton.onlyBitmapHeight(230);
            FrenchButton.ImageSource = "pack://application:,,,/Resources/francais.jpg";
            FrenchButton.Label = "";
            FrenchButton.onlyBitmapHeight(230);
            ItalianButton.ImageSource = "pack://application:,,,/Resources/Italian.jpg";
            ItalianButton.Label = "";
            ItalianButton.onlyBitmapHeight(230);
            DeutchButton.ImageSource = "pack://application:,,,/Resources/allemand.jpg";
            DeutchButton.Label = "";
            DeutchButton.onlyBitmapHeight(230);
            SpanishButton.ImageSource = "pack://application:,,,/Resources/espagnol.gif";
            SpanishButton.Label = "";
            SpanishButton.onlyBitmapHeight(230);
           
          //  Utility.gradientManager gm = new Utility.gradientManager();
          //  gm.setgradient(gridBackground);            
        }

        private void closePage()
        {
            SurfaceWindow1.changeLanguage();
            SurfaceWindow1.closePage();
            SurfaceWindow1.UpdateLists();
            
        }

        private void EnglishButton_TouchUp(object sender, TouchEventArgs e)
        {
            LocalizationManager.UICulture = new CultureInfo("en-US");
            globalDatasingleton.getInstance().language = globalDatasingleton.LANGUAGETYPE.ENGLISH;
            closePage();
        }

        private void FrenchButton_TouchUp(object sender, TouchEventArgs e)
        {
            LocalizationManager.UICulture = new CultureInfo("fr-FR");
            globalDatasingleton.getInstance().language = globalDatasingleton.LANGUAGETYPE.FRENCH;
            closePage();
        }

        private void ItalianButton_TouchUp(object sender, TouchEventArgs e)
        {
            LocalizationManager.UICulture = new CultureInfo("it-IT");
            globalDatasingleton.getInstance().language = globalDatasingleton.LANGUAGETYPE.ITALIAN;
            closePage();
        }

        private void DeutchButton_TouchUp(object sender, TouchEventArgs e)
        {
        //    globalDatasingleton.getInstance().language = globalDatasingleton.LANGUAGETYPE.DEUTCH;
            closePage();
        }

        private void SpanishButton_TouchUp(object sender, TouchEventArgs e)
        {
         //   globalDatasingleton.getInstance().language = globalDatasingleton.LANGUAGETYPE.SPANISH;
            closePage();
        }


        private void viewbox1_TouchEnter(object sender, TouchEventArgs e)
        {
            Viewbox vb = (Viewbox)sender;
            var eog = new EaseObjectGroup();
            var eo = ArtefactAnimator.AddEase(vb, new[] { AnimationTypes.Width, AnimationTypes.Height }, new object[] { 430 * 1.2, 425 * 1.2 }, 0.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);
        }

        private void viewbox1_TouchLeave(object sender, TouchEventArgs e)
        {
            Viewbox vb = (Viewbox)sender;

            var eog = new EaseObjectGroup();
            var eo = ArtefactAnimator.AddEase(vb, new[] { AnimationTypes.Width, AnimationTypes.Height }, new object[] { 430, 425 }, 0.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);

        }
    }
}
