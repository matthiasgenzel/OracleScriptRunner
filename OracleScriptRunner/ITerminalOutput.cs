namespace OracleScriptRunner
{
    public interface ITerminalOutput
    {
        bool HasError { get; }
        string Output { get; set; }
    }
}