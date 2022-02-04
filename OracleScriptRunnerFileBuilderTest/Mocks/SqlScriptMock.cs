using OracleScriptRunner;

namespace OracleScriptRunnerFileBuilderTest.Mocks
{
    internal class SqlScriptMock : ISqlScript
    {
        public string PromptName { get; set; }
        public string FilePath { get; set; }
    }
}