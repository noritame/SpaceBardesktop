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
            using (var connection = GetConnection())
            {
                using (var cmd = new SqlCommand("spacelogin", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Utilizar parâmetros nomeados
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@loguser", Value = credential.UserName });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@senhauser", Value = credential.Password });

                    connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            UpdatePasswords();
                            string nomeUsuario = credential.UserName;
                            string senhaUsuarioFromDatabase = reader["senha_usuario"].ToString();
                            if (!string.IsNullOrEmpty(senhaUsuarioFromDatabase) && senhaUsuarioFromDatabase.Length >= 60)
                            {
                                bool senhaCorreta = BCrypt.Net.BCrypt.Verify(credential.Password, senhaUsuarioFromDatabase);

                                if (senhaCorreta)
                                {
                                    UserModel user = GetByType(nomeUsuario);

                                    if (user != null && !IsInvalidUserType(user.Type))
                                    {
                                        int userId = Convert.ToInt32(reader["cod_usuario"]);
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



        private void UpdatePasswords()
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                using (var selectCmd = new SqlCommand("SELECT cod_usuario, senha_usuario FROM tblUsuario", connection))
                using (var reader = selectCmd.ExecuteReader())
                {
                    List<Tuple<int, string>> passwordsToUpdate = new List<Tuple<int, string>>();

                    while (reader.Read())
                    {
                        int userId = reader.GetInt32(0);
                        string plainPassword = reader.GetString(1);
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);
                        if (hashedPassword.Length > 100) // Verifique o tamanho máximo permitido para a coluna senha_usuario
                        {
                            // Reduzir o tamanho da senha criptografada
                            hashedPassword = hashedPassword.Substring(0, 100);
                        }

                        passwordsToUpdate.Add(new Tuple<int, string>(userId, hashedPassword));
                    }

                    reader.Close();

                    foreach (var passwordTuple in passwordsToUpdate)
                    {
                        int userId = passwordTuple.Item1;
                        string hashedPassword = passwordTuple.Item2;

                        using (var updateCmd = new SqlCommand("UPDATE tblUsuario SET senha_usuario = @hashedPassword WHERE cod_usuario = @userId", connection))
                        {
                            updateCmd.Parameters.AddWithValue("@hashedPassword", hashedPassword);
                            updateCmd.Parameters.AddWithValue("@userId", userId);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        //private void AdicionarIconeAoUsuario(int userId, string caminhoIcone)
        //{
        //    byte[] iconBytes = File.ReadAllBytes(caminhoIcone);
        //    using (var connection = GetConnection())
        //    using (var cmd = new SqlCommand("UPDATE tblUsuario SET icon_usuario = @icon WHERE cod_usuario = @userId", connection))
        //    {
        //        cmd.Parameters.Add("@icon", SqlDbType.Image).Value = iconBytes;
        //        cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
        //        connection.Open();
        //        cmd.ExecuteNonQuery();
        //    }
        //}

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
                            Id = reader["cod_usuario"].ToString(),
                            Username = reader["login_usuario"].ToString(),
                            PasswordHash = reader["senha_usuario"].ToString(),

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
    