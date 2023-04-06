using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Spacebardesktop.ViewModels;

namespace Spacebardesktop
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected void ApplicationStart(object sender, StartupEventArgs e)
        {
            //var mainWindow = new MainWindow();
            //mainWindow.Show();
            //mainWindow.IsVisibleChanged += (s, ev) =>
            //{
            //    if (mainWindow.IsVisible == false && mainWindow.IsLoaded)
            //    {
                 var loginView = new LoginView();
                    loginView.Show();
            //        mainWindow.Close();
            //    }
            //};
        }
     }
}