using OracleScriptRunner;

namespace OracleScriptRunnerFileBuilderTest.Mocks
{
    internal class TerminaCommandMock : ITerminalCommand
    {
        public string Text { get; set; }

        public bool IsExecutingCommand()
        {
            return Text.Contains("execute");
        }
    }
}