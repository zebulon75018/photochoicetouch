using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.IO;

namespace SurfaceApplicationMaurice
{
    public class UserImgSelected
    {
        public  string path="";
        private string origin_path="";
        private Image img;

        public UserImgSelected(string p)
        {
            //img = i;
            
            //origin_path = originalPath;
            path = p;
        }

        public Image Img
        {
            get { return img; }
            set { img = value; }
        }

        public void DeleteImg()
        {
            if (File.Exists(path) == true)
            {
                File.Delete(path);
            }
        }

    }
}
