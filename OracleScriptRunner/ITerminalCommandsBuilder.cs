using OracleScriptRunner.Settings;
using System.Collections.Generic;

namespace OracleScriptRunner
{
    public interface ITerminalCommandsBuilder
    {
        string LogFilePath { get; set; }
        string SqlFilePath { get; set; }

        IEnumerable<ITerminalCommand> GetTerminalCommands(
            IDbConnectionString dbConnectionString,
            bool useFullFileName,
            bool doWriteHeader);
    }
}
