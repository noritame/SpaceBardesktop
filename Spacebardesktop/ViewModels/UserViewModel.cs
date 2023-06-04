using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spacebardesktop.ViewModels
{
    public class UserViewModel : ViewModelBase

    {
        public string CaminhoFoto { get; set; }

        public byte[] Foto { get; set; }
        private string Nome{ get; set;}
        

        private byte[] GetFoto(string caminhoFoto)
        {
            byte[] foto;
            using (var stream = new FileStream(caminhoFoto, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    foto = reader.ReadBytes((int)stream.Length);
                }
            }
            return foto;
        }
        public void Salvar(UserViewModel userView)
        {
            byte[] foto = GetFoto(userView.CaminhoFoto);
            var sql = "INSERT INTO tblUsuario";
        }
    }
}

