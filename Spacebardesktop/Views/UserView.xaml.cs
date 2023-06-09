using Spacebardesktop.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Spacebardesktop.Repositories;

namespace Spacebardesktop
{
    /// <summary>
    /// Interação lógica para UserView.xam
    /// </summary>
    public partial class UserView 
    {
       
        public string CaminhoFoto = "";
        public UserView()
        {
            InitializeComponent();
            }
        public void CarregarFoto()
        {
            var openFile = new OpenFileDialog();
            openFile.Filter = "Arquivos de imagens jpg e png|*.jpg;*.png";
            openFile.Multiselect = false;

            if (openFile.ShowDialog() == DialogResult.OK)
                CaminhoFoto = openFile.FileName;

        }

        private void BtnChange_Click(object sender, RoutedEventArgs e)
        {
            CarregarFoto();
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            

        }
    }
 }

