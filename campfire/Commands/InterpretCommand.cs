using Campfire.TreeWalkInterpreter;
using Campfire.TreeWalkInterpreter.AST_Visitors;

namespace Campfire.campfire;

public class InterpretCommand: ICommand
{
    public string Name => "run";
    
    public void ExecuteCommand(string[] args, ref MessageHandler.ExitCodes exitCode)
    {
        var tokens = CommandHelpers.LexFile(args, ref exitCode);

        if (exitCode == MessageHandler.ExitCodes.Successful)
        {
            var parser = new RecursiveDescentParser(tokens);
            var expression = parser.Parse();

            Console.WriteLine("Expression: " + new AstPrinter().Print(expression));
            
            var interpreter = new Interpreter();
            interpreter.Interpret(expression);
            
            
        }    
    }
}