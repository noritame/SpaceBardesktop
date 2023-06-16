using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.Win32;
using System.Data;
using System.Threading;
using Spacebardesktop.Models;
using System.Net;
using Spacebardesktop.Repositories;
using System.Security.Principal;

namespace Spacebardesktop.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private string _title;
        private string _description;
        public string CaminhoFoto { get; set; }
       
        public byte[] Foto { get; set; }


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

        public ICommand CriarPost { get; }


        public void Salvar(HomeViewModel homeView)
        {
            byte[] foto = GetFoto(homeView.CaminhoFoto);
            String conexaoString = "Server=(local); Database=SpaceBar; Integrated Security=true";
                String titulo = _title.ToString();
                String texto = _description.ToString();
                DateTime? dataAtual = DateTime.Now;
            UserModel user = GetById(Thread.CurrentPrincipal.Identity.Name);
            using (var con = new SqlConnection(conexaoString))
                {
                    con.Open();
                    using (var cmd = new SqlCommand("InsertPost", con))
                    {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@titulo", titulo);
                    if (!string.IsNullOrEmpty(texto))
                        cmd.Parameters.AddWithValue("@texto", texto);
                    else
                        cmd.Parameters.AddWithValue("@texto", DBNull.Value);
                    cmd.Parameters.AddWithValue("@data", dataAtual);
                    if (foto != null && foto.Length > 0)
                        cmd.Parameters.AddWithValue("@imagem", foto);
                    else
                        cmd.Parameters.AddWithValue("@imagem", DBNull.Value);
                    cmd.Parameters.AddWithValue("@id", user.Id);
                    cmd.ExecuteNonQuery();
                    }
                }
        }
        


        private byte[] GetFoto(string caminhoFoto)
        {
            if (string.IsNullOrEmpty(caminhoFoto))
            {
                // Lidar com a situação de caminho da foto vazio
                return null;
            }
            byte[] foto;
            using (var stream = new FileStream(caminhoFoto, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    foto = reader.ReadBytes((int)stream.Length);
                }
            }
            return foto;
        }

        public static UserModel GetById(string nomeUsuario)
        {
            string query = "SELECT cod_usuario FROM tblUsuario WHERE login_usuario = @nomeUsuario";
            string conexaoString = "Server=(local); Database=SpaceBar; Integrated Security=true";
      
            UserModel user = null;

            using (var connection = new SqlConnection(conexaoString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nomeUsuario", nomeUsuario);

                    // Execute a consulta e obtenha o ID do usuário
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        int userId = Convert.ToInt32(result);
                        user = new UserModel() {
                            Id = userId.ToString(),
                        };
                    }
                }
            }
            return user;
        }
    }
}