using Campfire.campfire.Commands;
using Campfire.TreeWalkInterpreter;

namespace Campfire.campfire;

public static class InterpretCommand
{
    public static void ExecuteCommand(string[] args, ref MessageHandler.ExitCodes exitCode, bool printToConsole)
    {

        var previousMs = GetTime();
        var tokens = LexCommand.ExecuteWork(args, ref exitCode);
        Console.WriteLine($"[Lexer took {GetTime() - previousMs} ms]");

        if (exitCode == MessageHandler.ExitCodes.Successful)
        {
            previousMs = GetTime();
            var statements = ParseCommand.ExecuteWork(tokens, ref exitCode);
            Console.WriteLine($"[Parser took {GetTime() - previousMs} ms]");

            if (ErrorHandler.HadError)
            {
                Console.WriteLine("Interpreter skipped due to Parsing Error");
                return;
            }
            
            var interpreter = new TreeWalkInterpreter.TreeWalkInterpreter();

            Resolver resolver = new Resolver(interpreter);

            previousMs = GetTime();
            resolver.Resolve(statements);
            Console.WriteLine($"[Resolver took {GetTime() - previousMs} ms]");
            
            if (ErrorHandler.HadError)
            {
                Console.WriteLine("Interpreter skipped due to Resolving Error");
                return;
            }

            previousMs = GetTime();
            interpreter.Interpret(statements);
            Console.WriteLine($"[Interpreter took {GetTime() - previousMs} ms]");
        }    
    }

    private static long GetTime()
    {
        return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
    }
}