using System.Security.Cryptography;
using System.Text;

public class CommandActions
{
    /// <summary>
    /// Génére un mot de passe sécurisé pour le Vault
    /// </summary>
    /// <param name="length"></param>
    /// <returns>Mot De Passe</returns>
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

    /// <summary>
    /// Ajoute une entrée au Vault 
    /// </summary>
    public static void Add()
    {
        string vault_path = @"C:\Users\rayan\Documents\Programming Projects\Vaultio-Project\vault.vlt";
        bool vault_exist = File.Exists(vault_path);
        string temp_path = vault_path + ".tmp";

        string? vault_key = CryptoHelper.RequestPassword(vault_exist);

        if (!vault_exist)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Info : Vault will be created");
            Console.ForegroundColor = ConsoleColor.White;
        }
        else
        {
            CryptoHelper.DecryptFile(vault_path, temp_path, vault_key);
        }

        string? password_source, password_value;
        do
        {
            Console.WriteLine("Please enter the source of the password :");
            password_source = Console.ReadLine();

            Console.WriteLine("Please enter your password for this source :");
            password_value = Console.ReadLine();
        }
        while (string.IsNullOrEmpty(password_source) || string.IsNullOrEmpty(password_value));

        File.AppendAllText(vault_path, password_source + "=" + password_value + "\n");

        CryptoHelper.EncryptFile(vault_path, temp_path, vault_key);
        File.Delete(vault_path);
        File.Move(temp_path, vault_path);
    }

    /// <summary>
    /// Récupére la liste compléte de mot de passe
    /// </summary>
    public static void ListAll()
    {
        string vault_path = @"C:\Users\rayan\Documents\Programming Projects\Vaultio-Project\vault.vlt";
        bool vault_exist = File.Exists(vault_path);
        string temp_path = vault_path + ".tmp";

        if (!vault_exist)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No vault configured, please run the 'add' command to add a vault.");
            return;
        }

        CryptoHelper.DecryptFile(vault_path, temp_path, null);
    }
}