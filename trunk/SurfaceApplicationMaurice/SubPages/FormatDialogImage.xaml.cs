using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SurfaceApplicationMaurice.Utility;
using Artefact.Animation;

namespace SurfaceApplicationMaurice.SubPages
{
    /// <summary>
    /// Interaction logic for FormatDialogImage.xaml
    /// </summary>
    public partial class FormatDialogImage : UserControl
    {
        string filenameImage = "";
        PhotoSelectionPage father = null;
        List<ProductWidget> lstGuiProduct = null;
        XmlNode nodePhoto = null;
        public FormatDialogImage(string image,PhotoSelectionPage f,XmlNode nodeUser,bool goody=false,bool onbook=false)
        {
            InitializeComponent();

            lstGuiProduct = new List<ProductWidget>();
            filenameImage = image;

            LoginManager m = LoginControler.getLoginManager();
            if (m != null)
            {
                nodePhoto = m.getNodeImageUser(filenameImage);

              /*  if (m.getNbDifferentPicture() < 15)
                {
                    labelMinimunForBook.Visibility = System.Windows.Visibility.Visible;
                    surfaceCheckBoxOnBook.IsEnabled = false;
                }
                else
                {
                    labelMinimunForBook.Visibility = System.Windows.Visibility.Hidden;
                    surfaceCheckBoxOnBook.IsEnabled = true;
                }
               * */

                if (m.getNbDifferentPicture() < 10)
                {
                    labelWarningMinimun.Visibility = System.Windows.Visibility.Visible;
                    surfaceCheckBoxOrderCD2.IsEnabled = false;
                }
                else
                {
                    labelWarningMinimun.Visibility = System.Windows.Visibility.Hidden;
                    surfaceCheckBoxOrderCD2.IsEnabled = true;
                }
            }


            Utility.gradientManager gm = new gradientManager();
            gm.setgradient(gridBackground);
            gm.setgradient(stackPanelFormatPhoto);
            gm.setgradient(grdiOderImageOnCD);
            gm.setgradient(gridOnBook);
            gm.setgradient(stackPanelProductObject);
            //gm.setgradient(tabControlProduct);

            this.Cursor = Cursors.Wait;

            addProduct(0, 0, 0, ref stackPanelFormatPhoto);
            addProduct(1, 0, 1, ref stackPanelFormatPhoto);
            addProduct(2, 0, 2, ref stackPanelFormatPhoto);
            addProduct(3, 1, 0, ref stackPanelFormatPhoto);
            addProduct(4, 1, 1, ref stackPanelFormatPhoto);
            addProduct(5, 1, 2, ref stackPanelFormatPhoto);

            addProduct(6, 0, 0, ref stackPanelProductObject);
            addProduct(7, 0, 1, ref stackPanelProductObject);
            addProduct(8, 0, 2, ref stackPanelProductObject);
            addProduct(9, 1, 0, ref stackPanelProductObject);
            addProduct(10, 1, 1, ref stackPanelProductObject);
            addProduct(11, 1, 2, ref stackPanelProductObject);                    

            this.Cursor = Cursors.Arrow;

            if (nodePhoto != null)
            {
                XmlNode cdorder = nodePhoto.Attributes.GetNamedItem("imageoncd");
                if (cdorder != null)
                {
                    if (cdorder.Value.ToLower() == "true")
                    {
                        surfaceCheckBoxOrderCD2.IsChecked = true;
                    }
                    else
                    {
                        surfaceCheckBoxOrderCD2.IsChecked = false;
                    }
                }
                else
                {
                    surfaceCheckBoxOrderCD2.IsChecked = false;
                }


                XmlNode bookorder = nodePhoto.Attributes.GetNamedItem("imageonbook");
                if (bookorder != null)
                {
                    if (bookorder.Value.ToLower() == "true")
                    {
                        surfaceCheckBoxOnBook.IsChecked = true;
                    }
                    else
                    {
                        surfaceCheckBoxOnBook.IsChecked = false;
                    }
                }
                else
                {
                    surfaceCheckBoxOnBook.IsChecked = false;
                }

            }

            father = f;            
            /*
             * LoginManager m = LoginControler.getLoginManager();
            if (m != null)
            {
                if (m.OrderCd) surfaceCheckBoxOrderCD2.IsChecked = true;
                else surfaceCheckBoxOrderCD2.IsChecked = false;

                string format =m.getFormatImage(image);

                if (format == "15x23") { surfaceRadioButton1.IsChecked = true; }
                if (format == "20x30") { surfaceRadioButton2.IsChecked = true; }
                if (format == "30x45") { surfaceRadioButton3.IsChecked = true; }                                
            }  
             * */

            if (goody)
            {
                tabItem2.IsSelected = true;               
            }
            if (onbook)
            {
                tabItem4.IsSelected = true;                
            }
            updatePrice();
            this.father.StartModal();
        }
      
