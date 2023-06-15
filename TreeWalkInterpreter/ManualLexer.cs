using System.Xml;

namespace Campfire.TreeWalkInterpreter;

public class ManualLexer
{
    private string source;
    private List<Token> tokens;

    private int lexStart = 0;
    private int lexCurrent = 0;
    private int lexLine = 1;

    public ManualLexer(string source)
    {
        this.source = source;
    }

    public List<Token> ScanTokens()
    {
        tokens = new List<Token>();

        while (!IsAtEnd())
        {
            lexStart = lexCurrent;
            ScanToken();
        }
        
        return tokens;
    }

    private void ScanToken()
    {
        switch (source[lexCurrent++])
        {
            case '(': AddToken(TokenType.LeftParen); break;
            case ')': AddToken(TokenType.RightParen); break;
            case '{': AddToken(TokenType.LeftBrace); break;
            case '}': AddToken(TokenType.RightBrace); break;
            case ',': AddToken(TokenType.Comma); break;
            case '.': AddToken(TokenType.Dot); break;
            case '-': AddToken(TokenType.Minus); break;
            case '+': AddToken(TokenType.Plus); break;
            case ';': AddToken(TokenType.Semicolon); break;
            case '*': AddToken(TokenType.Star); break;
            case '!': AddToken(Match('=') ? TokenType.BangEqual: TokenType.Bang); break;
            case '=': AddToken(Match('=') ? TokenType.EqualEqual: TokenType.Equal); break;
            case '<': AddToken(Match('=') ? TokenType.LessEqual: TokenType.Less); break;
            case '>': AddToken(Match('=') ? TokenType.GreaterEqual: TokenType.Greater); break;
            default: 
                ErrorHandler.Error(lexLine, "Unexpected Character");
                break;
        }
    }

    private void AddToken(TokenType type)
    {
        AddToken(type, null);
    }
    
    private void AddToken(TokenType type, object literal)
    {
        string lexeme = source.Substring(lexStart, lexCurrent - lexStart);
        tokens.Add(new Token(type, lexeme, literal, lexLine));
    }

    private bool IsAtEnd()
    {
        return lexCurrent >= source.Length;
    }

    private bool Match(char expected)
    {
        if (IsAtEnd())
            return false;

        if (source[lexCurrent] != expected) 
            return false;

        lexCurrent++;
        return true;
    }
    
}