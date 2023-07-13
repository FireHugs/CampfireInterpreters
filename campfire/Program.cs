namespace Campfire.campfire;

internal static class Program
{
    private static Dictionary<string, ICommand> commands = new();

    public delegate void ICommand(string[] args, ref MessageHandler.ExitCodes exitCode, bool printToConsole);
    
    private static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            MessageHandler.Exit(MessageHandler.ExitCodes.NoCommandSpecified, "Usage: command [param1|param2]");
        }

        BuildCommandList();

        var commandName = args[0];
        
        if (commandName == "help")
        {
            Console.WriteLine("Commands:");
            foreach (var command in commands)
            {
                Console.WriteLine($"  {command.Key}");
            }
            MessageHandler.Exit(MessageHandler.ExitCodes.Successful);
        }
        
        foreach (var command in commands)
        {
            if (command.Key == commandName)
            {
                MessageHandler.ExitCodes exitCode = MessageHandler.ExitCodes.Successful;
                
                commands[commandName](args[Range.StartAt(1)], ref exitCode, true);
                MessageHandler.Exit(exitCode);
            }
        }
    }

    private static void BuildCommandList()
    {
        commands.Add("generateAST", GenerateASTNodeDefinitionsCommand.ExecuteCommand);
        commands.Add("lex", LexCommand.ExecuteCommand);
        commands.Add("testAST", TestASTCommand.ExecuteCommand);
        commands.Add("parse", ParseCommand.ExecuteCommand);
        commands.Add("run", InterpretCommand.ExecuteCommand);
    }
}