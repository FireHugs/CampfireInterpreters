namespace Campfire.TreeWalkInterpreter;

public class RuntimeFunction: ICallable
{
    private readonly Function declaration;
    private readonly Environment closure;

    public RuntimeFunction(Function declaration, Environment closure)
    {
        this.declaration = declaration;
        this.closure = closure;
    }
    
    public object Call(Interpreter interpreter, List<object> arguments)
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
            return returnValue.Value;
        }
        
        return null;
    }

    public int Arity => declaration.parameters.Count;

    public override string ToString()
    {
        return $"<fun {declaration.name.Lexeme}>";
    }
}