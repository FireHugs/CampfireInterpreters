namespace Campfire.TreeWalkInterpreter;

public class RecursiveDescentParser
{
    private readonly List<Token> tokens;
    private int current; 

    public RecursiveDescentParser(List<Token> tokens)
    {
        this.tokens = tokens;
    }

    public List<Stmt> Parse()
    {
        var statements = new List<Stmt>();
        while (!IsAtEnd())
        {
            statements.Add(ParseDeclaration());
        }

        return statements;
    }

    private Expr ParseExpression()
    {
        return ParseAssignment();
    }

    private Expr ParseAssignment()
    {
        var expression = ParseOr();

        if (Match(TokenType.Equal))
        {
            var equals = Previous();
            var value = ParseAssignment();

            if (expression is Variable)
            {
                var name = ((Variable)expression).name;
                return new Assign(name, value);
            }
            
            ErrorHandler.Error(equals, "Invalid assignment target.");
        }

        return expression;
    }

    private Expr ParseOr()
    {
        var expression = ParseAnd();

        while (Match(TokenType.Or))
        {
            var op = Previous();
            var right = ParseAnd();
            expression = new Logical(expression, op, right);
        }

        return expression;
    }

    private Expr ParseAnd()
    {
        var expression = ParseEquality();

        while (Match(TokenType.And))
        {
            var op = Previous();
            var right = ParseEquality();
            expression = new Logical(expression, op, right);
        }
        
        return expression;
    }

    private Expr ParseEquality()
    {
        Expr expression = ParseComparison();
        while (Match(TokenType.BangEqual, TokenType.EqualEqual))
        {
            var op = Previous();
            var right = ParseComparison();
            expression = new Binary(expression, op, right);
        }

        return expression;
    }

    private Expr ParseComparison()
    {
        var expression = ParseTerm();
        while (Match(TokenType.Greater, TokenType.GreaterEqual, TokenType.Less, TokenType.LessEqual))
        {
            var op = Previous();
            var right = ParseTerm();
            expression = new Binary(expression, op, right);
        }

        return expression;
    }

    private bool Match(params TokenType[] tokenTypes)
    {
        foreach (var tokenType in tokenTypes)
        {
            if (Check(tokenType))
            {
                Advance();
                return true;
            }
        }

        return false;
    }

    private bool Check(TokenType tokenType)
    {
        if (IsAtEnd()) return false;
        return Peek().Type == tokenType;
    }

    private Token Advance()
    {
        if (!IsAtEnd()) current++;
        return Previous();
    }

    private bool IsAtEnd()
    {
        return Peek().Type == TokenType.EOF;
    }

    private Token Peek()
    {
        return tokens[current];
    }

    private Token Previous()
    {
        return tokens[current - 1];
    }

    private Expr ParseTerm()
    {
        var expression = ParseFactor();

        while (Match(TokenType.Minus, TokenType.Plus))
        {
            var op = Previous();
            var right = ParseFactor();
            expression = new Binary(expression, op, right);
        }

        return expression;
    }

    private Expr ParseFactor()
    {
        var expression = ParseUnary();

        while (Match(TokenType.Slash, TokenType.Star))
        {
            var op = Previous();
            var right = ParseUnary();
            expression = new Binary(expression, op, right);
        }

        return expression;
    }

    private Expr ParseUnary()
    {
        if (Match(TokenType.Bang, TokenType.Minus))
        {
            var op = Previous();
            var right = ParseUnary();
            return new Unary(op, right);
        }

        return ParsePrimary();
    }

    private Expr ParsePrimary()
    {
        if (Match(TokenType.False)) return new Literal(false);
        if (Match(TokenType.True)) return new Literal(true);
        if (Match(TokenType.Nil)) return new Literal(null);

        if (Match(TokenType.Number, TokenType.String))
        {
            return new Literal(Previous().Literal);
        }

        if (Match(TokenType.Identifier))
        {
            return new Variable(Previous());
        }

        if (Match(TokenType.LeftParen))
        {
            var expression = ParseExpression();
            Consume(TokenType.RightParen, "Expect ')' after expression.");
            return new Grouping(expression);
        }

        throw ThrowParseError(Peek(), "Expect expression.");
    }

    private Token Consume(TokenType tokenType, string message)
    {
        if (Check(tokenType)) return Advance();
        throw ThrowParseError(Peek(), message);
    }

    private ParseError ThrowParseError(Token token, string message)
    {
        ErrorHandler.Error(token, message);
        return new ParseError();
    }

    private void Synchronize()
    {
        Advance();
        while (!IsAtEnd())
        {
            if (Previous().Type == TokenType.Semicolon) return;

            switch (Peek().Type)
            {
                case TokenType.Class:
                case TokenType.For:
                case TokenType.Fun:
                case TokenType.If:
                case TokenType.Print:
                case TokenType.Return:
                case TokenType.Var:
                case TokenType.While:
                    return;
            }

            Advance();
        }
    }

    private Stmt ParseStatement()
    {
        if (Match(TokenType.If)) return ParseIfStatement();
        if (Match(TokenType.Print)) return ParsePrintStatement();
        if (Match(TokenType.LeftBrace)) return new Block(ParseBlock());
        return ParseExpressionStatement();
    }

    private List<Stmt> ParseBlock()
    {
        var statements = new List<Stmt>();
        while (!Check(TokenType.RightBrace) && !IsAtEnd())
        {
            statements.Add(ParseDeclaration());
        }

        Consume(TokenType.RightBrace, "Expect } after block.");
        return statements;
    }

    private Stmt ParseIfStatement()
    {
        Consume(TokenType.LeftParen, "Expect '(' after 'if'.");
        var condition = ParseExpression();
        Consume(TokenType.RightParen, "Expect ')' after if condition.");

        var thenBranch = ParseStatement();
        Stmt elseBranch = null;
        if (Match(TokenType.Else))
        {
            elseBranch = ParseStatement();
        }

        return new If(condition, thenBranch, elseBranch);
    }

    private Stmt ParsePrintStatement()
    {
        var value = ParseExpression();
        Consume(TokenType.Semicolon, "Expect ; after value.");
        return new Print(value);
    }

    private Stmt ParseExpressionStatement()
    {
        var expression = ParseExpression();
        Consume(TokenType.Semicolon, "Expect ; after expression.");
        return new Expression(expression);
    }

    private Stmt ParseDeclaration()
    {
        try
        {
            if (Match(TokenType.Var)) return ParseVarDeclaration();
            return ParseStatement();
        }
        catch (ParseError e)
        {
            Synchronize();
            return null;
        }
    }

    private Stmt ParseVarDeclaration()
    {
        var name = Consume(TokenType.Identifier, "Expect variable name.");

        Expr initializer = null;
        if (Match(TokenType.Equal))
        {
            initializer = ParseExpression();
        }

        Consume(TokenType.Semicolon, "Expect ; after variable declaration.");
        return new Var(name, initializer);
    }

    private class ParseError : Exception { }
}