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
            UserRepository userRepository = new UserRepository();
            UserModel user = userRepository.GetByUsername(nomeUsuario);
            if (user != null)
            {
                // Atualizar a propriedade CurrentUserAccount com os dados do usuário
                viewModel.CurrentUserAccount.Username = user.Username;
                viewModel.CurrentUserAccount.DisplayName = $"{user.Username}";

                // Verificar se o usuário tem um ícone definido
                if (user.Icon != null && user.Icon.Length > 0)
                {
                    using (MemoryStream stream = new MemoryStream(user.Icon))
                    {
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = stream;
                        image.EndInit();

                        // Atribuir o objeto BitmapImage à propriedade ProfilePicture
                        viewModel.CurrentUserAccount.ProfilePicture = image;
                    }
                }
                else
                {
                    // Definir uma imagem padrão ou deixar em branco, caso não haja imagem do usuário
                    viewModel.CurrentUserAccount.ProfilePicture = null;
                }
            }
            else
            {
                // Usuário inválido
                viewModel.CurrentUserAccount.DisplayName = "Usuário Inválido";
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
