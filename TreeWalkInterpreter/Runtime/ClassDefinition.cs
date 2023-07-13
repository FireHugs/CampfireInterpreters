namespace Campfire.TreeWalkInterpreter;

public class ClassDefinition: ICallable
{
    public string Name { get; }
    private Dictionary<string, RuntimeFunction> methods;
    private ClassDefinition superClass;

    public ClassDefinition(string name, ClassDefinition superclass, Dictionary<string, RuntimeFunction> methods)
    {
        Name = name;
        this.superClass = superclass;
        this.methods = methods;
    }

    public override string ToString()
    {
        return Name;
    }

    public object Call(TreeWalkInterpreter interpreter, List<object> arguments)
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

        if (superClass != null)
        {
            return superClass.FindMethod(name);
        }

        return null;
    }
}