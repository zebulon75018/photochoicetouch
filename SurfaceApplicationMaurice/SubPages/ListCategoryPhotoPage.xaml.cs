using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using SurfaceApplicationMaurice.Utility;
using WPFLocalization;

namespace SurfaceApplicationMaurice.SubPages
{
    /// <summary>
    /// Interaction logic for ListCategoryPhotoPage.xaml
    /// </summary>
    public partial class ListCategoryPhotoPage : UserControl
    {        
        FileScanner fs = new FileScanner();

        // On se souvienbt des categories selectionnées.
        private Stack<XmlElement> stackNode = null;
        //
        // Pour avoir la correspondance entre les DateTime et les items de la surfaceList.
        // 
        List<DateTime> dateTimeItemsList = new List<DateTime>();
        globalDatasingleton.TypeOfSelection typeOfSelection = globalDatasingleton.TypeOfSelection.BYCATEGORY;
        CategoriesXml xmlCat = null;
        XmlElement currentElm =null;       

        public ListCategoryPhotoPage()
        {
            stackNode = new Stack<XmlElement>();
            InitializeComponent();
            xmlCat = globalDatasingleton.getInstance().getCategories();
            UpdateList();
            
            Utility.gradientManager gm = new Utility.gradientManager();
            gm.setgradient(gridBackground);
            gm.setgradient(QuestionLogOut);
        }

        public  bool CurrentElmIsNull()
        {
            if (currentElm == null)
            {
                return true;
            }
            return false;
        }

       public void UpdateList()
        {
            typeOfSelection = globalDatasingleton.TypeOfSelection.BYCATEGORY;
            SurfaceListBoxItem il = null;
            ItemListCategory ilc = null;            
            List<String> name = null;
            List<String> bitmapname = null;

            if (currentElm == null)
            {
                name = xmlCat.GetNameCategories();
                bitmapname = xmlCat.GetBitmapsCategories();               
            }
            else
            {             
                name = xmlCat.GetNameChildCategories(ref currentElm);
                bitmapname = xmlCat.GetBitmapsChildCategories(ref currentElm);                
            }

            surfaceListBox1.Items.Clear();
            int i = 0;

            if (currentElm != null)
            {
                il = new SurfaceListBoxItem();
                ilc = new ItemListCategory();

                textBlockName.Text = xmlCat.getLanguageName(currentElm);

                //ilc.Label = LocalizationManager.ResourceManager.GetString("Back");
                //ilc.ImageSource = "pack://application:,,,/Images/previous.png";
                //il.Content = ilc;
                //surfaceListBox1.Items.Add(il);
            }
            else
            {
                textBlockName.Text = "";
            }
           
            foreach (String n in name)
            {
                il = new SurfaceListBoxItem();
                ilc = new ItemListCategory();
                ilc.GrowingStep = 0;
                if (File.Exists(bitmapname[i]) == true)
                {
                    ilc.ImageSource = bitmapname[i];
                }
                ilc.Label = n;
                il.Content = ilc;
                surfaceListBox1.Items.Add(il);
                i++;
            }            
        }

       public void resetSelectionList()
       {
           stackNode.Clear();
           surfaceListBox1.SelectedIndex = -1;           
           currentElm = null;
           UpdateList();
       }

        /*
         * Retourne true s'il y a une category a deplier
         * Faux Sinon.
         */
       public bool GoBackInParentCategory()
       {
           if (stackNode.Count() == 0) return false;

           currentElm = stackNode.Pop();
           UpdateList();
           return true;
       }


        private void surfaceListBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         //   if (SurfaceWindow1.StaticIsListPhotoInsert() == true) return;
            if (surfaceListBox1.SelectedIndex == -1) return;

