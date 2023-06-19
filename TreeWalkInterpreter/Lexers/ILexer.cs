namespace Campfire.TreeWalkInterpreter;

public interface ILexer
{
    public List<Token> ScanTokens();
}