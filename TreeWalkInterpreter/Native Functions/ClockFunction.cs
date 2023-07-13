namespace Campfire.TreeWalkInterpreter.Native_Functions;

public class ClockFunction: ICallable
{
    int ICallable.Arity => 0;

    public object Call(TreeWalkInterpreter interpreter, List<object> arguments)
    {
        return DateTime.Now.Millisecond / 1000.0;
    }

    public override string ToString()
    {
        return "<native fn>";
    }
}