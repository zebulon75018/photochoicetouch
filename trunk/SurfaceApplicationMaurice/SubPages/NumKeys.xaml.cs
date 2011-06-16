using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Artefact.Animation;

namespace SurfaceApplicationMaurice.SubPages
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class NumKeys : UserControl
    {
        private Control textentry=null;
        DispatcherTimer _timer = null;
        bool activity = false;
        public NumKeys()
        {
            InitializeComponent();
        }

        public void SetTextEntry(Control text)
        {
            textentry = text;
            ShowKeyboard();
        }

        private void ShowKeyboard()
        {
            Margin = new Thickness(textentry.Margin.Left + textentry.Width, textentry.Margin.Top, -300, -400);
            Opacity = 0;
            Visibility = System.Windows.Visibility.Visible;

            var eog = new EaseObjectGroup();
            var eo = ArtefactAnimator.AddEase(this, new[] { AnimationTypes.Alpha }, new object[] { 1 }, 0.5, AnimationTransitions.CubicEaseIn, 0);
            eog.AddEaseObject(eo);

            _timer = new DispatcherTimer();

            // Set the Interval to 2 seconds
            _timer.Interval = TimeSpan.FromSeconds(10);
            activity=false;
              _timer.Tick += new EventHandler(delegate(object s, EventArgs a)
            {
                if (activity== false)
                {                    
                    HideKeyboard();
                }
                activity=false;
            });

              _timer.Start();
        }

        private void HideKeyboard()
        {
            if (_timer!=null) _timer.Stop();

            //numkeyboard.Opacity = 0;
            this.Visibility = System.Windows.Visibility.Visible;

            var eog = new EaseObjectGroup();
            var eo = ArtefactAnimator.AddEase(this, new[] { AnimationTypes.Alpha }, new object[] { 0 }, 0.5, AnimationTransitions.CubicEaseIn, 0);
            eog.AddEaseObject(eo);
            eog.Complete += delegate(EaseObjectGroup easeObjectGroup) { this.Visibility = System.Windows.Visibility.Hidden; };
        }
        private void AddNumber(string n)
        {
            activity = true;
            if (textentry.GetType().Name == "PasswordBox")
            {
                    ((PasswordBox)textentry).Password += n;
            }
            else
            {
                if (textentry.GetType().Name == "TextBox")
                {
                    ((TextBox)textentry).AppendText(n);
                }
            }
        }

        private void DeleteChar()
        {
            activity = true;
            if (textentry.GetType().Name == "PasswordBox")
            {
                string pass = ((PasswordBox)textentry).Password;
                if (pass.Length > 0)
                {
                    ((PasswordBox)textentry).Password = pass.Remove(pass.Length - 1);
                }
            }
            else
            {
                if (textentry.GetType().Name == "TextBox")
                {
                    int indexC = ((TextBox)textentry).CaretIndex;
                    string text = ((TextBox)textentry).Text;
                    if (text.Length == 0) return;
                    ((TextBox)textentry).Text = text.Remove(text.Length-1);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddNumber("1");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            HideKeyboard();
        }

        private void button_2_Click(object sender, RoutedEventArgs e)
        {
            AddNumber("2");
        }

        private void button_3_Click(object sender, RoutedEventArgs e)
        {
            AddNumber("3");
        }

        private void button_4_Click(object sender, RoutedEventArgs e)
        {
            AddNumber("4");
        }

        private void button_5_Click(object sender, RoutedEventArgs e)
        {
            AddNumber("5");
        }

        private void button_6_Click(object sender, RoutedEventArgs e)
        {
            AddNumber("6");
        }

        private void button_7_Click(object sender, RoutedEventArgs e)
        {
            AddNumber("7");
        }

        private void button_8_Click(object sender, RoutedEventArgs e)
        {
            AddNumber("8");
        }

        private void button_9_Click(object sender, RoutedEventArgs e)
        {
            AddNumber("9");
        }

        private void button_0_Click(object sender, RoutedEventArgs e)
        {
            AddNumber("0");
        }

        private void button_delete_Click(object sender, RoutedEventArgs e)
        {
            DeleteChar();
        }
    }
}
