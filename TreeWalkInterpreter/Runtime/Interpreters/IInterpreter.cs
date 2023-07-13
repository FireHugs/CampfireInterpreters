namespace Campfire.TreeWalkInterpreter;

public interface IInterpreter
{
    public void Interpret(List<Stmt> statements);
}