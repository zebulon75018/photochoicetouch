using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace SurfaceApplicationMaurice
{
    public class CategoriesXml
    {
        XmlDocument categories = null;      

        public CategoriesXml(string filename)
        {                            
            categories = new XmlDocument();
            try
            {
                if (File.Exists(filename))
                {
                    categories.Load(filename);
                }
                else
                {
                    System.Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
                    System.Console.WriteLine(System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName));
                    categories.LoadXml("<categories><category name=\"Error File not exist "+filename+"\" /></categories>");
                }
            }
            catch (Exception e)
            {
                categories.LoadXml("<categories><category name=\"Error Loading "+e.Message+"\" /></categories>");
            }
        }

        public XmlElement getRootNode()
        {
            return (XmlElement) categories.ChildNodes[0];
        }
        public XmlElement getCurrentElem(int c)
        {
            int n = 0;
            foreach (XmlElement elm in categories.ChildNodes[0])
            {
                if (elm.Name == "category")
                {
                    if (n == c) return elm;
                    n++;
                }
            }
            return null;
        }

        public XmlElement getCurrentChildElem(ref XmlElement e,int c)
        {
            int n = 0;
            if (e == null) return null;
            foreach (XmlElement elm in e.ChildNodes)
            {
                if (elm.Name == "category")
                {
                    if (n == c) return elm;
                    n++;
                }
            }
            return null;
        }

        public List<string> getListDirectory()
        {
            List<String> result = new List<string>();
            foreach (XmlElement n in categories.FirstChild.ChildNodes)
            {
                try
                {
                    result.Add(n.GetAttribute("directory"));
                }
                catch (Exception e) { }
            }
            return result;
        }

        public string getCurrentElemDirectory(ref XmlElement current)
        {
            if (current != null)
            {
                return current.GetAttribute("directory");
            }
            return "";
        }

        public string GetNameParent(ref XmlElement elm)
        {
            if (elm.ParentNode != null)
            {
                return getLanguageName((XmlElement)elm.ParentNode);
            }
            return "";
        }

        public string getLanguageName(XmlElement e)
        {
            if (e == null) return "";
            string attr = globalDatasingleton.getInstance().getLanguageAttribut();
            if (e.GetAttribute(attr) == "")
            {
                attr = "name";
            }
            return e.GetAttribute(attr);
        }

        public bool IsParentRootNode(XmlElement elm)
        {
            if (elm == categories.ChildNodes[0]) return true;
            foreach (XmlElement e in categories.ChildNodes[0])
            {
                if (e == elm) return true;
            }

            return false;
        }

        public List<String> getListChildCurrent(ref XmlElement current)
        {
            List<String> result = new List<string>();
            foreach (XmlElement elm in current)
            {
                if (elm.Name == "category")
                {
                    result.Add(getLanguageName(elm));
                }
            }
            return result;
        }

        public List<String> GetNameCategories()
        {
            List<String> result = new List<string>();
            foreach (XmlElement elm in categories.ChildNodes[0])
            {
                if (elm.Name == "category")
                {
                    string attr = globalDatasingleton.getInstance().getLanguageAttribut();
                    if (elm.GetAttribute(attr) == "")
                    {
                        attr = "name";
                    }                    
                    result.Add(elm.GetAttribute(attr));
                }
            }
            return result;
        }

        public List<String> GetBitmapsCategories()
        {
            List<String> result = new List<string>();
            foreach (XmlElement elm in categories.ChildNodes[0])
            {
                if (elm.Name == "category")
                {
                    result.Add(elm.GetAttribute("bitmap"));
                }
            }
            return result;
        }

        public List<String> GetNameChildCategories(ref XmlElement cur)
        {
            List<String> result = new List<string>();
            if (cur == null) return result;

            foreach (XmlElement elm in cur.ChildNodes)
            {
                if (elm.Name == "category")
                {
                    string attr = globalDatasingleton.getInstance().getLanguageAttribut();
                    if (elm.GetAttribute(attr) == "")
                    {
                        attr = "name";
                    }
                    result.Add(elm.GetAttribute(attr));
                }
            }
            return result;
        }

        public List<String> GetBitmapsChildCategories(ref XmlElement cur)
        {
            List<String> result = new List<string>();
            if (cur == null) return result;

            foreach (XmlElement elm in cur.ChildNodes)            
            {
                if (elm.Name == "category")
                {
                    result.Add(elm.GetAttribute("bitmap"));
                }
            }
            return result;
        }


        public XmlElement GetChild(ref XmlElement cur, int index)
        {
            int n = 0;
            foreach (XmlElement elm in cur.ChildNodes)
            {
                if (elm.Name == "category")
                {
                    if (n == index)
                    {
                        return elm;
                    }
                    n++;
                }
            }
            return null;
        }
    }
}
