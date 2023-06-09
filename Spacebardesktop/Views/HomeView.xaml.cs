using Spacebardesktop.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace Spacebardesktop
{
    /// <summary>
    /// Interação lógica para HomeView.xam
    /// </summary>
    public partial class HomeView
    {

        public string CaminhoFoto = "";
        private HomeViewModel homeView = new HomeViewModel();
        public HomeView()
        {
            InitializeComponent();
        }

        private void btnPostar_Click(object sender, RoutedEventArgs e)
        {
            Salvar();
        }
        private void  btnImagen_Click(object sender, RoutedEventArgs e)
        {
            CarregarFoto();

        }
        private void CarregarFoto()
        {
            var openFile = new OpenFileDialog();
            openFile.Filter = "Arquivos de imagens jpg e png|*.jpg;*.png";
            openFile.Multiselect = false;

            if(openFile.ShowDialog()== DialogResult.OK)
                CaminhoFoto = openFile.FileName;

            if (!string.IsNullOrEmpty(CaminhoFoto))
            {
                BitmapImage image = new BitmapImage(new Uri(CaminhoFoto));
                imageControl.Source = image;
            }

        }
        private void Salvar()
        {
            homeView.Title = titulo_post.Text;
            homeView.Description = desc_post.Text;
            homeView.CaminhoFoto = CaminhoFoto;
            homeView.Salvar(homeView);

            System.Windows.MessageBox.Show("AAAAA");
        }

       
    }
}