namespace Campfire.TreeWalkInterpreter;

public partial class Interpreter: Stmt.Visitor<object>
{
    public object VisitBlockStmt(Block stmt)
    {
        ExecuteBlock(stmt.statements, new Environment(environment));
        return null;
    }

    public object VisitExpressionStmt(Expression stmt)
    {
        var value = EvaluateExpression(stmt.expression);
        if (stmt.expression is not Assign && 
            stmt.expression is not Call)
        {
            Console.WriteLine(Stringify(value));
        }
        return null;
    }

    public object VisitFunctionStmt(Function stmt)
    {
        var function = new UserFunction(stmt);
        environment.Define(stmt.name.Lexeme, function);
        return null;
    }

    public object VisitIfStmt(If stmt)
    {
        if (IsTruthy(EvaluateExpression(stmt.condition)))
        {
            Execute(stmt.thenBranch);
        }
        else if (stmt.elseBranch != null)
        {
            Execute(stmt.elseBranch);
        }
        return null;
    }

    public object VisitPrintStmt(Print stmt)
    {
        var value = EvaluateExpression(stmt.expression);
        Console.WriteLine(Stringify(value));
        return null;
    }

    public object VisitVarStmt(Var stmt)
    {
        if (stmt.initializer != null)
        {
            environment.Define(stmt.name.Lexeme, EvaluateExpression(stmt.initializer));
        }
        else
        {
            environment.Declare(stmt.name.Lexeme);
        }

        return null;
    }

    public object VisitWhileStmt(While stmt)
    {
        while (IsTruthy(EvaluateExpression(stmt.condition)))
        {
            Execute(stmt.body);
        }

        return null;
    }

    private string Stringify(object obj)
    {
        if (obj == null) return "nil";
        if (obj is double)
        {
            var text = obj.ToString();
            if (text.EndsWith(".0"))
            {
                text = text.Substring(0, text.Length - 2);
            }

            return text;
        }

        return obj.ToString();
    }
}