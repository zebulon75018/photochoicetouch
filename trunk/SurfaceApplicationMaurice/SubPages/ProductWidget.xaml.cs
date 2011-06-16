using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using WPFLocalization;

namespace SurfaceApplicationMaurice.SubPages
{
    /// <summary>
    /// Interaction logic for ProductWidget.xaml
    /// </summary>
    public partial class ProductWidget : UserControl
    {        
        String  productID;
        String filenameImage;
        FormatDialogImage father;
        public ProductWidget(String filename,XmlNode node,FormatDialogImage f)
        {
            InitializeComponent();

            father = f;

            for (int n = 0; n < 20; n++)
            {
                comboBoxNbProduct.Items.Add(n);
            }

            LoginManager lm = LoginControler.getLoginManager();
            filenameImage = filename;
            if (node != null)
            {
                XmlNode nameNode = node.Attributes.GetNamedItem("name");

                if (nameNode != null)
                {                    
                    productID = nameNode.Value;
                    this.comboBoxNbProduct.SelectedIndex = int.Parse(lm.getQuantity(filename, productID));

                    labelName.Content = LocalizationManager.ResourceManager.GetString(nameNode.Value);                    
                }
                

                XmlNode priceNode = node.Attributes.GetNamedItem("price");
                if (priceNode != null)
                {                 
                    labelPrice.Content = priceNode.Value + " Mur"; 
                }
                
                XmlNode imageNode = node.Attributes.GetNamedItem("image");
                if (imageNode != null)
                {
                    try
                    {
                        XmlNode noRessourceNode = node.Attributes.GetNamedItem("noressource");
                        if (noRessourceNode != null)
                        {
                            image1.Source = new BitmapImage(new Uri(imageNode.Value));
                        }
                        else
                        {
                            image1.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/" + imageNode.Value));
                        }
                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show(" Error loading Ressource File " + exp.Message);
                    }
                }                                
            }          
        }

        public void ResetQuantity()
        {
            this.comboBoxNbProduct.SelectedIndex = 0;
        }

        public void SetQuantity(int n)
        {
            this.comboBoxNbProduct.SelectedIndex = n;
        }

        private void comboBoxNbProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             LoginManager lm = LoginControler.getLoginManager();
             lm.setQuantity(filenameImage,this.comboBoxNbProduct.SelectedValue.ToString(),productID);

             labelTotal.Content = Price().ToString() + " Mur";
             father.updatePrice();
        }

        public int Price()
        {
            return productConfig.getInstance().getPrice(productID)*int.Parse(this.comboBoxNbProduct.SelectedValue.ToString());
        }

        public bool IsPhoto()
        {
            if (productID.Contains("F_"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }        
    }
}
