using OracleScriptRunner;

namespace OracleScriptRunnerFileRunnerTest
{
    internal class TerminalOutputMock : ITerminalOutput
    {
        public bool HasError => false;

        public string Output { get; set; }
    }
}