namespace Campfire.TreeWalkInterpreter;

public class Environment
{
    private class ValueEntry
    {
        private object value;

        public object Value
        {
            get => value;
            set
            {
                this.value = value;
                IsInitialized = true;
            }
        }

        public bool IsInitialized
        {
            get;
            private set;
        }

        public void Reset()
        {
            IsInitialized = false;
        }
    }
    
    private readonly Dictionary<string, ValueEntry> values = new();
    private readonly Environment? enclosingEnvironment;

    public Environment(Environment? enclosingEnvironment = null)
    {
        this.enclosingEnvironment = enclosingEnvironment;
    }

    public void Declare(string name)
    {
        if (values.ContainsKey(name))
        {
            values[name].Reset();
        }
        else
        {
            values[name] = new ValueEntry();
        }
    }
    
    public void Define(string name, object value)
    {
        if (!values.ContainsKey(name))
        {
            values[name] = new ValueEntry();
        }
        values[name].Value = value;
    }

    public void AssignTokenValue(Token name, object value)
    {
        if (values.ContainsKey(name.Lexeme))
        {
            values[name.Lexeme].Value = value;
            return;
        }

        if (enclosingEnvironment != null)
        {
            enclosingEnvironment.AssignTokenValue(name, value);
            return;
        }
        
        throw new RuntimeError(name, $"Undefined variable {name.Lexeme}.");
    }

    public object GetTokenValue(Token name)
    {
        if (values.ContainsKey(name.Lexeme))
        {
            if (!values[name.Lexeme].IsInitialized)
            {
                throw new RuntimeError(name, $"Accessing unassigned variable {name.Lexeme}.");        
            }
            return values[name.Lexeme].Value;
        }
        
        if (enclosingEnvironment != null)
        {
            return enclosingEnvironment.GetTokenValue(name);
        }

        throw new RuntimeError(name, $"Undefined variable {name.Lexeme}.");
    }

    public object GetTokenValueAt(int distance, Token name)
    {
        return GetAncestor(distance).values[name.Lexeme].Value;
    }

    public void AssignTokenValueAt(int distance, Token name, object value)
    {
        var valueEntry = new ValueEntry();
        valueEntry.Value = value;
        
        GetAncestor(distance).values[name.Lexeme] = valueEntry;
    }

    private Environment GetAncestor(int distance)
    {
        var environment = this;
        for (int i = 0; i < distance; i++)
        {
            environment = environment.enclosingEnvironment;
        }

        return environment;
    }
}