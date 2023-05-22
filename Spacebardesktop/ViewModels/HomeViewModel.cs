using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Spacebardesktop.Repositories;

namespace Spacebardesktop.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private string _title;
        private string _description;
        public byte[] foto { get; set; }
        public string CaminhoFoto { get; set; }
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));

                }
            }
            public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public ICommand CriarPost {get; }
        public void Salvar(HomeViewModel home)
        {
            byte[] foto = GetFoto(home.CaminhoFoto);

            var sql = "INSERT INTO tblPost (img_post) VALUES (@caminho_img) ";

            using (var con = new SqlConnection("Server=(local); Database=SpaceBar; Integrated Security=true"))
            {
                con.Open();
                using(var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.Add("@caminho_img", System.Data.SqlDbType.Image, foto.Length).Value = foto;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public byte[] GetFoto(string caminho)
        {
            byte[] foto;
            using (var stream = new FileStream(caminho, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    foto = reader.ReadBytes((int)stream.Length);
                }
            }
            return foto;
        }

    }
    
}
