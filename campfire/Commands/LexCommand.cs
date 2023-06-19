using Campfire.TreeWalkInterpreter;

namespace Campfire.campfire;

public class LexCommand: ICommand
{
    public string Name => "lex";

    private readonly string defaultSourcePath = "../../../../TreeWalkInterpreter/Scripts/TestScript.cf";

    public void ExecuteCommand(string[] args, ref MessageHandler.ExitCodes exitCode)
    {
        ILexer? lexer = null;

        var sourcePath = args.Length == 0 ? defaultSourcePath : args[0];
        
        string source = null;
        if (!TryGetSource(sourcePath, ref source))
        {
            MessageHandler.Error("Invalid Source Path");
            exitCode = MessageHandler.ExitCodes.InvalidArguments;
            return;
        }
        
        if (args.Length > 1)
        {
            var lexType = args[1];
            if (lexType is "-manual" or "-m")
            {
                lexer = new ManualLexer(File.ReadAllText(sourcePath));
            }
            else
            {
                MessageHandler.Error("Invalid Lexer Type [(m)anual]");
                exitCode = MessageHandler.ExitCodes.InvalidArguments;
                return;
            }
        }
        lexer ??= new ManualLexer(source);

        var tokens = lexer.ScanTokens();

        Console.Write("Lexed Tokens: ");
        foreach (var token in tokens)
        {
            Console.Write($" {token} |");
        }
    }

    private bool TryGetSource(string sourcePath, ref string source)
    {
        if (!File.Exists(sourcePath))
        {
            return false;
        }

        source = File.ReadAllText(sourcePath);
        return true;
    }
}