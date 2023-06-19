using Campfire.TreeWalkInterpreter;

namespace Campfire.campfire;

public class LexCommand: ICommand
{
    public string Name => "lex";

    public void ExecuteCommand(string[] args, ref MessageHandler.ExitCodes exitCode)
    {
        var tokens = CommandHelpers.LexFile(args, ref exitCode);

        if (exitCode == MessageHandler.ExitCodes.Successful)
        {
            Console.Write("Lexed Tokens: ");
            foreach (var token in tokens)
            {
                Console.Write($" {token} |");
            }    
        }
    }
}