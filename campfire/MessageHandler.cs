namespace Campfire.campfire;

public class MessageHandler
{
    public enum ExitCodes
    {
        Successful,
        Unknown = 1,
        NoCommandSpecified = 2,
        InvalidArguments = 3,
        ExecutionFailed = 4
    }
    
    public static bool hadError = false;

    public static void Exit(ExitCodes exitCode, string errorMessage = "")
    {
        if(errorMessage.Length > 0)
        {
            Console.WriteLine(errorMessage);
        }
        Environment.Exit((int)exitCode);
    }
    
    public static void Error(string message)
    {
        Console.WriteLine($"[Error] {message}");
        hadError = true;
    }

    public static void Warning(string message)
    {
        Console.WriteLine($"[Warning] {message}");
    }
}