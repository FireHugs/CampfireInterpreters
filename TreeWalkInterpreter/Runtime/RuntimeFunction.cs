namespace Campfire.TreeWalkInterpreter;

public class RuntimeFunction: ICallable
{
    private readonly Function declaration;
    private readonly Environment closure;
    private bool isInitializer;

    public RuntimeFunction(Function declaration, Environment closure, bool isInitializer)
    {
        this.declaration = declaration;
        this.closure = closure;
        this.isInitializer = isInitializer;
    }
    
    public object Call(TreeWalkInterpreter interpreter, List<object> arguments)
    {
        var environment = new Environment(closure);
        for(int i=0; i < declaration.parameters.Count; i++)
        {
            environment.Define(declaration.parameters[i].Lexeme, arguments[i]);
        }

        try
        {
            interpreter.ExecuteBlock(declaration.body, environment);
        }
        catch (Return returnValue)
        {
            return isInitializer ? closure.GetTokenValueAt(0, "this"): returnValue.Value;
        }

        if (isInitializer) return closure.GetTokenValueAt(0, "this");
        return null;
    }

    public int Arity => declaration.parameters.Count;

    public override string ToString()
    {
        return $"<fun {declaration.name.Lexeme}>";
    }

    public RuntimeFunction Bind(ClassInstance instance)
    {
        var environment = new Environment(closure);
        environment.Define("this", instance);
        return new RuntimeFunction(declaration, environment, isInitializer);
    }
}