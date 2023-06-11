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
            int userId;
            bool validUser = false;
            DataTable dt = new DataTable();

            using (var connection = GetConnection())
            {
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

                    using (var reader = cmd.ExecuteReader())
                    {
                        UpdatePasswords();
                        dt.Clear();
                        dt.Load(reader);
                        if (dt.DefaultView.Count == 1)

                        {
                            string nomeUsuario = credential.UserName;
                            string senhaUsuario = credential.Password;
                            string senhaUsuarioFromDatabase = dt.Rows[0]["senha_usuario"].ToString();
                            bool senhaCorreta = false;
                            if(!string.IsNullOrEmpty(senhaUsuarioFromDatabase) && senhaUsuarioFromDatabase.Length >= 60)
                            {
                                senhaCorreta = BCrypt.Net.BCrypt.Verify(senhaUsuario, senhaUsuarioFromDatabase);
                            }
                            if (senhaCorreta)
                            {
                                UserModel user = GetByType(nomeUsuario);

                                if (user != null && !IsInvalidUserType(user.Type))
                                {
                                    userId = Convert.ToInt32(dt.Rows[0]["cod_usuario"]);
                                    validUser = true;
                                }
                            } 
                        }
                    }
                }
            }

            return validUser;
        }


        private bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
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
                        Console.WriteLine($"Password before hashing: {plainPassword}");
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);
                        Console.WriteLine($"Password after hashing: {hashedPassword}");
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
    