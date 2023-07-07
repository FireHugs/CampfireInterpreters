namespace Campfire.TreeWalkInterpreter;

public class Environment
{
    private readonly Dictionary<string, object> values = new Dictionary<string, object>();

    public void Define(string name, object value)
    {
        values[name] = value;
    }

    public void AssignTokenValue(Token name, object value)
    {
        if (values.ContainsKey(name.Lexeme))
        {
            values[name.Lexeme] = value;
        }
    }

    public object GetTokenValue(Token name)
    {
        if (values.ContainsKey(name.Lexeme))
        {
            return values[name.Lexeme];
        }

        throw new Interpreter.RuntimeError(name, $"Undefined variable {name.Lexeme}.");
    }
}