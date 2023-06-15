namespace Campfire.TreeWalkInterpreter;

public class Token
{
    private TokenType Type;
    private string Lexeme;
    private object Literal;
    private int Line;

    public Token(TokenType type, string lexeme, object literal, int line)
    {
        Type = type;
        Lexeme = lexeme;
        Literal = literal;
        Line = line;
    }

    public override string ToString()
    {
        return $"{Type} {Lexeme} {Literal}";
    }
}