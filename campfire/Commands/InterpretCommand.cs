using Campfire.campfire.Commands;
using Campfire.TreeWalkInterpreter;

namespace Campfire.campfire;

public static class InterpretCommand
{
    public static void ExecuteCommand(string[] args, ref MessageHandler.ExitCodes exitCode, bool printToConsole)
    {
        var tokens = LexCommand.ExecuteWork(args, ref exitCode);

        if (exitCode == MessageHandler.ExitCodes.Successful)
        {
            var statements = ParseCommand.ExecuteWork(tokens, ref exitCode);

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