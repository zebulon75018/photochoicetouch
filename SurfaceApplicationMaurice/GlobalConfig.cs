using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows;

namespace SurfaceApplicationMaurice
{
    public class GlobalConfig
    {
        static GlobalConfig _instance = null;
        XmlDocument doc = null;
        public static bool debug = false;
        public GlobalConfig()
        {            
            try
            {
                doc = new XmlDocument();
                doc.Load("config.xml");

                XmlNode node = doc.LastChild.Attributes.GetNamedItem("debug");
                if (node == null)
                {
                    GlobalConfig.debug = false;
                }
                else
                {
                    if (node.Value.ToString().ToUpper() == "TRUE")
                    {
                        MessageBox.Show("DEBUG ON ");
                        GlobalConfig.debug = true;
                    }
                }            
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);                
            }
        }

        public int getNbCase
        {
            get
            {
                try { return Int32.Parse(doc.LastChild.Attributes["NbCase"].Value); }
                catch (Exception e) { return 6; }
            }
        }

        public int sizeViewBox
        {
            get
            {
                try
                {
                    return Int32.Parse(doc.LastChild.Attributes["sizeViewBox"].Value);
                }
                catch (Exception e) { return 300; }
            }
        }

        public string communConfig
        {
            get
            {
                try
                {
                    return doc.LastChild.Attributes["communConfig"].Value;
                }
                catch (Exception e)
                {
                    if (GlobalConfig.debug) MessageBox.Show(" Error communConfig " + e.Message);
                    return "";
                }
            }
        }

        static public GlobalConfig getInstance()
        {
            if (_instance == null)
            {
                _instance = new GlobalConfig();
            }

            return _instance;
        }
    }
}
