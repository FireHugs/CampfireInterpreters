namespace Campfire.TreeWalkInterpreter;

public static class ErrorHandler
{
    public static bool HadError { get; private set; }
    
    public static void Error(int line, string message)
    {
        HadError = true;
        Report(line, "", message);
    }

    public static void Error(Token token, string message)
    {
        HadError = true;
        Report(token.Line, token.Type == TokenType.EOF ? " at end" : "", message);
    }

    public static void RuntimeError(RuntimeError error)
    {
        Report(error.Token.Line, "", error.Message);
    }

    private static void Report(int line, string where, string message)
    {
        Console.WriteLine($"[line {line}] Error {where}: {message}");
    }
}