        public void updatePrice()
        {
            LoginManager m = LoginControler.getLoginManager();
            if (nodePhoto != null && m!=null)
            {
                labelPrice.Visibility = System.Windows.Visibility.Visible;
                labelNormalPrice.Visibility = System.Windows.Visibility.Visible;
                LoginManager.ResultPrice result = m.getPriceOnPicture(nodePhoto);
                if (tabItem1.IsSelected)
                {                       
                    if (result.reduction == 0)
                    {
                        labelNormalPrice.Content = result.TotalPriceFormatPhoto.ToString() + " Mur";
                        labelPourcentage.Visibility = System.Windows.Visibility.Hidden;
                        labelReduction.Visibility = System.Windows.Visibility.Hidden;
                        labelPromo.Visibility = System.Windows.Visibility.Hidden;
                        labelPromotionPrice.Visibility = System.Windows.Visibility.Hidden;
                        labelReduction.Visibility = System.Windows.Visibility.Hidden;
                    }
                    else
                    {
                        labelPourcentage.Visibility = System.Windows.Visibility.Visible;
                        labelReduction.Visibility = System.Windows.Visibility.Visible;
                        labelPromo.Visibility = System.Windows.Visibility.Visible;
                        labelPromotionPrice.Visibility = System.Windows.Visibility.Visible;
                        labelReduction.Visibility = System.Windows.Visibility.Visible;
                        labelPromotionPrice.Content = result.PromotionPrice.ToString() + " Mur";
                        labelReduction.Content = result.reduction.ToString() + " %";
                    }
                }
                if (tabItem2.IsSelected)
                {
                    labelNormalPrice.Content = result.TotalPriceObjetPhoto.ToString() + " Mur";
                    labelPourcentage.Visibility = System.Windows.Visibility.Hidden;
                    labelReduction.Visibility = System.Windows.Visibility.Hidden;
                    labelPromo.Visibility = System.Windows.Visibility.Hidden;
                    labelPromotionPrice.Visibility = System.Windows.Visibility.Hidden;
                    labelReduction.Visibility = System.Windows.Visibility.Hidden;
                }
                if (tabItem3.IsSelected)
                {
                    labelPrice.Visibility = System.Windows.Visibility.Hidden;
                    labelNormalPrice.Visibility = System.Windows.Visibility.Hidden;
                    //labelNormalPrice.Content = result.TotalPriceObjetPhoto.ToString() + " Mur";
                    labelPourcentage.Visibility = System.Windows.Visibility.Hidden;
                    labelReduction.Visibility = System.Windows.Visibility.Hidden;
                    labelPromo.Visibility = System.Windows.Visibility.Hidden;
                    labelPromotionPrice.Visibility = System.Windows.Visibility.Hidden;
                    labelReduction.Visibility = System.Windows.Visibility.Hidden;
                }
                if (tabItem4.IsSelected)
                {
                    labelPrice.Visibility = System.Windows.Visibility.Hidden;
                    labelNormalPrice.Visibility = System.Windows.Visibility.Hidden;
                    //labelNormalPrice.Content = result.TotalPriceObjetPhoto.ToString() + " Mur";
                    labelPourcentage.Visibility = System.Windows.Visibility.Hidden;
                    labelReduction.Visibility = System.Windows.Visibility.Hidden;
                    labelPromo.Visibility = System.Windows.Visibility.Hidden;
                    labelPromotionPrice.Visibility = System.Windows.Visibility.Hidden;
                    labelReduction.Visibility = System.Windows.Visibility.Hidden;
                }
            }        
        }
      

