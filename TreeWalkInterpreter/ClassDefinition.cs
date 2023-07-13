namespace Campfire.TreeWalkInterpreter;

public class ClassDefinition: ICallable
{
    public string Name { get; }
    private Dictionary<string, RuntimeFunction> methods;

    public ClassDefinition(string name, Dictionary<string, RuntimeFunction> methods)
    {
        Name = name;
        this.methods = methods;
    }

    public override string ToString()
    {
        return Name;
    }

    public object Call(Interpreter interpreter, List<object> arguments)
    {
        var instance = new ClassInstance(this);
        
        var initializer = FindMethod("init");
        initializer?.Bind(instance).Call(interpreter, arguments);

        return instance;
    }

    public int Arity
    {
        get
        {
            var initializer = FindMethod("init");
            return initializer?.Arity ?? 0;
        }
    }

    public RuntimeFunction? FindMethod(string name)
    {
        if (methods.ContainsKey(name))
        {
            return methods[name];
        }

        return null;
    }
}