using OracleScriptRunner;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("OracleScriptRunnerCmdSqlplusTest")]
namespace OracleScriptRunnerCmdSqlplus
{
    internal class WinCmdCommand : ITerminalCommand
    {
        public string Text { get; set; }
        public bool IsExecutingCommand()
        {
            return Text.ToLower().IndexOf("sqlplus") > -1;
        }
    }
}
