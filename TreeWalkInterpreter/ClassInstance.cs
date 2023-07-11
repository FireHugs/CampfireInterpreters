namespace Campfire.TreeWalkInterpreter;

public class ClassInstance
{
    private ClassDefinition definition;
    private readonly Dictionary<string, object> fields;

    public ClassInstance(ClassDefinition definition)
    {
        this.definition = definition;
        fields = new Dictionary<string, object>();
    }

    public object Get(Token name)
    {
        if (fields.ContainsKey(name.Lexeme))
        {
            return fields[name.Lexeme];
        }

        RuntimeFunction? method = definition.FindMethod(name.Lexeme);
        if (method != null)
        {
            return method;
        }
        
        throw new RuntimeError(name, $"Undefined property {name.Lexeme}.");
    }

    public void Set(Token name, object value)
    {
        fields[name.Lexeme] = value;
    }

    public override string ToString()
    {
        return $"{definition.Name} instance";
    }
}