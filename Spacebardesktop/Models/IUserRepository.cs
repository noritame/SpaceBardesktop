using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace Spacebardesktop.Models
{
    public interface IUserRepository
    {
        bool AuthenticateUser(NetworkCredential credential);

        UserModel GetByUsername(string username);
       IEnumerable<UserModel> GetByAll();
        //...
    }
}
