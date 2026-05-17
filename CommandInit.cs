public class CommandInit
{
    /// <summary>
    /// Lecture de la commande pour savoir quelle action entreprendre.
    /// </summary>
    /// <param name="command"></param>
    public static void SwitchOnCommand(string command)
    {
        string[] userArgs = Environment.GetCommandLineArgs()
        .SkipWhile(arg => arg.EndsWith(".exe") || arg.EndsWith(".dll"))
        .ToArray();

        switch (command)
        {
            case "generate-password":
                string password = CommandActions.GeneratePassword();
                Console.WriteLine(password);
                break;
            case "add":
                CommandActions.Add();            
                break;
            case "list":
                CommandActions.ListAll();
                break;
            case "get":
                CommandActions.GetSolo(userArgs);
                break;
        }
    }
}