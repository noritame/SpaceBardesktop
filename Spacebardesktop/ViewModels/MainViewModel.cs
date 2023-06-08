﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FontAwesome.Sharp;
using Spacebardesktop.Models;
using Spacebardesktop.Repositories;

namespace Spacebardesktop.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

        //fields 
        private UserAccountModel _currentUserAccount;
        private ViewModelBase _currentChildView;
        private string _caption;
        private IconChar _icon;


        private IUserRepository userRepository;

        public UserAccountModel CurrentUserAccount
        {
            get { return _currentUserAccount; }
            set { _currentUserAccount = value;  OnPropertyChanged(nameof(CurrentUserAccount)); }
        
        }

        public ViewModelBase CurrentChildView 
        {
            get{return _currentChildView;}
            set{ _currentChildView = value; OnPropertyChanged(nameof(CurrentChildView)); }

        }
        public string Caption
        {
            get { return _caption; }
            set { _caption = value;     
                OnPropertyChanged(nameof(Caption)); }
        }
        public IconChar Icon
        {
            get { return _icon; }
            set { _icon = value; OnPropertyChanged(nameof(Icon)); }
        }

        //comandos
        public ICommand ShowHomeViewCommand { get; }
        public ICommand ShowUserViewCommand { get; }

        public MainViewModel()
        {
           userRepository = new UserRepository();
           CurrentUserAccount = new UserAccountModel();


            //inicialização dos comandos

            ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeViewCommand);
            ShowUserViewCommand = new ViewModelCommand(ExecuteShowUserViewCommand);
            //ShowSettingsViewCommand = new ViewModelCommand(ExecuteShowSettingsViewCommand);

            //Default View
            ExecuteShowHomeViewCommand(null);
            ExecuteShowUserViewCommand(null);
            LoadCurrentUserData();
        }


        private void ExecuteShowHomeViewCommand(object obj)
        {
            CurrentChildView = new HomeViewModel();
            Caption = "Criar Post";
            Icon = IconChar.Pen;
        }
        private void ExecuteShowUserViewCommand(object obj)
        {
            CurrentChildView = new UserViewModel();
            Caption = "Configurações do usuario";
            Icon = IconChar.User;
        }

        private void LoadCurrentUserData()
        {
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                CurrentUserAccount.Username = user.Username;
                CurrentUserAccount.DisplayName = $"{user.Username}";
                CurrentUserAccount.ProfilePicture = null;
            }
            else
            {
               CurrentUserAccount.DisplayName="Usuario Invalido";

            }
        }
    }
}
 