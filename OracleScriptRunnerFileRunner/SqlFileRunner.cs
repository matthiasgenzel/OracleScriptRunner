using OracleScriptRunner;
using OracleScriptRunner.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("OracleScriptRunnerFileRunnerTest")]
namespace OracleScriptRunnerFileRunner
{
    public class SqlFileRunner<TTerminalCommandsBuilder, TTerminalRunner, TTerminalOutput> : ISqlFileRunner<TTerminalOutput>
        where TTerminalCommandsBuilder : ITerminalCommandsBuilder, new()
        where TTerminalRunner: ITerminalRunner, new()
        where TTerminalOutput: ITerminalOutput, new()
    {
        public string SqlFilePath { get; set; }
        internal ITerminalCommandsBuilder commands;

        public TTerminalOutput ExecuteForDbConnection(IDbConnectionString db)
        {
            // build terminal commands to run sql
            commands = new TTerminalCommandsBuilder()
            {
                SqlFilePath = SqlFilePath
            };

            var cmds = commands.GetTerminalCommands(
                dbConnectionString: db, 
                useFullFileName: true,
                doWriteHeader: false // no header in direct mode
            );
            var runner = new TTerminalRunner();

            var output = new TTerminalOutput()
            {
                Output = runner.RunProgram(cmds)
            };

            return output;
        }
    }
}
