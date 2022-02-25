using System.Security.Cryptography;
using System.Text;

namespace Frwk.Bootcamp.QuickWait.ProfileUsers.Application.Helpers
{
    public static class UserHelper
    {
        public static string GenerateHashMd5(string password)
        {
            MD5 md5Hash = MD5.Create();

            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            StringBuilder stringBuilder = new();

            for (int i = 0; i < data.Length; i++)
            {
                stringBuilder.Append(data[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}
