using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Controls;
using System.Windows.Forms;

namespace SurfaceApplicationMaurice
{
    public class globalDatasingleton
    {
        private static globalDatasingleton _instance = null;
        List<Viewbox> ListVB = null;
        List<String> filenames = null;
        int cursor = -1;
        bool logged = false;
        CategoriesXml categories = null;
        CategoriesXml infocategories = null;
        private bool m_bDirty;

        public enum LANGUAGETYPE
        {
            ENGLISH,
            FRENCH,
            DEUTCH,
            ITALIAN,
            SPANISH
        };

        public enum TypeOfSelection
        {
            BYCATEGORY,
            BYDATE
        };

        FileSystemWatcher fsWatcher;
        FileSystemWatcher fsWatcherInfo;
        public LANGUAGETYPE language = LANGUAGETYPE.FRENCH;

        public globalDatasingleton()
        {
            m_bDirty = false;
            ListVB = new List<Viewbox>();
            filenames = new List<String>();
          
        }

        public bool IsLogged
        {
            get { return logged; }
        }

        public void SetLogged()
        {
            logged = true;
        }
        

        private void ReadXmlFile()
        {
        }

        public String getPath()
        {
            return "";
        }

        public bool IsLoged()
        {
            return logged;
        }

        public string getLanguageAttribut()
        {        
         switch(  language)
         {
             case LANGUAGETYPE.ENGLISH:
                 return "englishtext";
             case LANGUAGETYPE.FRENCH:
                 return "frenchtext";
             case LANGUAGETYPE.DEUTCH:
                 return "deutchtext";
             case LANGUAGETYPE.ITALIAN:
                 return "italiantext";
             case LANGUAGETYPE.SPANISH:
                 return "spanishtext";
             default:
                 return "name"; 
         }
        }

        public void AddViewBox(ref Viewbox f,string filename)
        {
            ListVB.Add(f);
            filenames.Add(filename.Replace("\\miniatures", ""));
        }

        public void InitCursor(Viewbox f)
        {
            try
            {
                cursor = ListVB.FindIndex(delegate(Viewbox vb) { return (vb == f); });
            }
            catch (Exception e)
            {
                MessageBox.Show("Error InitCursor " + e.Message);
            }
        }

        public string getCursorFilename()
        {
            if (cursor < filenames.Count && cursor >= 0)
            {
                return filenames[cursor];
            }
            return "";
        }

        public string getNext()
        {
            if (cursor < (filenames.Count - 1) && cursor > -1)
            {
                cursor++;
                return (filenames[cursor]);
                //return(ListVB[cursor]); 
            }
            else
            {
                if (filenames.Count > 0)
                {
                    cursor = 0;
                    return (filenames[cursor]);
                }

            }
            return null;
        }
        public string getPrevious()
        {
            if (cursor >0 && cursor< filenames.Count )
            {
                cursor--;
                return (filenames[cursor]);
                //return (ListVB[cursor]);
            }
            else
            {
                if (filenames.Count == 0) return null;
                cursor = filenames.Count-1;
                return (filenames[cursor]);
            }
            return null;
        }

        void OnChanged(object sender, FileSystemEventArgs e)
        {
           if (!m_bDirty)
           {
            //MessageBox.Show(" Change " + e.Name);
            categories = null;
            //m_bDirty = true;
            SurfaceWindow1.CategoryChanged();
          }
        }

        void OnChangedInfo(object sender, FileSystemEventArgs e)
        {
           if (!m_bDirty)
           {
           // MessageBox.Show(" Change " + e.Name);
            infocategories = null;
            //m_bDirty = true;
            SurfaceWindow1.InfoChanged();
          }
        }        

        public CategoriesXml getCategories()
        {
            if (categories == null)
            {
                FileInfo fi = new FileInfo(CommunConfig.getInstance().fileCat);
                if (fsWatcher == null)
                {
                    fsWatcher = new FileSystemWatcher();
                    
                    fsWatcher.Path = fi.DirectoryName;
                    fsWatcher.Filter = fi.Name;

                    fsWatcher.NotifyFilter = NotifyFilters.LastWrite;
                    fsWatcher.Changed += new FileSystemEventHandler(OnChanged);                    
                    fsWatcher.EnableRaisingEvents = true;                                     
                }
                categories = new CategoriesXml(fi.FullName);
                //categories = new CategoriesXml(@"c:\maurice\category.xml");
            }
            return categories;
        }
        public CategoriesXml getInfoCategories()
        {
            if (infocategories == null)
            {
                FileInfo fi = new FileInfo(CommunConfig.getInstance().fileInfo);
                if (fsWatcherInfo == null)
                {
                    fsWatcherInfo = new FileSystemWatcher();
                    fsWatcherInfo.Path = fi.DirectoryName;
                    fsWatcherInfo.Filter = fi.Name;

                    fsWatcherInfo.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                      | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                    fsWatcherInfo.Changed += new FileSystemEventHandler(OnChangedInfo);
                    fsWatcherInfo.EnableRaisingEvents = true;
                    fsWatcherInfo.Changed += new FileSystemEventHandler(OnChangedInfo);
                }
                infocategories = new CategoriesXml(fi.FullName);
                //categories = new CategoriesXml(@"c:\maurice\category.xml");
            }
            return infocategories;
        }

        static public globalDatasingleton getInstance()
        {
            if (globalDatasingleton._instance == null) globalDatasingleton._instance = new globalDatasingleton();

            return _instance;
        }
    }
}
