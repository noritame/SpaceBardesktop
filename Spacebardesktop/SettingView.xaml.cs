using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Spacebardesktop.ViewModels;

namespace Spacebardesktop
{
    /// <summary>
    /// Interação lógica para SettingView.xam
    /// </summary>
    ///



    public partial class SettingView : UserControl
    {
        public string caminhoFoto = "";
        private SettingsViewModel SettingViewModel = new SettingsViewModel();
        public SettingView()
        {
            //InitializeComponent();
        }

        private void btnPostar_Click(object sender, RoutedEventArgs e)
        {
            //SalvarImg();
            SqlConnection con = new SqlConnection("Server=(local); Database=SpaceBar; Integrated Security=true");
            DataSet dt = new DataSet();
            con.Open();
            SqlCommand r = new SqlCommand();
            r.Connection = con;
            //variaeis locias
            r.CommandText = "insert into tblPost (titulo_Post, descricao_post) values  (@titulo, @descricao)";
            r.CommandText = "ALTER TABLE tblPost";
            String titulo = new_titulo.Text.ToString();
            //int data = Convert.ToInt32(Data_post.Text);
            String descricao = new_desc.Text.ToString();
            //r.Parameters.Add("@tipo_usu", SqlDbType.Int).Value = 1;
            r.Parameters.Add("@titulo", SqlDbType.VarChar).Value = titulo;
            //r.Parameters.Add("@data", SqlDbType.Int).Value = data;
            r.Parameters.Add("@descricao", SqlDbType.VarChar).Value = descricao;
            // 1 = usuário comum
            // 2 = criador de conteúdo
            // 3 = verificado
            // 4 = adm
            r.ExecuteNonQuery();
            MessageBox.Show("Post atualizado com sucesso.");
            con.Close();
            return;
        }
    }
}
