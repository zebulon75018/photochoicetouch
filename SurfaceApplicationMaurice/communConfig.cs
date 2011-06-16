using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;
using System.Windows;

namespace SurfaceApplicationMaurice
{
    public class CommunConfig
    {
        static CommunConfig _instance = null;
        XmlDocument doc = null;
        XmlDocument docGuiConfig = null;
        public CommunConfig(string filename)
        {
            if (GlobalConfig.debug) MessageBox.Show("Loading " + filename);
            try
            {
                doc = new XmlDocument();
                doc.Load(filename);

                if (GlobalConfig.debug) MessageBox.Show("Loading " + filename + " with Sucess");
            }
            catch (Exception e)
            {
                MessageBox.Show("Error Loading " + e.Message);
            }

            
        }

        private XmlDocument getguiConfig()
        {
            if (docGuiConfig == null)
            {
                try
                {
                    docGuiConfig = new XmlDocument();                    
                    XmlNode n = doc.LastChild;

                    XmlNode node = doc.LastChild.Attributes.GetNamedItem("guiconfigfile");
                    if (node == null)
                    {
                        MessageBox.Show("getguiConfig Error ");
                    }

                    XmlAttribute a = doc.LastChild.Attributes["guiconfigfile"];

                    if ( GlobalConfig.debug) MessageBox.Show(a.Value);
                    docGuiConfig.Load(a.Value);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error Loading " + e.Message);
                }
            }

            return docGuiConfig;
        }

        public string fileCat
        {
            get
            {
                XmlNode node = doc.LastChild.Attributes.GetNamedItem("categoryfile");
                if (node == null) MessageBox.Show("Error Find categoryfile");
                return doc.LastChild.Attributes["categoryfile"].Value;
            }
        }

       public string fileInfo
        {
            get
            {
                XmlNode node = doc.LastChild.Attributes.GetNamedItem("infofile");
                if (node == null) MessageBox.Show("Error Find infofile");
                return doc.LastChild.Attributes["infofile"].Value;
            }
        }

       public string userDirectory
        {
            get
            {
                XmlNode node = doc.LastChild.Attributes.GetNamedItem("userdirectory");
                if (node == null) MessageBox.Show("Error Find userdirectory");
                return doc.LastChild.Attributes["userdirectory"].Value;
            }
        }

       public string guiConfig
       {
           get
           {
               XmlNode node = doc.LastChild.Attributes.GetNamedItem("guiconfigfile");
               if (node == null) MessageBox.Show("Error Find guiconfigfile");
               return doc.LastChild.Attributes["guiconfigfile"].Value;
           }
       }

    public string InfoGuiImage
    {
        get
        {
            XmlNode node = getguiConfig().LastChild.Attributes.GetNamedItem("imageinfo");
            if (node == null) return "";
            return node.Value;
        }
    }

    public string PhotoGuiImage
    {
        get
        {
            XmlNode node = getguiConfig().LastChild.Attributes.GetNamedItem("imagephoto");
            if (node == null) return "";
            return node.Value;
        }
    }

    public string ProductFile
    {
        get
        {           
            XmlNode productFile = doc.LastChild.Attributes.GetNamedItem("productfile");
            if (productFile != null)
            {
                return productFile.Value;
            }
            else
            {
                MessageBox.Show("productFile Error ");
            }
            return "";
        }
    }


    public string LanguageImage
    {
        get
        {
            return getguiConfig().LastChild.Attributes["imagelangue"].Value;
        }
    }

    public Color StartColor
    {
        get
        {
            return System.Drawing.ColorTranslator.FromHtml(getguiConfig().LastChild.Attributes["colorstart"].Value);            
        }
    }

    public Color EndColor
    {
        get
        {
            return System.Drawing.ColorTranslator.FromHtml(getguiConfig().LastChild.Attributes["colorend"].Value);                        
        }
    }

    public int Angle
    {
        get
        {
            return Int32.Parse(getguiConfig().LastChild.Attributes["angle"].Value);
        }
    }


    public bool ImageRotate
    {
        get
        {
            XmlNode node = getguiConfig().LastChild.Attributes.GetNamedItem("rotateimage");
            if (node == null) return false;
            if (node.Value =="true")
            {
                return true; 
            }
            return false;
        }
    }
        



        static public CommunConfig getInstance()
        {
            if (_instance == null)
            {
                _instance = new CommunConfig(GlobalConfig.getInstance().communConfig);
            }

            return _instance;
        }
    }
}
