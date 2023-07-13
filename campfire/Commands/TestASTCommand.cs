using Campfire.TreeWalkInterpreter;
using Campfire.TreeWalkInterpreter.AST_Visitors;

namespace Campfire.campfire;

public static class TestASTCommand
{
    public static void ExecuteCommand(string[] args, ref MessageHandler.ExitCodes exitCode, bool printToConsole)
    {
        var expression = new Binary(
            new Unary(
                new Token(TokenType.Minus, "-", null, 1),
                new Literal(123)
            ),
            new Token(TokenType.Star, "*", null, 1),
            new Grouping(new Literal(45.67))
        );
        
        Console.WriteLine("=AST Printer Test=");
        Console.WriteLine("Expected Line: (* (- 123) (group 45.67))");
        Console.WriteLine($"Processed Line: {new AstPrinter().Print(expression)}");
    }
}