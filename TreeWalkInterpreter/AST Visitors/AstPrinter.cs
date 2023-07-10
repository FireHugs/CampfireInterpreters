using System.Text;

namespace Campfire.TreeWalkInterpreter.AST_Visitors;

public class AstPrinter: Expr.Visitor<string>
{
    public string Print(Expr expression)
    {
        return expression.Accept(this);
    }

    public string VisitAssignExpr(Assign expr)
    {
        throw new NotImplementedException();
    }

    public string VisitUnaryExpr(Unary expr)
    {
        return parenthesize(expr.Op.Lexeme, expr.Right);
    }

    public string VisitBinaryExpr(Binary expr)
    {
        return parenthesize(expr.Op.Lexeme, expr.Left, expr.Right);
    }

    public string VisitGroupingExpr(Grouping expr)
    {
        return parenthesize("group", expr.Expression);
    }

    public string VisitLiteralExpr(Literal expr)
    {
        if (expr.Value == null) return "nil";
        return expr.Value.ToString();
    }

    public string VisitLogicalExpr(Logical expr)
    {
        throw new NotImplementedException();
    }

    public string VisitVariableExpr(Variable expr)
    {
        throw new NotImplementedException();
    }

    private string parenthesize(string name, params Expr[] expressions)
    {
        var builder = new StringBuilder();

        builder.Append('(').Append(name);
        foreach (var expression in expressions)
        {
            builder.Append(' ');
            builder.Append(expression.Accept(this));
        }
        builder.Append(')');

        return builder.ToString();
    }
}