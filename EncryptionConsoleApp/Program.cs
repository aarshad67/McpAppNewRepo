using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace EncryptionConsoleApp
{
    class Program
    {
        private const string hash = @"foxle@rn";
        
        static void Main(string[] args)
        {
            string inputtedPwd = "";
            string decryptedPwd = "";
            string encryptedPwd = "";
            Console.WriteLine("Testing Web Encryption/Decryption");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Enter decrypted pwd : ");
            inputtedPwd = Console.ReadLine();
            encryptedPwd = WebEncrypt(inputtedPwd);
            Console.WriteLine($"Encrypted pwd : {encryptedPwd} ");
            Console.WriteLine($"Decrypting {encryptedPwd} .............. ");
            decryptedPwd = WebDecrypt(encryptedPwd);
            Console.WriteLine($"Decrypted pwd : {decryptedPwd} ");
            Console.ReadKey();
        }

        private static string WebEncrypt(string decryptedPwd)
        {
            try
            {
                byte[] data = UTF8Encoding.UTF8.GetBytes(decryptedPwd);
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                    using (TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                    {
                        ICryptoTransform transform = tripleDes.CreateEncryptor();
                        byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                        return Convert.ToBase64String(results);
                    }
                }
            }
            catch (Exception ex)
            {
                return $"ERROR - {ex.InnerException.ToString()}";
            }

        }

        private static string WebDecrypt(string encryptedPwd)
        {
            try
            {
                byte[] data = Convert.FromBase64String(encryptedPwd);
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                    using (TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                    {
                        ICryptoTransform transform = tripleDes.CreateDecryptor();
                        byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                        return UTF8Encoding.UTF8.GetString(results);
                    }
                }
            }
            catch (Exception ex)
            {
                return $"ERROR - {ex.InnerException.ToString()}";
            }
        }
    }
}
