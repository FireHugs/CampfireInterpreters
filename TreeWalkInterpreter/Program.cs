namespace Campfire.TreeWalkInterpreter;

internal static class Program
{
    static void Main(string[] args)
    {
        if (args.Length > 1)
        {
            Console.WriteLine("Usage: Campfire.TreeWalkInterpreter [script]");
            ErrorHandler.Exit(ErrorHandler.ErrorCodes.NoScriptSpecified);
        }
        else if (args.Length == 1)
        {
            RunFile(args[0]);
            Environment.Exit(0);
        }
        // else
        // {
        //     RunPrompt();    
        // }
    }

    static void RunFile(string script)
    {
        Run(File.ReadAllText(script));
        
        if (ErrorHandler.hadError)
        {
            Environment.Exit((int)ErrorHandler.ErrorCodes.Unknown);    
        }
        else
        {
            Environment.Exit(0);
        }
    }

    static void RunPrompt()
    {
        //TODO: Implement reading from console input lines
    }

    static void Run(string source)
    {
        var lexer = new ManualLexer(source);
        var tokens = lexer.ScanTokens();

        Console.Write("Tokens: ");
        foreach (var token in tokens)
        {
            Console.Write($" {token} |");
        }
    }
}