            if (stackNode.Count() == 0)
            {          
                 // Le -1 est la pour compensé l'element choix par date dans la surfaceListBox.  
                 XmlElement elm = xmlCat.getCurrentElem(surfaceListBox1.SelectedIndex);               
                 if (elm.ChildNodes.Count == 0)
                 {
                   ListPhotosPage pp = new ListPhotosPage(elm);
                   SurfaceWindow1.staticSetBackPage(this, pp);
                   surfaceListBox1.SelectedIndex = -1;                   
                   }
                  else
                  {
                    stackNode.Push(currentElm);
                    currentElm = elm;                   
                    UpdateList();                   
                   }
            }
            else
            {
             
                            if( currentElm.ChildNodes.Count < surfaceListBox1.SelectedIndex ) { return; }
                            XmlElement elm = xmlCat.getCurrentChildElem(ref currentElm, surfaceListBox1.SelectedIndex );
                            if (elm  == null) return;
                            if (elm.ChildNodes.Count == 0)
                            {
                                ListPhotosPage pp = new ListPhotosPage(elm);                                
                                SurfaceWindow1.staticSetBackPage(this, pp);
                                surfaceListBox1.SelectedIndex = -1;                                
                            }
                            else
                            {
                                stackNode.Push(currentElm);
                                currentElm = elm;
                                UpdateList();
                            }                                                                        
            }
        }

        private void FillListByDate()
        {
            typeOfSelection = globalDatasingleton.TypeOfSelection.BYDATE;
            SurfaceListBoxItem il = null;
            ItemListCategory ilc = null;

            CategoriesXml xmlcat = globalDatasingleton.getInstance().getCategories();

            if (fs.isEmpty == true)
            {
                foreach (string dirs in xmlcat.getListDirectory())
                {
                    fs.AddDirectory(dirs);
                }                
            }

            surfaceListBox1.Items.Clear();            

            il = new SurfaceListBoxItem();
            ilc = new ItemListCategory();
            ilc.Label = LocalizationManager.ResourceManager.GetString("ChoicebyCategory");
            il.Content = ilc;
            surfaceListBox1.Items.Add(il);
           
            // On ajoute un item dans la liste pour correspondre au index de la surfaceListbox
            dateTimeItemsList.Add(DateTime.Today);

            DateTime curTime = DateTime.Today;
            for (int i = 0; i < 15; i++)
            {
                if (fs.getFiles(curTime).Count > 0)
                {
                  //  string spaceString = "";
                    il = new SurfaceListBoxItem();
                    ilc = new ItemListCategory();
                    ilc.GrowingStep = 0;
                    /*for (int n = 20-curTime.DayOfWeek.ToString().Length; n>0 ; n--)
                    {
                        spaceString += " ";
                    }
                     */

                    ilc.Label = curTime.Day.ToString() + " / " + curTime.Month + " / " + curTime.Year;
                    il.Content = ilc;
                    surfaceListBox1.Items.Add(il);
                    DateTime dateTmp = new DateTime(curTime.Year, curTime.Month, curTime.Day);
                    dateTimeItemsList.Add(dateTmp);
                }
                curTime = curTime.AddDays(-1);
            }
        }

        private void canvas1_TouchUp(object sender, TouchEventArgs e)
        {
            currentElm = null;
            UpdateList();
        }

        public void setModal(bool b)
        {
            stackPanel1.IsEnabled = !b;
        }

        public void ShowQuestionLogOut()
        {
            SurfaceWindow1.startModal();
            TransformationUtil.AppearGrid(QuestionLogOut);

        }

        private void SurfaceButton_Click(object sender, RoutedEventArgs e)
        {
                       
            TransformationUtil.DisAppearGrid(QuestionLogOut);
            SurfaceWindow1.stopModal();
        }

        private void SurfaceButton_Click_1(object sender, RoutedEventArgs e)
        {
            setModal(false);
            TransformationUtil.DisAppearGrid(QuestionLogOut);
            LoginControler.Disconnect();
            SurfaceWindow1.stopModal();
            SurfaceWindow1.StaticGotoMenu();
        }
    }
}
