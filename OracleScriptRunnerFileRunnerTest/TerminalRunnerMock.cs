using OracleScriptRunner;
using OracleScriptRunnerFileRunner;
using System.Collections.Generic;

namespace OracleScriptRunnerFileRunnerTest
{
    internal class TerminalRunnerMock : ITerminalRunner
    {
        public string RunProgram(IEnumerable<ITerminalCommand> executionFilePaths)
        {
            var result = string.Empty;

            foreach (var executionFile in executionFilePaths)
                result += $"execute {executionFile.Text}";

            return result;
        }
    }
}