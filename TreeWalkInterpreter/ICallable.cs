namespace Campfire.TreeWalkInterpreter;

public interface ICallable
{
    object Call(Interpreter interpreter, List<object> arguments);
    int Arity { get; }
}