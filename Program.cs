if (args.Length == 1) {
    Guide.ShowGuide();
    return;
}

string command = args[1];
//Console.WriteLine("SHOW COMMAND DEBUG ONLY : " + command);
CommandInit.SwitchOnCommand(command);