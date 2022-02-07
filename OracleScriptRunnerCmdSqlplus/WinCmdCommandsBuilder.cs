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

            commands.Add(GetSqlPlusCommand(dbConnectionString, useFullFileName));

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

        private string getFileName(
            bool useFullFileName)
        {
            if (!string.IsNullOrEmpty(SqlFilePath))
            {
                return " @" + (useFullFileName ? SqlFilePath : Path.GetFileName(SqlFilePath));
            }

            return string.Empty;
        }

        /*
         returns sqlplus command with options for silent and w/o login prompt
           if SqlFilePath is not set, sqlplus will be plainly opened
           if set the file will be executed and sqlplus closed afterwards
         */
        private ITerminalCommand GetSqlPlusCommand(
            IDbConnectionString connection,
            bool useFullFileName)
        {
            // when one sql file is directly called -> always push quit at the end to make sure that 
            // sqlplus will be closed
            var alwaysQuit = !string.IsNullOrEmpty(SqlFilePath) ? "echo quit | " : string.Empty;

            /*
             * -S silent
             * -L no logon prompt
             */
            var sqlPlusCall = $"sqlplus -L -S {connection.ConnectionString}";

            var command = new WinCmdCommand
            {
                Text = $"{alwaysQuit}{sqlPlusCall}{getFileName(useFullFileName)}{getLogPart()}"
            };
            
            return command;
        }
    }
}
