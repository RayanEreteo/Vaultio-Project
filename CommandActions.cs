using System.Security.Cryptography;
using System.Text;
using System.IO;

public class CommandActions
{
    /// <summary>
    /// Génére un mot de passe sécurisé pour le Vault
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string GeneratePassword(int length = 20)
    {
        const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_-+=[{]};:>|./?";

        StringBuilder password = new();

        for (int i = 0; i < length; i++)
        {
            int randomIndex = RandomNumberGenerator.GetInt32(validChars.Length);
            password.Append(validChars[randomIndex]);
        }

        return password.ToString();
    }

    // TODO : A SUPPRIMER
    public static void CreateVault()
    {
        string vault_path = @"C:\Users\rayan\Documents\Programming Projects\Vaultio-Project\vault.vlt";
        string temp_path = vault_path + ".tmp";

        if (File.Exists(vault_path))
        {
            Console.WriteLine("=== VAULT ALREADY EXISTS ===");
            return;
        }

        string? password;
        do
        {
            Console.WriteLine("Please Enter Your Password (3 characters minimum) : ");
            password = Console.ReadLine();
        }
        while (string.IsNullOrEmpty(password) || password.Length < 3);

        File.WriteAllText(temp_path, "VaultInitialized=True\n");

        try
        {
            CryptoHelper.EncryptFile(temp_path, vault_path, password);
            File.Delete(temp_path);
            Console.WriteLine("Vault created and encrypted successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error encrypting vault: {ex.Message}");
        }
    }

    /// <summary>
    /// Ajoute une entrée au Vault
    /// </summary>
    public static void Add()
    {
        string vault_path = @"C:\Users\rayan\Documents\Programming Projects\Vaultio-Project\vault.vlt";
        bool vault_exist = File.Exists(vault_path);
        string temp_path = vault_path + ".tmp";

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

        if (!vault_exist)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Info : Vault will be created");
            Console.ForegroundColor = ConsoleColor.White;
        }

        string? password_source, password_value;
        do
        {
            Console.WriteLine("Please enter the source of the password :");
            password_source = Console.ReadLine();

            Console.WriteLine("Please enter your password for this source :");
            password_value = Console.ReadLine();
        }
        while (string.IsNullOrEmpty(password_source) && string.IsNullOrEmpty(password_value));

        Console.WriteLine(password_value);
        Console.WriteLine(password_source);
    }
}