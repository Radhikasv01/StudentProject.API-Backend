using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Student.Service
{
    public class HelperClass
    {
        private const int SaltSize = 16; // 128 bits
        private const int KeySize = 32; // 256 bits
        private const int IvSize = 16; // 128 bits
        private const int Iterations = 100000; // Increased iterations

        // Encrypts the password using a randomly generated salt
        public string EncryptPassword(string password)
        {
            // Generate a random salt
            byte[] salt = GenerateRandomSalt();
            byte[] clearBytes = Encoding.Unicode.GetBytes(password);

            using (Aes aes = Aes.Create())
            {
                using (var pdb = new Rfc2898DeriveBytes("proteam001", salt, Iterations, HashAlgorithmName.SHA256))
                {
                    aes.Key = pdb.GetBytes(KeySize);
                    aes.IV = pdb.GetBytes(IvSize);

                    using (var ms = new MemoryStream())
                    {
                        ms.Write(salt, 0, salt.Length); // Store the salt at the beginning
                        using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.FlushFinalBlock();
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }

        // Decrypts the password using the salt embedded in the encrypted string
        public string DecryptPassword(string encryptedPassword)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedPassword);

            using (Aes aes = Aes.Create())
            {
                using (var ms = new MemoryStream(encryptedBytes))
                {
                    // Extract the salt from the beginning
                    byte[] salt = GenerateRandomSalt();
                    ms.Read(salt, 0, salt.Length);

                    using (var pdb = new Rfc2898DeriveBytes("proteam001", salt, Iterations, HashAlgorithmName.SHA256))
                    {
                        aes.Key = pdb.GetBytes(KeySize);
                        aes.IV = pdb.GetBytes(IvSize);

                        using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            using (var sr = new StreamReader(cs, Encoding.Unicode))
                            {
                                return sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }

        // Generates a random salt
        private static byte[] GenerateRandomSalt()
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }

}
