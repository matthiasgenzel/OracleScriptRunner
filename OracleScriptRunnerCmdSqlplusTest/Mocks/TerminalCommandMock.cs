using OracleScriptRunner;

namespace OracleScriptRunnerCmdSqlplusTest.Mocks
{
    internal class TerminalCommandMock : ITerminalCommand
    {
        public string Text { get; set; }

        public bool IsExecutingCommand()
        {
            return Text.Contains("sqlplus");
        }
    }
}