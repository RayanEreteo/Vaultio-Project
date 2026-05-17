public class VltParser
{
    /// <summary>
    /// Récupére toutes les pairs de clé du fichier Vlt
    /// </summary>
    /// <param name="temp_path">Le chemin du fichier .Vlt décrypté</param>
    public static void GetKeyPairs(string temp_path)
    {
        string[] key_pairs = File.ReadAllLines(temp_path);

        for (int i = 0; i < key_pairs.Length; i++)
        {
            string formatted = key_pairs[i].Replace("=", " : ");
            Console.WriteLine((i + 1) + "." + " " + formatted);
        }
    }

    public static string GetKeyValue(string value, string temp_path)
    {
        string[] key_pairs = File.ReadAllLines(temp_path);
        string found_value = "";

        for (int i = 0; i < key_pairs.Length; i++)
        {
            string[] current = key_pairs[i].Split("=");

            if (current[0] == value)
            {
                found_value = current[1];
                break;
            }
        }

        if (string.IsNullOrEmpty(found_value))
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.WriteLine("No password with this source name found.");
            Console.BackgroundColor = ConsoleColor.White;
        }

        return found_value;
    }
}