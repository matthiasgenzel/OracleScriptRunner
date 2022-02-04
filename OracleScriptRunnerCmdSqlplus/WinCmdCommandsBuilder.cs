using OracleScriptRunner;
using OracleScriptRunner.Settings;
using System.Collections.Generic;
using System.IO;

namespace OracleScriptRunnerCmdSqlplus
{
    public class WinCmdCommandsBuilder : ITerminalCommandsBuilder
    {
        public string LogFilePath { get; set; }
        public string SqlFilePath { get; set; }

        public IEnumerable<ITerminalCommand> GetTerminalCommands(IDbConnectionString dbConnectionString, bool useFullFileName, bool doWriteHeader)
        {
            var commands = new List<ITerminalCommand>();
            if (doWriteHeader)
                commands.Add(GetBatchHeaderSqlForSqlPlus(dbConnectionString));

            commands.Add(GetSqlPlusCommand(dbConnectionString, SqlFilePath, useFullFileName));

            return commands;
        }

        private ITerminalCommand GetBatchHeaderSqlForSqlPlus(IDbConnectionString c)
        {
            var command = new WinCmdCommand
            {
                Text = $"echo {c.ToString()}{getLogPart()}"
            };

            return command;
        }

        private string getLogPart()
        {
            var l = string.Empty;
            if (!string.IsNullOrEmpty(LogFilePath))
                l = $" >> {LogFilePath}";
            return l;
        }

        private ITerminalCommand GetSqlPlusCommand(
            IDbConnectionString connection,
            string sqlFilePath,
            bool useFullFileName)
        {
            var fileName = useFullFileName ? sqlFilePath : Path.GetFileName(sqlFilePath);

            var command = new WinCmdCommand
            {
                Text = $"echo quit | sqlplus -L -S {connection.ConnectionString} @{fileName}{getLogPart()}"
            };

            /*
             * -S silent
             * -L no logon prompt
             */
            return command;
        }
    }
}
