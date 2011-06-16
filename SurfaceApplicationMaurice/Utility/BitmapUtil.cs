using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SurfaceApplicationMaurice.Utility
{
    static class BitmapUtil
    {

        static public ImageSource GetImageSource(string fileName)
        {
            if (File.Exists(fileName) == false) return null;
            BitmapImage bitmap = new BitmapImage();
            try
            {
                byte[] buffer = File.ReadAllBytes(fileName);
                MemoryStream memoryStream = new MemoryStream(buffer);

                bitmap.BeginInit();
                bitmap.StreamSource = memoryStream;
                bitmap.EndInit();
                bitmap.Freeze();
            }
            catch (Exception e)
            {
            }

            return bitmap;
        }
    }
}
