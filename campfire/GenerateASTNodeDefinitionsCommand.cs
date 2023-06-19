using Campfire.TreeWalkInterpreter;

namespace Campfire.campfire;

public class GenerateASTNodeDefinitionsCommand: ICommand
{
    public string Name => "generateAST";

    public void ExecuteCommand(string[] args, ref MessageHandler.ExitCodes exitCode)
    {
        if (args.Length > 0)
        {
            exitCode = MessageHandler.ExitCodes.InvalidArguments;
            return;
        }
        
        ASTNodeDefinitionTextEmitGenerator.GenerateNodeDefinition("../../../../TreeWalkInterpreter/Scripts/Generated", "Expr", new List<string>
        {
            "Unary : Token op, Expr right",
            "Binary : Expr left, Token op, Expr right",
            "Grouping : Expr expression",
            "Literal : Object value"
        });
    }
}