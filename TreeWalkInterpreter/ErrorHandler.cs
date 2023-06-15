namespace Campfire.TreeWalkInterpreter;

public class ErrorHandler
{
    public enum ErrorCodes
    {
        Unknown = 1,
        NoScriptSpecified = 2
        
    }
    
    public static bool hadError = false;

    public static void Exit(ErrorCodes errorCode)
    {
        Environment.Exit((int)errorCode);
    }
    
    public static void Error(int line, string message)
    {
        Report(line, "", message);
    }

    private static void Report(int line, string where, string message)
    {
        Console.WriteLine($"[line {line}] Error {where}: {message}");
        hadError = true;
    }
}