using Campfire.TreeWalkInterpreter;

namespace Campfire.campfire.Commands;

public static class ParseCommand
{
    public static void ExecuteCommand(string[] args, ref MessageHandler.ExitCodes exitCode, bool printToConsole)
    {
        var tokens = LexCommand.ExecuteWork(args, ref exitCode);

        if (exitCode == MessageHandler.ExitCodes.Successful)
        {
            var statements = ExecuteWork(tokens, ref exitCode);

            if (printToConsole && exitCode == MessageHandler.ExitCodes.Successful)
            {
                int indent = 0;
                foreach (var statement in statements)
                {
                    string currentStatement = "> ";
                    for (int i = 0; i < indent; ++i)
                    {
                        currentStatement += "  ";
                    }
                    currentStatement += $" [Statement] {statement}"; 
                    Console.WriteLine(currentStatement);
                }
                
                Console.WriteLine("> Parsing complete.");
            }
        }
    }

    public static List<Stmt> ExecuteWork(List<Token> tokens, ref MessageHandler.ExitCodes exitCode)
    {
        var parser = new RecursiveDescentParser(tokens);
        return parser.Parse();
    }
}