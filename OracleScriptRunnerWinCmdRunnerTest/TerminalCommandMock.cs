using OracleScriptRunner;

namespace OracleScriptRunnerWinCmdRunnerTest
{
    internal class TerminalCommandMock : ITerminalCommand
    {
        public string Text { get; set; }

        public bool IsExecutingCommand()
        {
            return true;
        }
    }
}