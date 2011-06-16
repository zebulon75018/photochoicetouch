using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace SurfaceApplicationMaurice
{
    public class LoginManager
    {
       public   class ResultPrice
        {
            public UInt64 TotalPrice;
            public UInt64 TotalPriceFormatPhoto;
            public UInt64 TotalPriceObjetPhoto;
            public UInt64 PromotionPrice;
            public UInt64 reduction;
            public int nbFormatPhoto;
            public int nbObjetPhoto;
            public bool putPIctureInCD;
            public bool putPIctureOnBook;
        }
        public XmlDocument doc=null;
        public bool isLogged = false;        
        private string Path = "";

        private string PathCustomer = CommunConfig.getInstance().userDirectory;

        public LoginManager()
        {
        }

        public LoginManager(string login)
        {
            try
            {
                doc = new XmlDocument();
                doc.Load(PathCustomer + login + ".xml");
                Path = PathCustomer + login + ".xml";
            }
            catch (Exception e)
            {
            }
        }

        public LoginManager(string login,string pass)
        {
            doc = new XmlDocument();
            doc.Load(PathCustomer + login + ".xml");
            XmlNode root = doc.ChildNodes[0];
            Path = PathCustomer + login + ".xml";

            isLogged = false;

           
            if (root.Attributes["login"].Value== login && root.Attributes["pass"].Value == pass)
                    {                        
                        SurfaceWindow1.HideLoginButton();
                        isLogged = true;    
                    }
        }

        public string UserDirectory()
        {
            if (doc == null) return "";

            return PathCustomer + doc.FirstChild.Attributes["login"].Value + "\\";
        } 

        private XmlNode CreateNewNode(string login, string pass, string date)
        {
            XmlNode result = doc.CreateElement("user");
            XmlAttribute attr =  doc.CreateAttribute("login");
            attr.Value = login;
            result.Attributes.Append(attr);
            attr =  doc.CreateAttribute("pass");
            attr.Value = pass;
            result.Attributes.Append(attr);
            attr =  doc.CreateAttribute("date");
            attr.Value = date;
            result.Attributes.Append(attr);
            attr = doc.CreateAttribute("ordercd");
            attr.Value = "false";
            result.Attributes.Append(attr);

            return result;
            
        }

        public void AddImage(string image,string originalPath)
        {
            XmlNode result = doc.CreateElement("img");
            XmlAttribute attr =  doc.CreateAttribute("path");
            attr.Value = image;
            result.Attributes.Append(attr);
            attr = doc.CreateAttribute("F_1523");
            attr.Value = "1";
            result.Attributes.Append(attr);
            attr = doc.CreateAttribute("originalpath");
            attr.Value = originalPath;
            result.Attributes.Append(attr);
            attr = doc.CreateAttribute("imageoncd");
            attr.Value = "false";
            result.Attributes.Append(attr);

            attr = doc.CreateAttribute("imageonbook");
            attr.Value = "false";
            result.Attributes.Append(attr);

            doc.FirstChild.AppendChild(result);
            doc.Save(Path);
        }

        public string getQuantity(String filename, string productID)
        {
            foreach (XmlNode n in doc.FirstChild.ChildNodes)
            {
                if (n.Attributes["path"].Value == filename)
                {
                    XmlNode product = n.Attributes.GetNamedItem(productID);
                    if (product != null)
                    {
                        return product.Value;
                    }
                    else
                    {
                        return "0";
                    }
                }
            }
            return "0";
        }

        public void setQuantity(String filename, string quantity, string productID)
        {
            XmlNodeList lstChild =doc.FirstChild.ChildNodes;
            for (int n = 0; n < lstChild.Count; n++)
            {
                if (lstChild[n].Attributes["path"].Value == filename)
                {
                    XmlNode customer = lstChild[n].Attributes.GetNamedItem(productID);
                    if (customer != null)
                    {
                        customer.Value = quantity;
                    }
                    else
                    {
                        XmlAttribute attr = doc.CreateAttribute(productID);
                        attr.Value = quantity;
                        lstChild[n].Attributes.Append(attr);
                    }                    
                    break;
                }
            }
            /*
            foreach (XmlNode n in doc.FirstChild.ChildNodes)
            {
                if (n.Attributes["path"].Value == filename)
                {
                   
                    if (n.Attributes["path"].Value == filename)
                    {
                        XmlNode customer = n.Attributes.GetNamedItem(productID);
                        if (customer != null)
                        {
                            customer.Value = quantity;
                        }
                        else
                        {
                            XmlAttribute attr = doc.CreateAttribute(productID);
                            attr.Value = quantity;                            
                            n.Attributes.Append(attr);
                        }
                        break;
                    }
                }
            }
             * */
        }


        public bool isImageAlreadyIn(string originalPath)
        {
            XmlNodeList lstChild =doc.FirstChild.ChildNodes;
            foreach (XmlNode n in doc.FirstChild.ChildNodes)
            {
                XmlNode originalNode = n.Attributes.GetNamedItem("originalpath");
                if (originalNode != null)
                {
                    if (originalNode.Value == originalPath)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public XmlNode getImageNode(string filename)
        {
               XmlNodeList lstChild =doc.FirstChild.ChildNodes;
            foreach(XmlNode n in doc.FirstChild.ChildNodes)
            {
                if (n.Attributes["path"].Value == filename)
                {
                    return n;
                }
            }
            return null;
        }

        public void Save()
        {
            doc.Save(Path);
        }

        public void DeleteImage(string image)
        {
            XmlNode toDel = null;
            foreach (XmlNode n in doc.FirstChild.ChildNodes)
            {
                if (n.Attributes["path"].Value == image)
                {
                    toDel = n;
                }
            }
            if (toDel != null)
            {
                doc.FirstChild.RemoveChild(toDel);

                if (isCDWithoutPrint() && getNbDifferentPicture() < 50)
                {
                    unResetAllCD();
                }

                doc.Save(Path);
            }
            //image = image.Replace("file:///", "");
            //File.Delete(image);            
        }

        public List<String> listImageUserSelection()
        {
            List<String> result = new List<string>();
            foreach (XmlNode n in doc.FirstChild.ChildNodes)
            {
                result.Add(n.Attributes["path"].Value);                
            }
            return result;
        }

        public XmlNode getNodeImageUser(String filename)
        {
            foreach (XmlNode n in doc.FirstChild.ChildNodes)
            {
                if (n.Attributes["path"].Value == filename)
                {
                    return n;
                }
            }

            return null;
        }

        public void setFormatImage(string img,string format)
        {
            /*
            foreach (XmlNode n in doc.FirstChild.ChildNodes)
            {
                if (new Uri(n.Attributes["path"].Value).ToString() == img)
                {
                    n.Attributes["format"].Value = format;
                }
                if (n.Attributes["path"].Value == img)
                {
                    n.Attributes["format"].Value= format;
                }
            }
            doc.Save(Path);
             * */
        }
        
        
        public UInt64  getPrice(XmlNode imageNode,string productID)
        {
            XmlNode productNode = imageNode.Attributes.GetNamedItem(productID);

            if (productNode != null)
            {
                return (UInt64)productConfig.getInstance().getPrice(productID) * UInt64.Parse(productNode.Value);
            }            
            return 0;
        }

        public int getQuantity(XmlNode imageNode, string productID)
        {
            XmlNode productNode = imageNode.Attributes.GetNamedItem(productID);

            if (productNode != null)
            {
               return  int.Parse(productNode.Value);
            }
            return 0;
        }

        public ResultPrice getPriceOnPicture(XmlNode imageNode)
        {
            ResultPrice result = new ResultPrice();
         
            UInt64 totalOtherPrice=0;
            UInt64 totalPriceFormatPhoto=0;
            int nbFormatPhoto = 0;
            int nbProduct= productConfig.getInstance().NbProduct();
            for (int n = 0; n < nbProduct; n++)
            {                
                String productID = productConfig.getInstance().getProductID(n);
                if (productID.Contains("F_"))
                {
                    totalPriceFormatPhoto +=(UInt64) getPrice(imageNode, productID);
                    nbFormatPhoto += getQuantity(imageNode, productID);
                }
                else
                {
                    result.nbObjetPhoto += getQuantity(imageNode, productID);
                    totalOtherPrice +=  getPrice(imageNode, productID);
                }                  
            }
            
            result.reduction = productConfig.getInstance().getPourcentageReduction(nbFormatPhoto);
            UInt64 promotionalFormatPhotoPrice = totalPriceFormatPhoto - (result.reduction * totalPriceFormatPhoto) / 100;
            result.PromotionPrice = totalOtherPrice + promotionalFormatPhotoPrice;
            result.TotalPrice = totalOtherPrice + totalPriceFormatPhoto;            
            result.nbFormatPhoto = nbFormatPhoto;
            result.TotalPriceFormatPhoto = totalPriceFormatPhoto;
            result.TotalPriceObjetPhoto = totalOtherPrice;
            result.putPIctureInCD = isPhotoOnCD(imageNode);
            result.putPIctureOnBook = isPhotoOnBook(imageNode);
                        
            return result;
        }

        public int getNbDifferentPicture()
        {
            return doc.FirstChild.ChildNodes.Count;
        }

        public int getNbFormatImage(XmlNode imageNode)
        {
            int totalformatPhoto = 0;
            int nbProduct = productConfig.getInstance().NbProduct();
            for (int n = 0; n < nbProduct; n++)
            {
                String productID = productConfig.getInstance().getProductID(n);
                if (productID.Contains("F"))
                {
                    totalformatPhoto += getQuantity(imageNode, productID);
                }
            }
            return totalformatPhoto;
        }

        public UInt64 getPromotionalPriceOnPicture(XmlNode imageNode)
        {
            UInt64 totalPrice = 0;
            UInt64 totalPhoto = 0;
            UInt64 totalformatPhoto = 0;
            UInt64 totalOther = 0;
            int nbProduct = productConfig.getInstance().NbProduct();
            for (int n = 0; n < nbProduct; n++)
            {
                String productID = productConfig.getInstance().getProductID(n);
                if (productID.Contains("F"))
                {
                    totalformatPhoto +=(UInt64) getQuantity(imageNode, productID);
                    totalPhoto += (UInt64)getPrice(imageNode, productID);
                }
                else
                {
                    totalOther +=(UInt64) getPrice(imageNode, productID);
                }                
            }

            UInt64 pourcentage = productConfig.getInstance().getPourcentageReduction((int)totalformatPhoto);

            if (pourcentage == 0)
            {
                totalPrice = totalPhoto + totalOther;
            }

            else
            {
                totalPrice = (totalPhoto * pourcentage / 100) + totalOther;
            }
            return totalPrice;
        }

        public void resetAllAcceptCD()
        {
            setImageBooleanAttribut(doc.FirstChild, true, "withoutprinting");
            foreach (XmlNode n in doc.FirstChild.ChildNodes)
            {
                resetAllAcceptCD(n);
            }
        }

        public void resetAllAcceptCD(XmlNode nodeImage)
        {            
            int nbProduct = productConfig.getInstance().NbProduct();
            for (int n = 0; n < nbProduct; n++)
            {
                String productID = productConfig.getInstance().getProductID(n);
                // On reset uniquement les tirages papiers.
                if (productID.Contains("F"))
                {
                    XmlNode customer = nodeImage.Attributes.GetNamedItem(productID);
                    if (customer != null)
                    {
                        customer.Value = "0";
                    }
                }
            }
            setImageOnCD(nodeImage, true);
        }
      
        public void unResetAllCD()
        {
            setImageBooleanAttribut(doc.FirstChild, false, "withoutprinting");
            setAllImageOnCD(false);

            foreach (XmlNode n in doc.FirstChild.ChildNodes)
            {            
                if (getQuantityFormat(n, "F_1523") == 0)
                {
                    setQuantityFormat(n, "F_1523", 1);
                }
            }
        }

        public int getQuantityFormat(XmlNode n,string format)
        {
            XmlNode attr = n.Attributes.GetNamedItem(format);
            if (attr == null) return 0;
            Int32 resultInt = 0;
            Int32.TryParse(attr.Value, out resultInt);

            return resultInt;
        }

        public void setQuantityFormat(XmlNode n, string format, int value)
        {
            XmlNode NodeAttr = n.Attributes.GetNamedItem(format);
            if (NodeAttr == null)
            {
                XmlAttribute attr = n.OwnerDocument.CreateAttribute(format);
                attr.Value = value.ToString();
                n.Attributes.Append(attr);
            }
            else
            {
                NodeAttr.Value = value.ToString();
            }                        
        }

        public void setAllImageOnCD(bool order)
        {
            foreach (XmlNode n in doc.FirstChild.ChildNodes)
            {
                setImageOnCD(n, order);               
            }
        }

        public void setImageBooleanAttribut(XmlNode nodeImage,bool value,string nameAttribut)
        {
            if (nodeImage != null)
            {
                XmlNode cdorder = nodeImage.Attributes.GetNamedItem(nameAttribut);
                if (cdorder != null)
                {
                    if (value == true)
                    {
                        cdorder.Value = "true";
                    }
                    else
                    {
                        cdorder.Value = "false";
                    }
                }
                else
                {
                    XmlAttribute attr = nodeImage.OwnerDocument.CreateAttribute(nameAttribut);
                    if (value == true)
                    {
                        attr.Value = "true";
                    }
                    else
                    {
                        attr.Value = "false";
                    }
                    nodeImage.Attributes.Append(attr);
                }
            }            
        }

        public void setImageOnCD(XmlNode nodeImage, bool order)
        {
            setImageBooleanAttribut(nodeImage, order, "imageoncd");            
        }

        public void setImageOnPhotoBook(XmlNode nodeImage, bool order)
        {
            setImageBooleanAttribut(nodeImage, order, "imageonbook");
        }

        public int nbNbPhotoOnCD()
        {
            int result = 0;
            foreach (XmlNode n in doc.FirstChild.ChildNodes)
            {
                if (isPhotoOnCD(n))
                {
                    result++;
                }                
            }
            return result;
        }

        public int nbNbPhotoOnBook()
        {
            int result = 0;
            foreach (XmlNode n in doc.FirstChild.ChildNodes)
            {
                if (isPhotoOnBook(n))
                {
                    result++;
                }
            }
            return result;
        }

        public bool isCDWithoutPrint()
        {
            return getAttributXmlNode(doc.FirstChild, "withoutprinting");
        }
        public bool getAttributXmlNode(XmlNode node, string attributName)
        {
            XmlNode nodeAttribut = node.Attributes.GetNamedItem(attributName);
            if (nodeAttribut != null)
            {
                if (nodeAttribut.Value.ToLower() == "true")
                {
                    return true;
                }
            }
            return false;
        }

        public bool isPhotoOnBook(XmlNode node)
        {
            return getAttributXmlNode(node, "imageonbook");            
        }

        public void setAllPhotoBookBoolean(bool setornot)
        {
            foreach (XmlNode n in doc.FirstChild.ChildNodes)
            {
                setImageOnPhotoBook(n, setornot);
            }
        }     

        public bool isBookOrder()
        {
            if (doc == null)
            {
                return false;
            }

            XmlNode root = doc.ChildNodes[0];
            XmlNode cdorder = root.Attributes.GetNamedItem("orderbook");

            if (cdorder != null)
            {
                if (cdorder.Value.ToString().ToLower() == "true")
                {
                    return true;
                }
            }
            return false;            
        }


        public void SetBookOrder(bool b)
        {
            string valueBool;

            if (b == true)
            {
                valueBool = "true";
            }
            else
            {
                valueBool = "false";
            }

            
            XmlNode root = doc.ChildNodes[0];
            XmlNode cdorder = root.Attributes.GetNamedItem("orderbook");

            if (cdorder == null)
            {
                XmlAttribute attr = doc.CreateAttribute("orderbook");
                attr.Value = valueBool;
                root.Attributes.Append(attr);
            }
            else
            {
                root.Attributes["orderbook"].Value = valueBool;
            }
            
        }

        public void SetNbPageOnBook(int nbPage)
        {           
            XmlNode root = doc.ChildNodes[0];
            XmlNode cdorder = root.Attributes.GetNamedItem("nbpageonbook");

            if (cdorder == null)
            {
                XmlAttribute attr = doc.CreateAttribute("nbpageonbook");
                attr.Value = nbPage.ToString();
                root.Attributes.Append(attr);
            }
            else
            {
                root.Attributes["nbpageonbook"].Value = nbPage.ToString();
            }
        }

        public int getNbPageOkBook()
        {
            if (doc == null) return 0;
            XmlNode root = doc.ChildNodes[0];
            XmlNode nbpageNode = root.Attributes.GetNamedItem("nbpageonbook");
            if (nbpageNode != null)
            {
                Int32 returnValue = 0;
                Int32.TryParse(nbpageNode.Value, out returnValue);
                return returnValue;
            }
            return 0;
        }

        public bool isPhotoOnCD(XmlNode node)
        {
            XmlNode cdorder = node.Attributes.GetNamedItem("imageoncd");
            if (cdorder!=null)
            {
                if (cdorder.Value.ToLower() == "true")
                {
                    return true;
                }
            }
            return false;
        }



        public bool CreateNewLogin(string login,string pass,string date,String name,String firstname)
        {
            XmlNode root = null;
            if (File.Exists(PathCustomer + login + ".xml"))
            {
                // L'utilisateur existe deja 
                return false;

               // doc.Load(PathCustomer + login + ".xml");
               // root = doc.ChildNodes[0];                
            }
            else
            {
                doc = new XmlDocument();
                doc.AppendChild(doc.CreateElement("root"));
                root = doc.ChildNodes[0];
            }
            Path = PathCustomer + login + ".xml";
            if (Directory.Exists(PathCustomer + login) == false)
            {
                Directory.CreateDirectory(PathCustomer + login);
            }            

            XmlAttribute attr = doc.CreateAttribute("login");
            attr.Value = login;
            root.Attributes.Append(attr);
            attr = doc.CreateAttribute("pass");
            attr.Value = pass;
            root.Attributes.Append(attr);
            attr = doc.CreateAttribute("name");
            attr.Value = name;
            root.Attributes.Append(attr);
            attr = doc.CreateAttribute("firstname");
            attr.Value = firstname;
            root.Attributes.Append(attr);
            attr = doc.CreateAttribute("date");
            attr.Value = date;
            root.Attributes.Append(attr);         
            doc.Save(PathCustomer + login + ".xml");

            return true;
        }
    }
}
