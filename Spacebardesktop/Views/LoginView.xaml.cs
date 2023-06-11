﻿using System.Windows;
using System.Windows.Input;
using Spacebardesktop.Repositories;
using Spacebardesktop.ViewModels;

namespace Spacebardesktop
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
            
        private void btnClosed_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string nomeUsuario = txtUser.Text;
            HomeViewModel.GetById(nomeUsuario);
            string nomeUsusarioType = txtUser.Text;
            UserRepository.GetByType(nomeUsusarioType);
        }

        private void BindablePasswordBox_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}