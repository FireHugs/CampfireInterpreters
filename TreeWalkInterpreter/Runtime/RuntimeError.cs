namespace Campfire.TreeWalkInterpreter;

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

