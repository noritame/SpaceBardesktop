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
                command.CommandText = "select *from [tblAdm] where Nome=@Nome and [Senha]=@Senha";
                command.Parameters.Add("@Nome", SqlDbType.NVarChar).Value = credential.UserName;
                command.Parameters.Add("@Senha", SqlDbType.NVarChar).Value = credential.Password;
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
                command.CommandText = "select *from [tblAdm] where Nome=@Nome";
                command.Parameters.Add("@Nome", SqlDbType.NVarChar).Value = username;
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
