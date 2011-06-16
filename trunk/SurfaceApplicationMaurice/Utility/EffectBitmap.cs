using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace SurfaceApplicationMaurice.Utility
{
    static public class EffectBitmap
    {

        #region Image-Bitmap Interop Helpers.

        public static ImageSource convertBitmapToBitmapSource(System.Drawing.Bitmap bitmap)
        {
            using (bitmap)
            {
                System.Windows.Media.Imaging.BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    bitmap.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                return bitmapSource;
            }
        }

        public static System.Drawing.Bitmap GetBitmap(System.Windows.Controls.Image image)
        {
            if (image.Source == null) return null;
            //System.Windows.Forms.PictureBox picture = _pictureBox;
            //Stream stm = File.Open("Waterfall.jpg", FileMode.Open, FileAccess.Read))
            //// Since we're not specifying a System.Windows.Media.Imaging.BitmapCacheOption, the pixel format
            //// will be System.Windows.Media.PixelFormats.Pbgra32.
            //System.Windows.Media.Imaging.BitmapSource bitmapSource = System.Windows.Media.Imaging.BitmapFrame.Create(
            //    stm, 
            //    System.Windows.Media.Imaging.BitmapCreateOptions.None, 
            //    System.Windows.Media.Imaging.BitmapCacheOption.OnLoad);

            // TODO .... ERREYR entre Control et BitmapSource.
            System.Windows.Media.Imaging.BitmapSource bitmapSource = image.Source as System.Windows.Media.Imaging.BitmapSource;

            // Scale the image so that it will display similarly to the WPF Image.
            double newWidthRatio = image.Source.Width / (double)bitmapSource.PixelWidth;
            double newHeightRatio = ((image.Source.Width * bitmapSource.PixelHeight) / (double)bitmapSource.PixelWidth) / (double)bitmapSource.PixelHeight;

            System.Windows.Media.Imaging.BitmapSource transformedBitmapSource = new System.Windows.Media.Imaging.TransformedBitmap(
                bitmapSource,
                new System.Windows.Media.ScaleTransform(newWidthRatio, newHeightRatio));

            int width = transformedBitmapSource.PixelWidth;
            int height = transformedBitmapSource.PixelHeight;
            int stride = width * ((transformedBitmapSource.Format.BitsPerPixel + 7) / 8);

            byte[] bits = new byte[height * stride];

            transformedBitmapSource.CopyPixels(bits, stride, 0);

            unsafe
            {
                fixed (byte* pBits = bits)
                {
                    IntPtr ptr = new IntPtr(pBits);

                    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(
                        width,
                        height,
                        stride,
                        System.Drawing.Imaging.PixelFormat.Format32bppPArgb,
                        ptr);

                    return bitmap;
                }
            }
        }

        #endregion Image-Bitmap Interop Helpers.


        public static System.Drawing.Bitmap ConvertSepiaTone(System.Drawing.Bitmap Image)
        {
            System.Drawing.Bitmap TempBitmap = Image;
            System.Drawing.Bitmap NewBitmap = new System.Drawing.Bitmap(TempBitmap.Width, TempBitmap.Height);
            System.Drawing.Graphics NewGraphics = System.Drawing.Graphics.FromImage(NewBitmap);
            float[][] FloatColorMatrix ={
                    new float[] {.393f, .349f, .272f, 0, 0},
                    new float[] {.769f, .686f, .534f, 0, 0},
                    new float[] {.189f, .168f, .131f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                };
            System.Drawing.Imaging.ColorMatrix NewColorMatrix = new System.Drawing.Imaging.ColorMatrix(FloatColorMatrix);
            System.Drawing.Imaging.ImageAttributes Attributes = new System.Drawing.Imaging.ImageAttributes();
            Attributes.SetColorMatrix(NewColorMatrix);
            NewGraphics.DrawImage(TempBitmap, new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), 0, 0, TempBitmap.Width, TempBitmap.Height, System.Drawing.GraphicsUnit.Pixel, Attributes);
            NewGraphics.Dispose();
            return NewBitmap;
        }

        public static System.Drawing.Bitmap ConvertBlackAndWhite(System.Drawing.Image TempImage)
        {
            System.Drawing.Imaging.ImageFormat ImageFormat = TempImage.RawFormat;
            System.Drawing.Bitmap TempBitmap = new System.Drawing.Bitmap(TempImage, TempImage.Width, TempImage.Height);

            System.Drawing.Bitmap NewBitmap = new System.Drawing.Bitmap(TempBitmap.Width, TempBitmap.Height);
            System.Drawing.Graphics NewGraphics = System.Drawing.Graphics.FromImage(NewBitmap);
            float[][] FloatColorMatrix ={
                    new float[] {.3f, .3f, .3f, 0, 0},
                    new float[] {.59f, .59f, .59f, 0, 0},
                    new float[] {.11f, .11f, .11f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                };

            System.Drawing.Imaging.ColorMatrix NewColorMatrix = new System.Drawing.Imaging.ColorMatrix(FloatColorMatrix);
            System.Drawing.Imaging.ImageAttributes Attributes = new System.Drawing.Imaging.ImageAttributes();
            Attributes.SetColorMatrix(NewColorMatrix);
            NewGraphics.DrawImage(TempBitmap, new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), 0, 0, TempBitmap.Width, TempBitmap.Height, System.Drawing.GraphicsUnit.Pixel, Attributes);
            NewGraphics.Dispose();
            return NewBitmap;
            //NewBitmap.Save(NewFileName, ImageFormat);
        }

    }
}
