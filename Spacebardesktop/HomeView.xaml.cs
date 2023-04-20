using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

namespace Spacebardesktop
{
    /// <summary>
    /// Interação lógica para HomeView.xam
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void btnPostar_Click(object sender, RoutedEventArgs e) 
        {
            SqlConnection con = new SqlConnection("Server=(local); Database=SpaceBar; Integrated Security=true");
            DataSet dt = new DataSet();
            con.Open();
            SqlCommand r = new SqlCommand();
            r.Connection = con;
            //variaeis locias
            r.CommandText = "insert into tblPost (titulo_Post, descricao_post) values  (@titulo, @descricao)";
            String titulo = titulo_post.Text.ToString();
            //int data = Convert.ToInt32(Data_post.Text);
            String descricao = desc_post.Text.ToString();
            //r.Parameters.Add("@tipo_usu", SqlDbType.Int).Value = 1;
            r.Parameters.Add("@titulo", SqlDbType.VarChar).Value = titulo;
            //r.Parameters.Add("@data", SqlDbType.Int).Value = data;
            r.Parameters.Add("@descricao", SqlDbType.VarChar).Value = descricao;
            // 1 = usuário comum
            // 2 = criador de conteúdo
            // 3 = verificado
            // 4 = adm
           r.ExecuteNonQuery();
            MessageBox.Show("Post inserido com sucesso.");
            con.Close();
            return;
            
        }
    }
}