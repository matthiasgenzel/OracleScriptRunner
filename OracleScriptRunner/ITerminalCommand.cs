namespace OracleScriptRunner
{
    public interface ITerminalCommand
    {
        string Text { get; set; }

        bool IsExecutingCommand();
    }
}