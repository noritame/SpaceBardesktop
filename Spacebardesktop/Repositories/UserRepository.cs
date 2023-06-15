using Spacebardesktop.Models;
using Spacebardesktop.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using BCrypt.Net;
using System.Diagnostics.Contracts;
using System.Drawing;

namespace Spacebardesktop.Repositories
{
    public class UserRepository : Repositorio, IUserRepository
    {
        private string _errorManage;
        //private string caminhoFoto;
        //public UserRepository(string caminhoFoto)
        //{
        //    this.caminhoFoto = caminhoFoto;
        //}
        public string ErrorManage
        {

            get
            {

                return _errorManage;

            }

            set
            {

                _errorManage = value;

            }
        }

        public void Add(UserModel userModel)
        {
            throw new NotImplementedException();
        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser = false;
            var parameters = new List<SqlParameter>();

            // Comparar as senhas criptografadas
            using (var connection = GetConnection())
            {
                using (var cmd = new SqlCommand("SELECT senha_usuario FROM tblUsuario WHERE login_usuario = @username", connection))
                {
                    cmd.Parameters.AddWithValue("@username", credential.UserName);
                    connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string senhaUsuarioFromDatabase = reader["senha_usuario"].ToString();
                            bool senhaCorreta = BCrypt.Net.BCrypt.Verify(credential.Password, senhaUsuarioFromDatabase);

                            if (senhaCorreta)
                            {
                                reader.Close();

                                // Senha correta, chamar a stored procedure para verificar o usuário

                                // Crie a lista de parâmetros e adicione os parâmetros necessários
                                List<SqlParameter> existingParameters = new List<SqlParameter>();
                                existingParameters.Add(new SqlParameter("@loguser", credential.UserName));

                                // Chame a stored procedure passando os parâmetros corretamente
                                Repositorio repositorio = new Repositorio();
                                DataSet dt = repositorio.SqlProcedure("spacelogin", existingParameters);

                                if (dt.Tables.Count > 0 && dt.Tables[0].Rows.Count > 0)
                                {
                                    string nomeUsuario = credential.UserName;
                                    UserModel user = GetByType(nomeUsuario);
                                    if (user != null && !IsInvalidUserType(user.Type))
                                    {
                                        int userId = Convert.ToInt32(dt.Tables[0].Rows[0]["cod_usuario"]);
                                        validUser = true;
                                    }
                                }
                            }


                        }
                    }
                }
            }

            return validUser;
        }



        public static bool IsInvalidUserType(string userType)
        {
            // Verifique se o userType é diferente de "2", "4" ou "5"
            return userType != "2" && userType != "4" && userType != "5";
        }


        public IEnumerable<UserModel> GetByAll()
        {
            throw new NotImplementedException();
        }

        public UserModel GetByUsername(string username)
        {
            UserModel user = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = ("Select * from tblUsuario where login_usuario=@loguser");
                command.Parameters.Add("@loguser", SqlDbType.VarChar).Value = username;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UserModel()
                        {
                            Id = reader["cod_usuario"].ToString(),
                            Username = reader["login_usuario"].ToString(),
                            PasswordHash = reader["senha_usuario"].ToString(),

                        };
                    }
                }
            }
            return user;
        }



        public static UserModel GetByType(string nomeUsuarioType)
        {
            string query = "SELECT cod_tipo FROM tblUsuario WHERE login_usuario = @nomeUsuario";
            string conexaoString = "Server=(local); Database=SpaceBar; Integrated Security=true";

            UserModel user = null;

            using (var connection = new SqlConnection(conexaoString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nomeUsuario", nomeUsuarioType);

                    // Execute a consulta e obtenha o Type do usuário
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        int userType = Convert.ToInt32(result);
                        user = new UserModel()
                        {
                            Type = userType.ToString(),
                        };
                    }
                }
            }
            return user;
        }

        public static UserModel GetByIcon(int userId)
        {
            UserModel user = null;
            string query = "SELECT icon_usuario FROM tblUsuario WHERE cod_usuario = @userId";
            string conexaoString = "Server=(local); Database=SpaceBar; Integrated Security=true";

            using (var connection = new SqlConnection(conexaoString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("icon_usuario"))) // Verifica se a coluna não é nula
                            {
                                byte[] imageData = (byte[])reader["icon_usuario"];
                                // Atribua diretamente os dados da imagem do perfil ao UserModel
                                user = new UserModel
                                {
                                    Icon = imageData
                                };
                            }
                        }
                    }
                }
            }

            return user;
        }
    

    }
}
    