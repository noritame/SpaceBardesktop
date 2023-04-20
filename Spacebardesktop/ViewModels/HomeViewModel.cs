using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Spacebardesktop.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private string _title;
        private string _description;

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));

                }
            }
            public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public ICommand criarPost {get; }
    
    }
    //public HomeViewModel() {

    //}
}
