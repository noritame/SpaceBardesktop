using Spacebardesktop.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Forms;


namespace Spacebardesktop
{
    /// <summary>
    /// Interação lógica para HomeView.xam
    /// </summary>
    public partial class HomeView
    {

        public string caminhoFoto = "";
        private HomeViewModel HomeViewModel = new HomeViewModel();
        public HomeView()
        {
            InitializeComponent();
        }

        private void btnPostar_Click(object sender, RoutedEventArgs e)
        {
            SalvarImg();
            String conexaoString = "Server=(local); Database=SpaceBar; Integrated Security=true";
            String titulo = titulo_post.Text.ToString();
            String texto = desc_post.Text.ToString();
            
            using (SqlConnection con = new SqlConnection(conexaoString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("insert into tblPost (titulo_Post, texto_post) values  (@titulo, @texto)", con))
                {
                    cmd.Parameters.Add("@titulo", SqlDbType.VarChar).Value = titulo;
                    cmd.Parameters.Add("@texto", SqlDbType.VarChar).Value = texto;
                    cmd.ExecuteNonQuery();
                }
            }
            System.Windows.MessageBox.Show("Post inserido com sucesso.");
            return;
            
        }
        private void SalvarImg()
        {
            HomeViewModel.CaminhoFoto = caminhoFoto;
            HomeViewModel.Salvar(HomeViewModel);
            System.Windows.MessageBox.Show("Gravei HAHAHAAHAHAHA");
        }

        private void btnImagen_Click(object sender, RoutedEventArgs e)
        {
            CarregarFoto();

        }
        private void CarregarFoto()
        {
            var OpenFile = new OpenFileDialog();
            OpenFile.Filter = "Arquivos de imagens jpg e png|*.jpg; *png";
            OpenFile.Multiselect = false;

            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                caminhoFoto = OpenFile.FileName;
            }
        }
    }
}
