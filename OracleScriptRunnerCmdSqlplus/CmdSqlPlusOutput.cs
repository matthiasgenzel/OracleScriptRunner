using OracleScriptRunner;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleScriptRunnerCmdSqlplus
{
    internal class CmdSqlPlusOutput : ITerminalOutput
    {
        public string Output { get; set; }
        public bool HasError =>
            Output.Contains($"{Environment.NewLine}ORA-") ||
            Output.Contains($"{Environment.NewLine}PLS-");
    }
}
