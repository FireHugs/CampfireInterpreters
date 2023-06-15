using System.Xml;

namespace Campfire.TreeWalkInterpreter;

public class ManualLexer
{
    private string source;
    private List<Token> tokens;

    private int lexStart;
    private int lexCurrent;
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
        char c = Advance();
        switch (c)
        {
            case '(':
                AddToken(TokenType.LeftParen);
                break;
            case ')':
                AddToken(TokenType.RightParen);
                break;
            case '{':
                AddToken(TokenType.LeftBrace);
                break;
            case '}':
                AddToken(TokenType.RightBrace);
                break;
            case ',':
                AddToken(TokenType.Comma);
                break;
            case '.':
                AddToken(TokenType.Dot);
                break;
            case '-':
                AddToken(TokenType.Minus);
                break;
            case '+':
                AddToken(TokenType.Plus);
                break;
            case ';':
                AddToken(TokenType.Semicolon);
                break;
            case '*':
                AddToken(TokenType.Star);
                break;
            case '!':
                AddToken(Match('=') ? TokenType.BangEqual : TokenType.Bang);
                break;
            case '=':
                AddToken(Match('=') ? TokenType.EqualEqual : TokenType.Equal);
                break;
            case '<':
                AddToken(Match('=') ? TokenType.LessEqual : TokenType.Less);
                break;
            case '>':
                AddToken(Match('=') ? TokenType.GreaterEqual : TokenType.Greater);
                break;
            case '/':
                if (Match('/'))
                {
                    while (Peek() != '\n' && !IsAtEnd())
                    {
                        Advance();
                    }
                }
                else
                {
                    AddToken(TokenType.Slash);
                }
                break;
            case ' ':
            case '\r':
            case '\t':
                break;
            case '\n':
                lexLine++;
                break;
            case '"': TokenizeString(); break;
            default:
                if (IsDigit(c))
                {
                    TokenizeNumber();
                }
                else if (IsAlpha(c))
                {
                    TokenizeIdentifier();
                }
                else
                {
                    ErrorHandler.Error(lexLine, "Unexpected Character");    
                }
                break;
        }
    }

    private char Advance()
    {
        return source[lexCurrent++];
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

    private bool IsAtEnd(int offset = 0)
    {
        return lexCurrent + offset >= source.Length;
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

    private char Peek(int offset = 0)
    {
        if (IsAtEnd(offset))
        {
            return '\0';
        }
        
        return source[lexCurrent+offset];
    }

    private bool IsDigit(char c)
    {
        return c >= '0' && c <= '9';
    }

    private bool IsAlpha(char c)
    {
        return (c >= 'a' && c <= 'z') ||
               (c >= 'A' && c <= 'Z') ||
               c == '_';
    }

    private bool IsAlphaNumeric(char c)
    {
        return IsAlpha(c) || IsDigit(c);
    }

    private void TokenizeNumber()
    {
        while (IsDigit(Peek()))
            Advance();

        if (Peek() == '.' && IsDigit(Peek(1)))
        {
            Advance();

            while (IsDigit(Peek()))
                Advance();
        }
        
        AddToken(TokenType.Number, Double.Parse(source.Substring(lexStart,lexCurrent-lexStart)));
    }
    
    private void TokenizeString()
    {
        while (Peek() != '"' && !IsAtEnd())
        {
            if (Peek() == '\n') 
                lexLine++;
            Advance();
        }

        if (IsAtEnd())
        {
            ErrorHandler.Error(lexLine, "Unterminated string.");
            return;
        }

        Advance();

        string value = source.Substring(lexStart + 1, lexCurrent - lexStart - 2);
        AddToken(TokenType.String, value);
    }

    private void TokenizeIdentifier()
    {
        while (IsAlphaNumeric(Peek())) 
            Advance();

        string identifier = source.Substring(lexStart, lexCurrent - lexStart);
        if (Keywords.ReservedKeywords.ContainsKey(identifier))
        {
            AddToken(Keywords.ReservedKeywords[identifier]);
        }
        else
        {
            AddToken(TokenType.Identifier);
        }
    }
}