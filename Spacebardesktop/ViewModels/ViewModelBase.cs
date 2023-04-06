using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Spacebardesktop.ViewModels
{
    /* aqui eu coloquei ele como classe abstrata para funcionar somente como herança e adicionei INotifyProperty para 
    trocar a interface, e reavaliar as variaveis que ele colocou (username & password) */
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
