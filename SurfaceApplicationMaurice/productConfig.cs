using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace SurfaceApplicationMaurice
{
    public class productConfig
    {
        static productConfig _instance = null;
        protected XmlNode listProduct;
        protected XmlNode listPromotion;
        XmlDocument doc = null;          

        productConfig(String filename)
        {                          
            try
            {
                doc = new XmlDocument();
                doc.Load(filename);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error Loading " + e.Message);
            }
            XmlNode  root = doc.ChildNodes[0];

            foreach (XmlNode tmpNode in root)
            {
                if (tmpNode.Name == "listproduct")
                {
                    listProduct = tmpNode;
                }
                if (tmpNode.Name == "promotions")
                {
                    listPromotion = tmpNode;
                }
            }
        }

        public Int32 getPrixPrestigeBook()
        {
            Int32 prix = XMLTools.GetAttributeIntValue(doc.FirstChild, "prixPrestigeBook");

            if (prix == 0)
                return 12000;
            else
                return prix;            
        }

        public Int32 getForfait50CD()
        {
            Int32 prix = XMLTools.GetAttributeIntValue(doc.FirstChild, "forfait50CD");

            if (prix == 0)
                return 9850;
            else
                return prix;
        }

        public Int32 getForfait100CD()
        {
            Int32 prix = XMLTools.GetAttributeIntValue(doc.FirstChild, "forfait100CD");

            if (prix == 0)
                return 15800;
            else
                return prix;
        }

        public Int32 getPrixPrestigePageMore()
        {
            Int32 prix = XMLTools.GetAttributeIntValue(doc.FirstChild, "prixPrestigePageMore");

            if (prix == 0)
                return 500;
            else
                return prix;
        }

        public Int32 getCDUnitPrice()
        {
            Int32 prix = XMLTools.GetAttributeIntValue(doc.FirstChild, "prixFichierCD");

            if (prix == 0)
                return 420;
            else
                return prix;            
        }

        public Int32 getPhotoOnBook()
        {
            Int32 prix = XMLTools.GetAttributeIntValue(doc.FirstChild, "prixPictureBook");

            if (prix == 0)
                return 80;
            else
                return prix;
        }

        public int NbProduct()
        {
            if (listProduct == null) return 0;            
            return listProduct.ChildNodes.Count;
        }

        public XmlNode getProduct(int n)
        {
            if (listProduct == null) return null;
            if (n >= listProduct.ChildNodes.Count) return null;
            return listProduct.ChildNodes[n];
        }

        public string getProductID(int n)
        {
            XmlNode product = getProduct(n);
            if (product == null) return null;
            XmlNode idProduct = product.Attributes.GetNamedItem("name");
            if (idProduct !=null)
            {
                return idProduct.Value;
            }
            return "";
        }

        public int getPrice(String idProduct)
        {
            foreach (XmlNode tmpNode in listProduct)
            {
                XmlNode nameNode = tmpNode.Attributes.GetNamedItem("name");
                if (nameNode.Value == idProduct)
                {
                    XmlNode priceNode = tmpNode.Attributes.GetNamedItem("price");
                    if (priceNode != null)
                    {
                        try
                        {
                            return int.Parse(priceNode.Value);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("getPrice error " + e.Message);
                        }
                    }
                }
                
                    
            }
            return 0;
        }

        public UInt64 getPourcentageReduction(int nbPhoto)
        {
            int lastNumber = 0;
            UInt64 result = 0;
            foreach (XmlNode tmpNode in listPromotion)
            {
                int nbPhotoValue = int.Parse(tmpNode.Attributes["nbphoto"].Value);
                if (nbPhoto > nbPhotoValue && lastNumber < nbPhotoValue)
                {
                    result = UInt64.Parse(tmpNode.Attributes["promotion"].Value);
                }
            }
            return result;
        }

        static public productConfig getInstance()
        {
            if (_instance == null)
            {
                _instance = new productConfig(CommunConfig.getInstance().ProductFile);
            }

            return _instance;
        }
    }
}
