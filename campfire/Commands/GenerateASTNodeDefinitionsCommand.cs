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
            "Unary : Token Op, Expr Right",
            "Binary : Expr Left, Token Op, Expr Right",
            "Grouping : Expr Expression",
            "Literal : Object Value"
        });
        
        ASTNodeDefinitionTextEmitGenerator.GenerateNodeDefinition("../../../../TreeWalkInterpreter/Scripts/Generated", "Stmt", new List<string>
        {
            "Expression : Expr expression",
            "Print : Expr expression"
        });
    }
}