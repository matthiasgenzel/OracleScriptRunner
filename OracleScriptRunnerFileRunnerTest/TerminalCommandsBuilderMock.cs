using OracleScriptRunner;
using OracleScriptRunner.Settings;
using System.Collections.Generic;
using System.IO;

namespace OracleScriptRunnerFileRunnerTest
{
    internal class TerminalCommandsBuilderMock<TTerminalCommand> : ITerminalCommandsBuilder
        where TTerminalCommand: ITerminalCommand, new()
    {
        public string LogFilePath { get; set; }
        public string SqlFilePath { get; set; }

        private string _lastConnection;

        public IEnumerable<ITerminalCommand> GetTerminalCommands(
            IDbConnectionString dbConnectionString, 
            bool useFullFileName, 
            bool doWriteHeader)
        {
            var _header = $"echo header {_lastConnection}";
            var _command = $"{SqlFileRunnerTest.ExecCommand} {dbConnectionString.ConnectionString} {SqlFilePath}";

            _lastConnection = dbConnectionString.ConnectionString;

            var result = new List<ITerminalCommand>();

            if (doWriteHeader)
                result.Add(new TTerminalCommand() {Text = _header });

            result.Add(new TTerminalCommand() { Text = _command });

            return result;
        }
    }
}