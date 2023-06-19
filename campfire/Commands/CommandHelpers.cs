using Campfire.TreeWalkInterpreter;

namespace Campfire.campfire;

public static class CommandHelpers
{
    private static string defaultSourcePath = "../../../../TreeWalkInterpreter/Scripts/TestScript.cf";
    
    public static List<Token> LexFile(string[] args, ref MessageHandler.ExitCodes exitCode)
    {
        
        ILexer? lexer = null;

        var sourcePath = args.Length == 0 ? defaultSourcePath : args[0];
        
        string source = null;
        if (!TryGetSource(sourcePath, ref source))
        {
            MessageHandler.Error("Invalid Source Path");
            exitCode = MessageHandler.ExitCodes.InvalidArguments;
            return null;
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
                return null;
            }
        }
        lexer ??= new ManualLexer(source);

        exitCode = MessageHandler.ExitCodes.Successful;
        return lexer.ScanTokens();
    }
    
    private static bool TryGetSource(string sourcePath, ref string source)
    {
        if (!File.Exists(sourcePath))
        {
            return false;
        }

        source = File.ReadAllText(sourcePath);
        return true;
    }
}