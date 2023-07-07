namespace Campfire.TreeWalkInterpreter;

public static class ErrorHandler
{
    public static void Error(int line, string message)
    {
        Report(line, "", message);
    }

    public static void Error(Token token, string message)
    {
        if (token.Type == TokenType.EOF)
        {
            Report(token.Line, " at end", message);
        }
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