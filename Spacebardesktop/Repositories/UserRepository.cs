using Spacebardesktop.Models;
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
        public void Add(UserModel userModel)
        {
            throw new NotImplementedException();
        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool ValidUser;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                Repositorio C = new Repositorio();
                var parametro = new List<SqlParameter>();
                parametro.Add(new SqlParameter("@loguser", credential.UserName));
                parametro.Add(new SqlParameter("@senhauser", credential.Password));
                ValidUser = C.sqlProcedure("spacelogin", parametro) == null ? false : true;
                //DataSet data = new DataSet();
                // SqlCommand cmd = new SqlCommand("space_login", connection);
                command.CommandType = CommandType.StoredProcedure;
                // cmd.Parameters.Add("@loguser", SqlDbType.NVarChar).Value = credential.UserName;
                // cmd.Parameters.Add("@senhauser", SqlDbType.NVarChar).Value = credential.Password;
                //SqlDataReader reader = cmd.ExecuteReader();
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
                command.CommandText = "select * from tblUsuario where login_usuario=@username";               
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
                
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
