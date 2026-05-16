public class VltParser
{
    /// <summary>
    /// Récupére toutes les pairs de clé du fichier Vlt
    /// </summary>
    /// <param name="temp_path">Le chemin du fichier .Vlt</param>
    public static void GetKeyPairs(string temp_path)
    {
        string[] key_pairs = File.ReadAllLines(temp_path);

        for (int i = 0; i < key_pairs.Length; i++)
        {
            string formatted = key_pairs[i].Replace("=", " : ");
            Console.WriteLine(i + "." + " " + formatted);
        }
    }
}