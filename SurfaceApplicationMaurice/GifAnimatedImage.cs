using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace SurfaceApplicationMaurice
{
    public class GifImage : Image
    {
        #region Memmbers

        private GifBitmapDecoder _gifDecoder;
        private Int32Animation _animation;
        private bool _isInitialized;

        #endregion Memmbers

        #region Properties

        private int FrameIndex
        {
            get { return (int)GetValue(FrameIndexProperty); }
            set { SetValue(FrameIndexProperty, value); }
        }

        private static readonly DependencyProperty FrameIndexProperty =
            DependencyProperty.Register("FrameIndex", typeof(int), typeof(GifImage), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(ChangingFrameIndex)));

        private static void ChangingFrameIndex(DependencyObject obj, DependencyPropertyChangedEventArgs ev)
        {
            GifImage image = obj as GifImage;
            image.Source = image._gifDecoder.Frames[(int)ev.NewValue];
        }

        /// &lt;summary>
        /// Defines whether the animation starts on it's own
        /// &lt;/summary>
        public bool AutoStart
        {
            get { return (bool)GetValue(AutoStartProperty); }
            set { SetValue(AutoStartProperty, value); }
        }

        public static readonly DependencyProperty AutoStartProperty =
            DependencyProperty.Register("AutoStart", typeof(bool), typeof(GifImage), new UIPropertyMetadata(false, AutoStartPropertyChanged));

        private static void AutoStartPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
                (sender as GifImage).StartAnimation();
        }

        public string GifSource
        {
            get { return (string)GetValue(GifSourceProperty); }
            set { SetValue(GifSourceProperty, value); }
        }

        public static readonly DependencyProperty GifSourceProperty =
            DependencyProperty.Register("GifSource", typeof(string), typeof(GifImage), new UIPropertyMetadata(string.Empty, GifSourcePropertyChanged));

        private static void GifSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // Reinitialize animation everytime image is changed
            (sender as GifImage).Initialize();
        }

        #endregion Properties

        #region Private Instance Methods

        private void Initialize()
        {
            /*
            // initialize the gifDecoder
            _gifDecoder = new GifBitmapDecoder(new Uri("pack://application:,,," + this.GifSource), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            // initialize the animation
            _animation = new Int32Animation(0, _gifDecoder.Frames.Count - 1, new Duration(new TimeSpan(0, 0, 0, _gifDecoder.Frames.Count / 10, (int)((_gifDecoder.Frames.Count / 10.0 - _gifDecoder.Frames.Count / 10) * 1000))));
            // repeat forever
            _animation.RepeatBehavior = RepeatBehavior.Forever;
            // set the image source
            this.Source = _gifDecoder.Frames[0];
            */
            // set this to true so it doesn't get initialized again
            _isInitialized = true;
        }

        public void setGifSource(string url)
        {
            // initialize the gifDecoder
            _gifDecoder = new GifBitmapDecoder(new Uri(url), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            // initialize the animation
            _animation = new Int32Animation(0, _gifDecoder.Frames.Count - 1, new Duration(new TimeSpan(0, 0, 0, _gifDecoder.Frames.Count / 10, (int)((_gifDecoder.Frames.Count / 10.0 - _gifDecoder.Frames.Count / 10) * 1000))));
            // repeat forever
            _animation.RepeatBehavior = RepeatBehavior.Forever;
            // set the image source
            this.Source = _gifDecoder.Frames[0];
            this.StartAnimation();
        }

        #endregion Private Instance Methods

        #region Public Instance Methods

        /// &lt;summary>
        /// Shows and starts the gif animation
        /// &lt;/summary>
        public void Show()
        {
            this.Visibility = Visibility.Visible;
            this.StartAnimation();
        }

        /// &lt;summary>
        /// Hides and stops the gif animation
        /// &lt;/summary>
        public void Hide()
        {
            this.Visibility = Visibility.Collapsed;
            this.StopAnimation();
        }

        /// &lt;summary>
        /// Starts the animation
        /// &lt;/summary>
        public void StartAnimation()
        {
            if (!_isInitialized)
                this.Initialize();

            BeginAnimation(FrameIndexProperty, _animation);
        }

        /// &lt;summary>
        /// Stops the animation
        /// &lt;/summary>
        public void StopAnimation()
        {
            BeginAnimation(FrameIndexProperty, null);
        }

        #endregion Public Instance Methods
    }
}
