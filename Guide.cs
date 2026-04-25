/// <summary>
/// Affiche le guide du CLI
/// </summary>
public class Guide
{
    public static void ShowGuide()
    {
        // Set colors for a professional look
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=== PASSWORD MANAGER COMMANDS ===\n");
        Console.ResetColor();

        string guide = @"
  MAIN COMMANDS
    vault <KEY>        : Access the vault
    delete-vault <KEY> : Delete the vault
    list               : List all saved entries
    add                : Add a new entry

  SECONDARY COMMANDS
    get <name>         : Get a specific password
    generate-password  : Generate a secure password
    ghost-typing <Y/N> : Toggle input masking
";
        Console.WriteLine(guide);
    }
}