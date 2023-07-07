namespace Campfire.TreeWalkInterpreter;

public partial class Interpreter
{
    private Environment environment = new();
    
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

    private void ExecuteBlock(List<Stmt> statements, Environment environment)
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