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
        try
        {
            using (FileStream fsIn = new FileStream(inputFile, FileMode.Open))
            {
                byte[] salt = new byte[16];
                fsIn.Read(salt, 0, salt.Length);

                byte[] iv = new byte[16];
                fsIn.Read(iv, 0, iv.Length);

                byte[] key = GenerateKey(password, salt);

                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;
                    // Ensure padding is on (it is by default)
                    aes.Padding = PaddingMode.PKCS7;

                    using (ICryptoTransform decryptor = aes.CreateDecryptor())
                    using (FileStream fsOut = new FileStream(outputFile, FileMode.Create))
                    {
                        using (CryptoStream cs = new CryptoStream(fsIn, decryptor, CryptoStreamMode.Read))
                        {
                            cs.CopyTo(fsOut);
                        }
                    }
                }
            }
        }
        catch (CryptographicException)
        {
            // Cleanup the garbage file created by the failed decryption
            if (File.Exists(outputFile)) File.Delete(outputFile);
            throw new Exception("Incorrect Password.");
        }
    }

    public static string RequestPassword(bool vault_exist)
    {
        string? vault_key;
        do
        {
            if (vault_exist)
            {
                Console.WriteLine("Please Enter Your Password :");
            }
            else
            {
                Console.WriteLine("Please setup a password for your vault (3 characters minimum) :");
            }

            vault_key = Console.ReadLine();
        }
        while (string.IsNullOrEmpty(vault_key) || vault_key.Length < 3);

        return vault_key;
    }
}