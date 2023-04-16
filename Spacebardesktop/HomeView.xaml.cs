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
            //SqlDataAdapter adapter = new SqlDataAdapter();
            //DataSet dt = new DataSet();
            //SqlConnection con = new SqlConnection("Server = (local); Database = SpaceBar; Integrated Security = true");
            //con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "insert into tblPost (titulo_Post, descricao_post, data_post) values  =("+ titulo_post.Text+"," + "'" + desc_post.Text + "'" + "," + "'" + data_post.Text + "'" + ")";
            cmd.ExecuteNonQuery();
            //int a = cmd.ExecuteNonQuery();
            //MessageBox.Show(a.ToString());

        }
    }
}
