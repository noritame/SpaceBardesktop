using Spacebardesktop.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Spacebardesktop.Models
{
    public class UserAccountModel : ViewModelBase
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public byte[] ProfilePicture {  get; set; }

    }
}
