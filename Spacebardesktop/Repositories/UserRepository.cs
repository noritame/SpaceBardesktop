using Spacebardesktop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Spacebardesktop.Repositories
{
    public class UserRepository : Repositorio, IUserRepository
    {
        public void Add(UserModel userModel)
        {
            throw new NotImplementedException();
        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool ValidUser;
            using (var connection = GetConnection())
                using(var command=new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select *from [tblUsuario] where login_usuario=@login_usuario and [senha_usuario]=@senha_usuario";
                command.Parameters.Add("@login_usuario", SqlDbType.NVarChar).Value = credential.UserName;
                command.Parameters.Add("@senha_usuario", SqlDbType.NVarChar).Value = credential.Password;
                ValidUser = command.ExecuteScalar() == null ? false : true;
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
                command.CommandText = "select *from [tblUsuario] where login_usuario=@login_usuario";
                command.Parameters.Add("@login_usuario", SqlDbType.NVarChar).Value = username;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UserModel()
                        {
                            Username = reader[1].ToString(),
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
    }
}
