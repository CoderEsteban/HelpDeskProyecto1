using System.Security.Cryptography;
using System.Text;

namespace HelpDeskProyecto1.Helpers
{
    public static class Seguridad
    {
        public static string GenerarHashSHA256(string texto)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes =
                    sha256.ComputeHash(Encoding.UTF8.GetBytes(texto));

                StringBuilder resultado = new StringBuilder();

                foreach (byte b in bytes)
                {
                    resultado.Append(b.ToString("X2"));
                }

                return resultado.ToString();
            }
        }
    }
}