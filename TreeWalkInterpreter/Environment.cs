namespace Campfire.TreeWalkInterpreter;

public class Environment
{
    private readonly Dictionary<string, object> values = new();
    private readonly Environment? enclosingEnvironment;

    public Environment(Environment? enclosingEnvironment = null)
    {
        this.enclosingEnvironment = enclosingEnvironment;
    }

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

        if (enclosingEnvironment != null)
        {
            enclosingEnvironment.AssignTokenValue(name, value);
            return;
        }
    }

    public object GetTokenValue(Token name)
    {
        if (values.ContainsKey(name.Lexeme))
        {
            return values[name.Lexeme];
        }
        
        if (enclosingEnvironment != null)
        {
            return enclosingEnvironment.GetTokenValue(name);
        }

        throw new Interpreter.RuntimeError(name, $"Undefined variable {name.Lexeme}.");
    }
}