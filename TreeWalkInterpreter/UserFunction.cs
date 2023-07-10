namespace Campfire.TreeWalkInterpreter;

public class UserFunction: ICallable
{
    private readonly Function declaration;

    public UserFunction(Function declaration)
    {
        this.declaration = declaration;
    }
    
    public object Call(Interpreter interpreter, List<object> arguments)
    {
        var environment = new Environment(interpreter.Globals);
        for(int i=0; i < declaration.parameters.Count; i++)
        {
            environment.Define(declaration.parameters[i].Lexeme, arguments[i]);
        }
        
        interpreter.ExecuteBlock(declaration.body, environment);
        return null;
    }

    public int Arity => declaration.parameters.Count;

    public override string ToString()
    {
        return $"<fun {declaration.name.Lexeme}>";
    }
}