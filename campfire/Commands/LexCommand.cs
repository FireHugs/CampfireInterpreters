namespace Campfire.campfire;

public static class LexCommand
{
    public static void ExecuteCommand(string[] args, ref MessageHandler.ExitCodes exitCode, bool printToConsole)
    {
        var tokens = CommandHelpers.LexFile(args, ref exitCode);

        if (printToConsole && exitCode == MessageHandler.ExitCodes.Successful )
        {
            Console.Write("> Lexed Tokens: ");
            foreach (var token in tokens)
            {
                Console.Write($" [{token}]");
            }
        }
    }
}