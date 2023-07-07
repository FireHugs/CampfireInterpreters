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
        if (stmt.expression is not Assign)
        {
            Console.WriteLine(Stringify(value));
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
        object value = null;
        if (stmt.initializer != null)
        {
            value = EvaluateExpression(stmt.initializer);
        }
        
        environment.Define(stmt.name.Lexeme, value);
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