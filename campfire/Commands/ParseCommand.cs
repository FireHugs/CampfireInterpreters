using Campfire.TreeWalkInterpreter;
using Campfire.TreeWalkInterpreter.AST_Visitors;

namespace Campfire.campfire;

public class ParseCommand: ICommand
{
    public string Name => "parse";
    
    public void ExecuteCommand(string[] args, ref MessageHandler.ExitCodes exitCode)
    {
        var tokens = CommandHelpers.LexFile(args, ref exitCode);

        if (exitCode == MessageHandler.ExitCodes.Successful)
        {
            var parser = new RecursiveDescentParser(tokens);
        }    
    }
}