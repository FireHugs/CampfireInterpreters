namespace Campfire.TreeWalkInterpreter;

public partial class Interpreter: Expr.Visitor<object>
{
    public object VisitAssignExpr(Assign expr)
    {
        var value = EvaluateExpression(expr.value);
        environment.AssignTokenValue(expr.name, value);
        return value;
    }

    public object VisitUnaryExpr(Unary expr)
    {
        var right = EvaluateExpression(expr.Right);

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
        var left = EvaluateExpression(expr.Left);
        var right = EvaluateExpression(expr.Right);

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
        return EvaluateExpression(expr.Expression);
    }

    public object VisitLiteralExpr(Literal expr)
    {
        return expr.Value;
    }

    public object VisitVariableExpr(Variable expr)
    {
        return environment.GetTokenValue(expr.name);
    }
    
    private Object EvaluateExpression(Expr expression)
    {
        return expression.Accept(this);
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
    
    private bool IsTruthy(Object obj)
    {
        if (obj == null) return false;
        if (obj is bool b) return b;
        return true;
    }
}