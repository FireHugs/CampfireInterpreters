namespace Campfire.TreeWalkInterpreter;

public interface ICallable
{
    object Call(TreeWalkInterpreter interpreter, List<object> arguments);
    int Arity { get; }
}