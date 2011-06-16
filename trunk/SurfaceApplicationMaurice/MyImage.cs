using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
namespace SurfaceApplicationMaurice
{
    public class MyImage : Image 
    {
        string originalPath = "";
        public MyImage()
        {            
        }

        public string OriginalPath
        {
            set { originalPath = value; }
            get { return originalPath; }
        }
    }
}
