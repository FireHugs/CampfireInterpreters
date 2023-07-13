using Campfire.TreeWalkInterpreter;

namespace Campfire.campfire;

public static class InterpretCommand
{
    public static void ExecuteCommand(string[] args, ref MessageHandler.ExitCodes exitCode, bool printToConsole)
    {
        var tokens = LexCommand.ExecuteWork(args, ref exitCode);

        if (exitCode == MessageHandler.ExitCodes.Successful)
        {
            var parser = new RecursiveDescentParser(tokens);
            var statements = parser.Parse();

            if (ErrorHandler.HadError)
            {
                Console.WriteLine("Interpreter skipped due to Parsing Error");
                return;
            }
            
            var interpreter = new TreeWalkInterpreter.TreeWalkInterpreter();

            Resolver resolver = new Resolver(interpreter);
            resolver.Resolve(statements);
            
            if (ErrorHandler.HadError)
            {
                Console.WriteLine("Interpreter skipped due to Resolving Error");
                return;
            }
            
            interpreter.Interpret(statements);
        }    
    }
}