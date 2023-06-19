using System.Text;

namespace Campfire.campfire;

internal static class Program
{
    private static List<ICommand> commandList = new();
    
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
            foreach (var command in commandList)
            {
                Console.WriteLine($"  {command.Name}");
            }
            MessageHandler.Exit(MessageHandler.ExitCodes.Successful);
        }
        
        foreach (var command in commandList)
        {
            if (command.Name == commandName)
            {
                MessageHandler.ExitCodes exitCode = MessageHandler.ExitCodes.Successful;
                
                command.ExecuteCommand(args[Range.StartAt(1)], ref exitCode);
                MessageHandler.Exit(exitCode);
            }
        }
    }

    private static void BuildCommandList()
    {
        commandList.Add(new GenerateASTNodeDefinitionsCommand());
        commandList.Add(new LexCommand());
    }
}