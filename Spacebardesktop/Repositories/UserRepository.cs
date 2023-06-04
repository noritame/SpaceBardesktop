﻿using Spacebardesktop.Models;
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

        public bool AuthenticateUser(NetworkCredential credential)//verificação de usuario
        {
            bool ValidUser;
            using (var connection = GetConnection())
            using (var cmd = new SqlCommand("spacelogin", connection))
            {
                connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = connection;
                Repositorio C = new Repositorio();
                var parametro = new List<SqlParameter>
                {
                    new SqlParameter("@loguser", credential.UserName),
                    new SqlParameter("@senhauser", credential.Password)
                };
                cmd.Parameters.AddRange(parametro.ToArray());

                // Executar a consulta e verificar se há um resultado válido
                using (var reader = cmd.ExecuteReader())
                {
                    ValidUser = reader.HasRows;
                }
            }
            return ValidUser;
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
            UserModel user=null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText=("Select * from tblUsuario where login_usuario=@loguser");
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

        public UserModel GetById(int id)
        {
            string query = "SELECT cod_usuario FROM tblUsuario WHERE nome_usuario = @nomeUsuario";
            UserModel user = null;
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nomeUsuario", value:user);

                    // Execute a consulta e obtenha o ID do usuário
                    int? userId = command.ExecuteScalar() as int?;

                    // Verifique se o ID do usuário foi encontrado
                    if (userId.HasValue)
                    {
                        // Faça algo com o ID do usuário, como armazená-lo em uma variável ou propriedade
                        int idUsuario = userId.Value;
                        // ...
                    }
                    else
                    {
                        // O usuário não foi encontrado
                    }
                }
            }
            return user;
        }
    }
}