        private void addProduct(int n,int row,int col,ref Grid grid)
        {
            ProductWidget pw = new ProductWidget(filenameImage,productConfig.getInstance().getProduct(n),this);
            Grid.SetColumn(pw, col);
            Grid.SetRow(pw, row);
            grid.Children.Add(pw);

            lstGuiProduct.Add(pw);
        }

        private void resetProduct()
        {
            surfaceCheckBoxOrderCD2.IsChecked = false;
            surfaceCheckBoxOnBook.IsChecked = false;
            foreach (ProductWidget pw in lstGuiProduct)
            {
                if (pw != null)
                {
                    pw.ResetQuantity();
                }
            }

            if (lstGuiProduct.Count != 0)
            {
                if (lstGuiProduct[0] != null)
                {
                    lstGuiProduct[0].SetQuantity(1);
                }
            }            
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            father.UpdateShowPrice();
            TransformationUtil.HideDialog((Grid)this.Parent);
            this.father.StopModal();

            LoginManager lm = LoginControler.getLoginManager();
            lm.Save();
        }

        private void surfaceCheckBoxOrderCD_Checked(object sender, RoutedEventArgs e)
        {
          
        }

        private void surfaceRadioButton1_Checked(object sender, RoutedEventArgs e)
        {
            LoginManager m = LoginControler.getLoginManager();
            if (m != null)
            {
                if ((bool)surfaceRadioButton1.IsChecked) m.setFormatImage(filenameImage, "15x23");
            }
             
        }

        private void surfaceRadioButton2_Checked(object sender, RoutedEventArgs e)
        {
            LoginManager m = LoginControler.getLoginManager();
            if (m != null)
            {
                if ((bool)surfaceRadioButton2.IsChecked) m.setFormatImage(filenameImage, "20x30");
            }
        }

        private void surfaceRadioButton3_Checked(object sender, RoutedEventArgs e)
        {
            LoginManager m = LoginControler.getLoginManager();
            if (m != null)
            {
                if ((bool)surfaceRadioButton3.IsChecked) m.setFormatImage(filenameImage, "30x45");
            }
        }

        private void surfaceCheckBoxOrderCD_TouchUp(object sender, TouchEventArgs e)
        {
          
        }
       
        private void surfaceCheckBoxOrderCD2_Unchecked(object sender, RoutedEventArgs e)
        {
            if (nodePhoto != null)
            {
                XmlNode cdorder = nodePhoto.Attributes.GetNamedItem("imageoncd");
                if (cdorder != null)
                {
                    cdorder.Value = "false";
                }
                else
                {
                    XmlAttribute attr = nodePhoto.OwnerDocument.CreateAttribute("imageoncd");
                    attr.Value = "false";
                    nodePhoto.Attributes.Append(attr);
                }
            }            
        }

        private void surfaceCheckBoxOrderCD2_Checked_1(object sender, RoutedEventArgs e)
        {
            if (nodePhoto != null)
            {
                XmlNode cdorder = nodePhoto.Attributes.GetNamedItem("imageoncd");
                if (cdorder != null)
                {
                    cdorder.Value = "true";
                }
                else
                {
                    XmlAttribute attr = nodePhoto.OwnerDocument.CreateAttribute("imageoncd");
                    attr.Value = "true";
                    nodePhoto.Attributes.Append(attr);
                }
            }            
        }

