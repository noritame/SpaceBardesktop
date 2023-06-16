using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Spacebardesktop.Models;
using Spacebardesktop.Repositories;
using Spacebardesktop.ViewModels;

namespace Spacebardesktop
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        private MainViewModel viewModel;    
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
            
        private void btnClosed_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            viewModel.LoadCurrentUserData();
            string nomeUsuario = txtUser.Text;
            HomeViewModel.GetById(nomeUsuario);
            string nomeUsusarioType = txtUser.Text;
            UserRepository.GetByType(nomeUsusarioType);
            UserModel user = UserRepository.GetByIcon(nomeUsuario);
            if (user != null)
            {
                byte[] imageData = viewModel.CurrentUserAccount.ProfilePicture as byte[];
                if (imageData != null && imageData.Length > 0)
                {
                    user.Icon = imageData; // Atribua o imageData à propriedade Icon do objeto user
                }
            }
        }

        private void BindablePasswordBox_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
