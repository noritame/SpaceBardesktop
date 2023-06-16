using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Spacebardesktop.Models
{
        public class UserModel
        {
            public string Id { get; set; }
            public string Type { get; set; }
            public byte[] Icon { get; set; }
            public string Username { get; set; }
            public string PasswordHash { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
        }
    }
