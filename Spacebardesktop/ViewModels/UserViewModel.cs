using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Spacebardesktop.Models;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Spacebardesktop.ViewModels
{
    public class UserViewModel : ViewModelBase

    {
        public string CaminhoFoto { get; set; }

        public byte[] Foto { get; set; }
        private string Nome { get; set; }

        public byte[] GetFoto(string caminhoFoto)
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

        public void Salvar(MainViewModel mainViewModel)
        {
            if (!string.IsNullOrEmpty(CaminhoFoto))
            {
                byte[] foto = GetFoto(CaminhoFoto);

                UserModel user = HomeViewModel.GetById(Nome);
                if (user != null)
                {
                    int userId = Convert.ToInt32(user.Id);
                    AdicionarImagemAoUsuarioLogado(userId, foto);

                    // Atualizar a imagem na propriedade ProfilePicture do MainViewModel
                    mainViewModel.CurrentUserAccount.ProfilePicture = ConvertImageSourceToByteArray(ByteArrayToImage(foto));
                }
                else
                {
                    // Lidar com a situação em que o usuário não foi encontrado
                    Console.WriteLine("Usuário não encontrado.");
                }
            }
            else
            {
                // Lidar com o caso em que o caminho da foto está nulo ou vazio
                Console.WriteLine("O caminho da foto não foi especificado.");
            }
        }
        private byte[] ConvertImageSourceToByteArray(ImageSource imageSource)
        {
            if (imageSource == null)
                return null;

            var encoder = new JpegBitmapEncoder(); // Pode ser necessário usar um codificador diferente dependendo do tipo de imagem
            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)imageSource));

            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                return stream.ToArray();
            }
        }
       
       

        public ImageSource ByteArrayToImage(byte[] byteArray)
        {
            if (byteArray == null)
                return null;

            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
                image.Freeze(); // Importante para evitar vazamento de memória
                return image;
            }
        }

        public ImageSource LoadIconImage(string iconPath)
        {
            if (!string.IsNullOrEmpty(iconPath) && File.Exists(iconPath))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(iconPath, UriKind.RelativeOrAbsolute);
                bitmap.EndInit();
                return bitmap;
            }
            return null;
        }

        public void AdicionarImagemAoUsuarioLogado(int userId, byte[] foto)
        {
            // Faça a inserção/atualização do ícone 

            string conexaoString = "Server=(local); Database=SpaceBar; Integrated Security=true";
            string sql = "UPDATE tblUsuario SET icon_usuario = @imagem WHERE cod_usuario = @userId";

            using (var con = new SqlConnection(conexaoString))
            {
                con.Open();
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.Add("@imagem", SqlDbType.Image, foto.Length).Value = foto;
                    cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
