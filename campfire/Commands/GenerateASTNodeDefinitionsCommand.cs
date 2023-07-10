﻿using Campfire.TreeWalkInterpreter;

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
            "Grouping : Expr Expression",
            "Literal : Object Value",
            "Logical : Expr left, Token Op, Expr right",
            "Variable : Token name"
        });
        
        ASTNodeDefinitionTextEmitGenerator.GenerateNodeDefinition("../../../../TreeWalkInterpreter/Scripts/Generated", "Stmt", new List<string>
        {
            "Block : List<Stmt> statements",
            "Expression : Expr expression",
            "If : Expr condition, Stmt thenBranch, Stmt elseBranch", 
            "Print : Expr expression",
            "Var : Token name, Expr initializer",
            "While : Expr condition, Stmt body"
        });
    }
}