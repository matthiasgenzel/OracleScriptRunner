using OracleScriptRunner.Settings;

namespace OracleScriptRunner
{
    public interface ISqlFileRunner<TTerminalOutput> 
        where TTerminalOutput : ITerminalOutput
    {
        string SqlFilePath { get; set; }

        TTerminalOutput ExecuteForDbConnection(IDbConnectionString db);
    }
}