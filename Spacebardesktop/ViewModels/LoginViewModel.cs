using Spacebardesktop.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;




namespace Spacebardesktop.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        //Campos
        private string _username;
        private SecureString _password;
        private string _errorManage;
        private bool _isViewVisible=true;
        private UserRepository userRepository;  

        //Propriedades
        public string Username {

            get { 
                
                return _username; 
            
            }
            
            set { 
            
                _username = value;
                OnPropertyChanged(nameof(Username));

            } 
        
        }
        public SecureString Password {

            get
            {

                return _password;

            }

            set
            {

                _password = value;
                OnPropertyChanged(nameof(Password));

            }

        }
        public string ErrorManage {

            get
            {

                return _errorManage;

            }

            set
            {

                _errorManage = value;
                OnPropertyChanged(nameof(ErrorManage));

            }

        }
        public bool IsViewVisible {

            get
            {

                return _isViewVisible;

            }

            set
            {

                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));

            }

        }

        //Comandos
        public ICommand LoginCommand{get;}
        public ICommand ShowPasswordCommand {get;}
        
        //Construtor
        public LoginViewModel()
        {
            userRepository = new UserRepository();
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
           

        }

        private bool CanExecuteLoginCommand(object obj)//codigo de verificaçao de usuario.
        {

            bool validData;
            if (string.IsNullOrEmpty(Username) || Username.Length < 3 || Password == null || Password.Length < 3)// Nome do usuario e senha com menos de 3 caracteres não serão aceitos.
                validData = false;
            else
                validData = true;
            return validData;
        }

        private void ExecuteLoginCommand(object obj)
        {
  
            var isValidUser = userRepository.AuthenticateUser(new NetworkCredential(Username,Password));
            if (isValidUser)// usuario valido
               
            {
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Username),null );
                IsViewVisible = false;
            }
            else// usuario invalido
            {
                ErrorManage = "* Invalid username or password";
            }
        }
    }
}
