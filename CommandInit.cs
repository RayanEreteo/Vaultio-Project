public class CommandInit
{
    /// <summary>
    /// Lecture de la commande pour savoir quelle action entreprendre.
    /// </summary>
    /// <param name="command"></param>
    public static void SwitchOnCommand(string command)
    {
        switch (command)
        {
            case "generate-password":
                string password = CommandActions.GeneratePassword();
                Console.WriteLine(password);
                break;
            case "setup-vault":
                CommandActions.CreateVault();
                break;
            case "add":
                CommandActions.Add();
                break;
        }
    }
}