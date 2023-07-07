using System.Transactions;

namespace Campfire.TreeWalkInterpreter;

public class Interpreter: Expr.Visitor<object>, Stmt.Visitor<object>
{
    public void Interpret(List<Stmt> statements)
    {
        try
        {
            foreach(var statement in statements)
            {
                Execute(statement);
            }
        }
        catch (RuntimeError e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public object VisitUnaryExpr(Unary expr)
    {
        var right = Evaluate(expr.Right);

        switch (expr.Op.Type)
        {
            case TokenType.Minus:
                CheckNumberOperand(expr.Op, right);
                return -(double)right;
            case TokenType.Bang:
                return !IsTruthy(right);
        }

        return null;
    }

    public object VisitBinaryExpr(Binary expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        switch (expr.Op.Type)
        {
            case TokenType.Minus:
                CheckNumberOperands(expr.Op, left, right);
                return (double)left - (double)right;
            case TokenType.Slash:
                CheckNumberOperands(expr.Op, left, right);
                return (double)left / (double)right;
            case TokenType.Star:
                CheckNumberOperands(expr.Op, left, right);
                return (double)left * (double)right;
            case TokenType.Plus:
                if (left is double ld && right is double rd) return ld + rd;
                if (left is string ls && right is string rs) return ls + rs;

                throw new RuntimeError(expr.Op, "Operands must be two numbers or two strings");
                break;
            case TokenType.Greater:
                CheckNumberOperands(expr.Op, left, right);
                return (double)left > (double)right;
            case TokenType.GreaterEqual:
                CheckNumberOperands(expr.Op, left, right);
                return (double)left >= (double)right;
            case TokenType.Less:
                CheckNumberOperands(expr.Op, left, right);
                return (double)left < (double)right;
            case TokenType.LessEqual:
                CheckNumberOperands(expr.Op, left, right);
                return (double)left <= (double)right;
            case TokenType.BangEqual:
                CheckNumberOperands(expr.Op, left, right);
                return !IsEqual(left, right);
            case TokenType.EqualEqual:
                CheckNumberOperands(expr.Op, left, right);
                return IsEqual(left, right);
        }

        throw new RuntimeError(expr.Op, "Unsupported operator");
    }

    public object VisitGroupingExpr(Grouping expr)
    {
        return Evaluate(expr.Expression);
    }

    public object VisitLiteralExpr(Literal expr)
    {
        return expr.Value;
    }

    public object VisitVariableExpr(Variable expr)
    {
        throw new NotImplementedException();
    }

    private Object Evaluate(Expr expression)
    {
        return expression.Accept(this);
    }
    
    private bool IsTruthy(Object obj)
    {
        if (obj == null) return false;
        if (obj is bool b) return b;
        return true;
    }

    private bool IsEqual(object a, object b)
    {
        if (a == null && b == null) return true;
        if (a == null) return false;
        return a.Equals(b);
    }

    private void CheckNumberOperands(Token op, object left, object right)
    {
        if (left is double && right is double) return;
        throw new RuntimeError(op, "Operands must be numbers.");
    }

    private void CheckNumberOperand(Token op, Object operand)
    {
        if (operand is double) return;
        throw new RuntimeError(op, "Operand must be a number.");
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
    public class RuntimeError : Exception
    {
        public Token Token { get; }
        
        public override string Message { get; }

        public RuntimeError(Token token, string message)
        {
            Token = token;
            Message = message;
        }
    }

    public object VisitExpressionStmt(Expression stmt)
    {
        Evaluate(stmt.expression);
        return null;
    }

    public object VisitPrintStmt(Print stmt)
    {
        var value = Evaluate(stmt.expression);
        Console.WriteLine(Stringify(value));
        return null;
    }

    public object VisitVarStmt(Var stmt)
    {
        throw new NotImplementedException();
    }

    public void Execute(Stmt statement)
    {
        statement.Accept(this);
    }
}