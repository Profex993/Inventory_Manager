using System.Security.Cryptography;
using System.Text;

//utility class for saving password into a file
namespace Inventory_Manager.Utils
{
    class PasswordStorage
    {
        public static string filePath = "./credentials.json";

        /// <summary>
        /// encrypt plaintet password
        /// </summary>
        /// <param name="plainText">plaintext password</param>
        /// <returns></returns>
        public static string Encrypt(string plainText)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedBytes = ProtectedData.Protect(plainBytes, null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedBytes);
        }

        /// <summary>
        /// decrypt encrypted password
        /// </summary>
        /// <param name="encryptedText">password hash</param>
        /// <returns>plaintext password</returns>
        public static string Decrypt(string encryptedText)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] decryptedBytes = ProtectedData.Unprotect(encryptedBytes, null, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}
