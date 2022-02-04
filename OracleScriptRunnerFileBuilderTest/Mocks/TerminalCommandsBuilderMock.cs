using OracleScriptRunner;
using OracleScriptRunner.Settings;
using System.Collections.Generic;
using System.IO;

namespace OracleScriptRunnerFileBuilderTest.Mocks
{
    internal class TerminalCommandsBuilderMock : ITerminalCommandsBuilder
    {
        public string LogFilePath { get; set; }
        public string SqlFilePath { get; set; }

        public IEnumerable<ITerminalCommand> GetTerminalCommands(
            IDbConnectionString dbConnectionString,
            bool useFullFileName,
            bool doWriteHeader)
        {
            List<TerminaCommandMock> commands = new List<TerminaCommandMock>();

            if (doWriteHeader)
                commands.Add(new TerminaCommandMock() { Text = "header" });

            commands.Add(new TerminaCommandMock()
            {
                Text = $"connstring: {dbConnectionString} || log to: {LogFilePath} || {Path.GetFileName(SqlFilePath)}"
            });

            return commands;
        }
    }
}