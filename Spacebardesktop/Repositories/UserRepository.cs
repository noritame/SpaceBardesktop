using Spacebardesktop.Models;
using Spacebardesktop.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Spacebardesktop.Repositories
{
    public class UserRepository : Repositorio, IUserRepository
    {
        private string _errorManage;
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
            int userId;
            int userType;
            bool validUser = false;

            using (var connection = GetConnection())
            using (var cmd = new SqlCommand("spacelogin", connection))
            {
                connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                var parametro = new List<SqlParameter>
        {
            new SqlParameter("@loguser", credential.UserName),
            new SqlParameter("@senhauser", credential.Password)
        };
                cmd.Parameters.AddRange(parametro.ToArray());

                // Executar a consulta e verificar se há um resultado válido
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string nomeUsuario = credential.UserName;
                        UserModel user = GetByType(nomeUsuario);

                        if(user != null && !IsInvalidUserType(user.Type)) 
                        { 
                        userId = Convert.ToInt32(reader["cod_usuario"]);
                        validUser = true;
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
        public void Edit(UserModel userModel)
        {
            throw new NotImplementedException();
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
                            Username = reader["login_usuario"].ToString(),
                            Password = string.Empty

                        };
                    }
                }
            }
            return user;
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
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
    }
}
    