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
    public Environment? EnclosingEnvironment { get; }

    public Environment(Environment? enclosingEnvironment = null)
    {
        EnclosingEnvironment = enclosingEnvironment;
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

        if (EnclosingEnvironment != null)
        {
            EnclosingEnvironment.AssignTokenValue(name, value);
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
        
        if (EnclosingEnvironment != null)
        {
            return EnclosingEnvironment.GetTokenValue(name);
        }

        throw new RuntimeError(name, $"Undefined variable {name.Lexeme}.");
    }

    public object GetTokenValueAt(int distance, Token name)
    {
        return GetAncestor(distance).values[name.Lexeme].Value;
    }
    
    public object GetTokenValueAt(int distance, string name)
    {
        return GetAncestor(distance).values[name].Value;
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
            environment = environment.EnclosingEnvironment;
        }

        return environment;
    }
}