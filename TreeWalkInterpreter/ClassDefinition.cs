namespace Campfire.TreeWalkInterpreter;

public class ClassDefinition
{
    private readonly string name;

    public ClassDefinition(string name)
    {
        this.name = name;
    }

    public override string ToString()
    {
        return name;
    }
}