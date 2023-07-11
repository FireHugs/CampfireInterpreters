using Campfire.TreeWalkInterpreter.Native_Functions;
namespace Campfire.TreeWalkInterpreter;

public partial class Interpreter
{
    public Environment globals;
    private Environment environment;

    public Interpreter()
    {
        globals = new Environment();
        environment = globals;
        globals.Define("clock", new ClockFunction());
    }
    
    public void Interpret(List<Stmt> statements)
    {
        try
        {
            foreach(var statement in statements)
            {
                Execute(statement);
            }
        }
        catch (RuntimeError e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void Execute(Stmt statement)
    {
        statement.Accept(this);
    }

    public void ExecuteBlock(List<Stmt> statements, Environment environment)
    {
        var previous = this.environment;
        try
        {
            this.environment = environment;
            foreach (var statement in statements)
            {
                Execute(statement);
            }
        }
        catch (RuntimeError e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            this.environment = previous;
        }
    }
}