        private void buttonReset_Click(object sender, RoutedEventArgs e)
        {
            TransformationUtil.AppearControl(labelAreyouSure);
        }

        private void labelAreyouSure_TouchDown(object sender, TouchEventArgs e)
        {

            resetProduct();
            TransformationUtil.DisAppearControl(labelAreyouSure);
            ShowMessage("Ok ");
        }

        private void labelAreyouSure_MouseDown(object sender, MouseButtonEventArgs e)
        {
            resetProduct();
            TransformationUtil.DisAppearControl(labelAreyouSure);
        }
        private void ShowMessage(string msg)
        {
            textBlockMessage.Content = msg;
            textBlockMessage.Opacity = 0;
            textBlockMessage.Visibility = Visibility.Visible;
            var eog = new EaseObjectGroup();
            //eog.Complete += g => Debug.WriteLine("COMPLETE");
            var eo = ArtefactAnimator.AddEase(textBlockMessage, new[] { AnimationTypes.Alpha }, new object[] { 1 }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);
            eog.Complete += delegate(EaseObjectGroup easeObjectGroup)
            {
                var eog2 = new EaseObjectGroup();
                //eog.Complete += g => Debug.WriteLine("COMPLETE");
                var eo2 = ArtefactAnimator.AddEase(textBlockMessage, new[] { AnimationTypes.Alpha }, new object[] { 0 }, 1.5, AnimationTransitions.CubicEaseOut, 1);
                eog2.AddEaseObject(eo2);
                eog2.Complete += delegate(EaseObjectGroup easeObjectGroup2) { textBlockMessage.Visibility = System.Windows.Visibility.Hidden; };
            };
        }

        private void ShowArrow()
        {            
            textBlockMessage.Opacity = 0;
            textBlockMessage.Visibility = Visibility.Visible;
            var eog = new EaseObjectGroup();
            //eog.Complete += g => Debug.WriteLine("COMPLETE");
            var eo = ArtefactAnimator.AddEase(textBlockMessage, new[] { AnimationTypes.Alpha }, new object[] { 1 }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);
            eog.Complete += delegate(EaseObjectGroup easeObjectGroup)
            {
                var eog2 = new EaseObjectGroup();
                //eog.Complete += g => Debug.WriteLine("COMPLETE");
                var eo2 = ArtefactAnimator.AddEase(textBlockMessage, new[] { AnimationTypes.Alpha }, new object[] { 0 }, 1.5, AnimationTransitions.CubicEaseOut, 1);
                eog2.AddEaseObject(eo2);
                eog2.Complete += delegate(EaseObjectGroup easeObjectGroup2) { textBlockMessage.Visibility = System.Windows.Visibility.Hidden; };
            };
        }

        private void tabControlProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updatePrice();
        }

        private void surfaceCheckBoxOnBook_Unchecked(object sender, RoutedEventArgs e)
        {
            if (nodePhoto != null)
            {
                XmlNode bookorder = nodePhoto.Attributes.GetNamedItem("imageonbook");
                if (bookorder != null)
                {
                    bookorder.Value = "false";
                }
                else
                {
                    XmlAttribute attr = nodePhoto.OwnerDocument.CreateAttribute("imageonbook");
                    attr.Value = "false";
                    nodePhoto.Attributes.Append(attr);
                }
            }            
        }

        private void surfaceCheckBoxOnBook_Checked(object sender, RoutedEventArgs e)
        {
            if (nodePhoto != null)
            {
                XmlNode bookorder = nodePhoto.Attributes.GetNamedItem("imageonbook");
                if (bookorder != null)
                {
                    bookorder.Value = "true";
                }
                else
                {
                    XmlAttribute attr = nodePhoto.OwnerDocument.CreateAttribute("imageonbook");
                    attr.Value = "true";
                    nodePhoto.Attributes.Append(attr);
                }
            }            
        }  
    }
}
