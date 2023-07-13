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
            "Assign : Token name, Expr value",
            "Unary : Token Op, Expr Right",
            "Binary : Expr Left, Token Op, Expr Right",
            "Call : Expr callee, Token paren, List<Expr> arguments",
            "Get : Expr obj, Token name",
            "Grouping : Expr Expression",
            "Literal : Object Value",
            "Logical : Expr left, Token Op, Expr right",
            "Set: Expr obj, Token name, Expr value",
            "Super : Token keyword, Token method",
            "This : Token keyword",
            "Variable : Token name"
        });
        
        ASTNodeDefinitionTextEmitGenerator.GenerateNodeDefinition("../../../../TreeWalkInterpreter/Scripts/Generated", "Stmt", new List<string>
        {
            "Block : List<Stmt> statements",
            "Class : Token name, Variable superclass, List<Function> methods",
            "Expression : Expr expression",
            "Function : Token name, List<Token> parameters, List<Stmt> body",
            "If : Expr condition, Stmt thenBranch, Stmt elseBranch", 
            "Print : Expr expression",
            "ReturnStatement: Token keyword, Expr value",
            "Var : Token name, Expr initializer",
            "While : Expr condition, Stmt body"
        });
    }
}