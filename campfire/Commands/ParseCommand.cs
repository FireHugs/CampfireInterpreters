using Campfire.TreeWalkInterpreter;

namespace Campfire.campfire;

public static class ParseCommand
{
    public static void ExecuteCommand(string[] args, ref MessageHandler.ExitCodes exitCode, bool printToConsole)
    {
        var tokens = CommandHelpers.LexFile(args, ref exitCode);

        if (exitCode == MessageHandler.ExitCodes.Successful)
        {
            var parser = new RecursiveDescentParser(tokens);
        }    
    }
}