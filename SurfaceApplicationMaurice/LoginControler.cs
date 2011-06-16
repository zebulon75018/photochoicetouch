using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Media;

namespace SurfaceApplicationMaurice
{    
    static class LoginControler
    {
        static bool isLogged = false;
        static LoginManager manager = null;
        static public bool wantToAddImageAfterLogin = false;
        static public bool wantToAccessToSelection = false;
        static public MyImage addImage = null;
        static public string _login = "";       


        public static bool WantToAddImageAfterLogin
        {
            set { if (value == false && addImage != null) addImage = null; wantToAddImageAfterLogin = value; }
            get { return wantToAddImageAfterLogin; } 
        }

        public static bool Login(string login, string password)
        {
            if (File.Exists(CommunConfig.getInstance().userDirectory + login + ".xml"))
            {
                manager = new LoginManager(login, password);
                isLogged = manager.isLogged;
                if (isLogged)
                {
                    _login = login;
                    SurfaceWindow1.HideLoginButton();
                }
                else
                {
                    SurfaceWindow1.ShowLoginButton();
                }

                if (addImage != null && wantToAddImageAfterLogin)
                {
                  SelectionUser.getInstance().AddImage(addImage);
                  wantToAddImageAfterLogin = false;
                }
                return isLogged;
            }
            else
            {
                SurfaceWindow1.ShowLoginButton();
                return false;
            }            
        }        

        public static bool IsLogged()
        {
            return isLogged;            
        }

        public static bool createLogin(string login, string password,string Date,String name,String firstname)
        {
            manager = new LoginManager();
            return manager.CreateNewLogin(login, password, Date, name, firstname);           
        }

        public static LoginManager getLoginManager()
        {
            if (isLogged && _login!="")
            {
                if (manager == null)
                {
                    manager = new LoginManager(_login);
                }
                else
                {
                    return manager;
                }                        
            }
            return null;
        }

        public static string getUserDirectory()
        {
            return manager.UserDirectory();
        }

        public static void Disconnect()
        {
            isLogged = false;
            manager = null;
            _login = "";
            SurfaceWindow1.ShowLoginButton();
            SurfaceWindow1.StaticGotoMenu();
        }
    }
}
