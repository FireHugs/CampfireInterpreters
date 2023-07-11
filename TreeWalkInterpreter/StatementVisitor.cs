namespace Campfire.TreeWalkInterpreter;

public partial class Interpreter: Stmt.Visitor<object>
{
    public object VisitBlockStmt(Block stmt)
    {
        ExecuteBlock(stmt.statements, new Environment(environment));
        return null;
    }

    public object VisitClassStmt(Class stmt)
    {
        environment.Define(stmt.name.Lexeme, null);

        var methods = new Dictionary<string, RuntimeFunction>();
        foreach (var method in stmt.methods)
        {
            var function = new RuntimeFunction(method, environment);
            methods[method.name.Lexeme] = function;
        }
        
        var classDefinition = new ClassDefinition(stmt.name.Lexeme, methods);
        environment.AssignTokenValue(stmt.name, classDefinition);
        return null;
    }

    public object VisitExpressionStmt(Expression stmt)
    {
        var value = EvaluateExpression(stmt.expression);
        if (stmt.expression is not Assign && 
            stmt.expression is not Call &&
            stmt.expression is not Get &&
            stmt.expression is not Set)
        {
            Console.WriteLine(Stringify(value));
        }
        return null;
    }

    public object VisitFunctionStmt(Function stmt)
    {
        var function = new RuntimeFunction(stmt, environment);
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

    public object VisitReturnStatementStmt(ReturnStatement stmt)
    {
        object value = null;
        if (stmt.value != null) 
            value = EvaluateExpression(stmt.value);

        throw new Return(value);
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

    public string Stringify(object obj)
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