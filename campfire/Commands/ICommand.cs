namespace Campfire.campfire;

public interface ICommand
{
    public string Name { get; }
    public void ExecuteCommand(string[] args, ref MessageHandler.ExitCodes exitCode);
}