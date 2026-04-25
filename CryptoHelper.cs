using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class CryptoHelper
{
    private static byte[] GenerateKey(string password, byte[] salt)
    {
        // Derive a 256-bit key from the password
        using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
        {
            return deriveBytes.GetBytes(32); // 32 bytes for AES-256
        }
    }

    public static void EncryptFile(string inputFile, string outputFile, string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16); // Random salt for security

        using (Aes aes = Aes.Create())
        {
            aes.Key = GenerateKey(password, salt);
            aes.GenerateIV();

            using (FileStream fsOut = new FileStream(outputFile, FileMode.Create))
            {
                // Prepend salt and IV to the file so they can be read during decryption
                fsOut.Write(salt, 0, salt.Length);
                fsOut.Write(aes.IV, 0, aes.IV.Length);

                using (CryptoStream cs = new CryptoStream(fsOut, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (FileStream fsIn = new FileStream(inputFile, FileMode.Open))
                {
                    fsIn.CopyTo(cs);
                }
            }
        }
    }

    public static void DecryptFile(string inputFile, string outputFile, string password)
    {
        using (FileStream fsIn = new FileStream(inputFile, FileMode.Open))
        {
            // 1. Read the salt (16 bytes)
            byte[] salt = new byte[16];
            fsIn.Read(salt, 0, salt.Length);

            // 2. Read the IV (16 bytes)
            byte[] iv = new byte[16];
            fsIn.Read(iv, 0, iv.Length);

            // 3. Derive the key using the password and the read salt
            byte[] key = GenerateKey(password, salt);

            // 4. Initialize AES with the key and the read IV
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                // 5. Decrypt the remaining data
                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                using (CryptoStream cs = new CryptoStream(fsIn, decryptor, CryptoStreamMode.Read))
                using (FileStream fsOut = new FileStream(outputFile, FileMode.Create))
                {
                    cs.CopyTo(fsOut);
                }
            }
        }
    }
}