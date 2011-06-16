using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using SurfaceApplicationMaurice.Utility;
using System.Windows.Forms;

namespace SurfaceApplicationMaurice
{
    public class SelectionUser
    {
        public List<UserImgSelected> userSelectionImage = null;

        static SelectionUser _instance;

        static public MyImage _ImageSelected = null;

        public SelectionUser()
        {
            userSelectionImage = new List<UserImgSelected>();
            FillListImage();
        }

        private void FillListImage()
        {
            LoginManager lm = LoginControler.getLoginManager();
            List<String> lstImage = lm.listImageUserSelection();
            foreach (String s in lstImage)
            {                
                userSelectionImage.Add(new UserImgSelected(s));                
            }
            

            /*String path = LoginControler.getUserDirectory() + "*.jpg";
            foreach (string file in Directory.GetFiles(LoginControler.getUserDirectory(),"*.jpg"))
            {
                Image img = new Image();
                img.Source = BitmapUtil.GetImageSource(file);
                userSelectionImage.Add(new UserImgSelected(file));                
            } 
             */
        }

        public List<UserImgSelected> getImageSelectionMemory()
        {
            return userSelectionImage;
        }

        public List<UserImgSelected> getImageSelection()
        {
            userSelectionImage.Clear();
            FillListImage();
            return userSelectionImage;
        }

        /*
        public void AddImage(ImageSource imgsrc)
        {
            MyImage img = new MyImage();
            img.OriginalPath = imgsrc.ToString();
            img.Source = imgsrc;
            AddImage(img);            
        }
         * */

        private void saveImage(ref MyImage img,string filename,System.IO.FileMode fm)
        {
            try
            {
                FileStream stream5 = new FileStream(filename, fm);
                JpegBitmapEncoder encoder5 = new JpegBitmapEncoder();
                encoder5.QualityLevel = 100;
                encoder5.Frames.Add(BitmapFrame.Create((BitmapSource)img.Source));
                encoder5.Save(stream5);
                encoder5 = null;
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception saving Image " + filename + " " + e.Message);
            }           
            GC.Collect();
        }
      
        public void AddImage(MyImage img)
        {   

            Random rand = new Random();

            FileInfo fi = new FileInfo(img.OriginalPath);
            
            char [] separator = new char[1];
            separator[0]='_';

            string [] liststr = fi.Name.Split(separator);

            string path;
            if (liststr.Length == 2)
            {
                path = LoginControler.getUserDirectory() + liststr[0] + "_selection" + rand.Next().ToString() + ".jpg";
            }
            else
            {
                path = LoginControler.getUserDirectory() + "selection" + rand.Next().ToString() + ".jpg";
            }                
            
            saveImage(ref img, path, FileMode.Create);
            userSelectionImage.Add(new UserImgSelected(path));

            LoginManager m = LoginControler.getLoginManager();
            if (m != null)
            {
                if (m.isImageAlreadyIn(img.OriginalPath) == false)
                {
                    m.AddImage(path, img.OriginalPath);
                }
            }
            else
            {
                MessageBox.Show("Error LoginManager NULL");
            }
        } 

        public void DeleteImage(String filepath)
        {
            LoginManager m = LoginControler.getLoginManager();
            if (m != null)
            {
                m.DeleteImage(filepath);
            }
            foreach (UserImgSelected u in userSelectionImage)
            {                                
                if (u.path== filepath)
                {
                    u.Img = null;                    
                    userSelectionImage.Remove(u);
                    u.DeleteImg();                   
                    return;
                }
            }
        }

        public XmlNode getXmlUserNode(string filename)
        {
            LoginManager lm = LoginControler.getLoginManager();
            return lm.getNodeImageUser(filename);
        }
        

        static public bool Login(string chamberNumber)
        {
            return false;
        }

        static public SelectionUser getInstance()
        {
            if (_instance == null) _instance = new SelectionUser();

            return _instance;
        }
    }
}
