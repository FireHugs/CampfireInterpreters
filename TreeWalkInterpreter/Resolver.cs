using System.Data;
using System.Net.Mail;

namespace Campfire.TreeWalkInterpreter;

public class Resolver : Expr.Visitor<object>, Stmt.Visitor<object>
{
    private enum FunctionType
    {
        None, 
        Function,
        Method
    }
    private readonly Interpreter interpreter;
    private Stack<Dictionary<string, bool>> scopes;
    private FunctionType currentFunctionType = FunctionType.None;  

    public Resolver(Interpreter interpreter)
    {
        this.interpreter = interpreter;
        scopes = new Stack<Dictionary<string, bool>>();
    }

    public object VisitAssignExpr(Assign expr)
    {
        Resolve(expr.value);
        ResolveLocal(expr, expr.name);
        return null;
    }

    public object VisitUnaryExpr(Unary expr)
    {
        Resolve(expr.Right);
        return null;
    }

    public object VisitBinaryExpr(Binary expr)
    {
        Resolve(expr.Left);
        Resolve(expr.Right);
        return null;
    }

    public object VisitCallExpr(Call expr)
    {
        Resolve(expr.callee);

        foreach (var argument in expr.arguments)
        {
            Resolve(argument);
        }

        return null;
    }

    public object VisitGetExpr(Get expr)
    {
        Resolve(expr.obj);
        return null;
    }

    public object VisitGroupingExpr(Grouping expr)
    {
        Resolve(expr.Expression);
        return null;
    }

    public object VisitLiteralExpr(Literal expr)
    {
        return null;
    }

    public object VisitLogicalExpr(Logical expr)
    {
        Resolve(expr.left);
        Resolve(expr.right);
        return null;
    }

    public object VisitSetExpr(Set expr)
    {
        Resolve(expr.value);
        Resolve(expr.obj);
        return null;
    }

    public object VisitVariableExpr(Variable expr)
    {
        if (scopes.Count > 0 && scopes.Peek().ContainsKey(expr.name.Lexeme) && scopes.Peek()[expr.name.Lexeme] == false)
        {
            ErrorHandler.Error(expr.name, "Can't read local variable in its own initializer.");
        }

        ResolveLocal(expr, expr.name);
        return null;
    }

    public object VisitBlockStmt(Block stmt)
    {
        BeginScope();
        Resolve(stmt.statements);
        EndScope();
        return null;
    }

    public object VisitClassStmt(Class stmt)
    {
        Declare(stmt.name);
        Define(stmt.name);
        
        foreach(var method in stmt.methods)
        {
            FunctionType declarationType = FunctionType.Method;
            ResolveFunction(method, declarationType);
        }
        
        return null;
    }

    public object VisitExpressionStmt(Expression stmt)
    {
        Resolve(stmt.expression);
        return null;
    }

    public object VisitFunctionStmt(Function stmt)
    {
        Declare(stmt.name);
        Define(stmt.name);
        ResolveFunction(stmt, FunctionType.Function);
        return null;
    }

    public object VisitIfStmt(If stmt)
    {
        Resolve(stmt.condition);
        Resolve(stmt.thenBranch);
        if(stmt.elseBranch != null)
            Resolve(stmt.elseBranch);
        return null;
    }

    public object VisitPrintStmt(Print stmt)
    {
        Resolve(stmt.expression);
        return null;
    }

    public object VisitReturnStatementStmt(ReturnStatement stmt)
    {
        if (currentFunctionType == FunctionType.None)
        {
            ErrorHandler.Error(stmt.keyword, "Can't return from top-level code");
        }
        
        if (stmt.value != null)
        {
            Resolve(stmt.value);
        }

        return null;
    }

    public object VisitVarStmt(Var stmt)
    {
        Declare(stmt.name);
        if (stmt.initializer != null)
        {
            Resolve(stmt.initializer);
        }
        Define(stmt.name);
        return null;
    }

    public object VisitWhileStmt(While stmt)
    {
        Resolve(stmt.condition);
        Resolve(stmt.body);
        return null;
    }

    public void Resolve(List<Stmt> statements)
    {
        foreach (var statement in statements)
        {
            Resolve(statement);
        }
    }

    private void Resolve(Stmt statement)
    {
        statement.Accept(this);
    }

    private void Resolve(Expr expression)
    {
        expression.Accept(this);
    }

    private void ResolveLocal(Expr expr, Token name)
    {
        for (int i = 0; i < scopes.Count; i++)
        {
            var currentScope = scopes.ToArray()[i];
            if (currentScope.ContainsKey(name.Lexeme))
            {
                interpreter.Resolve(expr, i);
            }
        }
    }

    private void ResolveFunction(Function function, FunctionType functionType)
    {
        FunctionType enclosingFunctionType = currentFunctionType;
        currentFunctionType = functionType;
        
        BeginScope();
        foreach (var param in function.parameters)
        {
            Declare(param);
            Define(param);
        }
        Resolve(function.body);
        EndScope();

        currentFunctionType = enclosingFunctionType;
    }

    private void BeginScope()
    {
        scopes.Push(new Dictionary<string, bool>());
    }

    private void EndScope()
    {
        scopes.Pop();
    }

    private void Declare(Token name)
    {
        if (scopes.Count == 0) return;

        Dictionary<string, bool> scope = scopes.Peek();
        if (scope.ContainsKey(name.Lexeme))
        {   
            ErrorHandler.Error(name, "Already a variable with this name in this scope.");
        }

        scope[name.Lexeme] = false;
    }

    private void Define(Token name)
    {
        if (scopes.Count == 0) return;
        scopes.Peek()[name.Lexeme] = true;
    }
}