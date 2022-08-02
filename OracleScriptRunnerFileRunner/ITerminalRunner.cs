using OracleScriptRunner;
using System.Collections.Generic;

namespace OracleScriptRunnerFileRunner
{
    public interface ITerminalRunner
    {
        string RunProgram(IEnumerable<ITerminalCommand> executionFilePaths);
    }
}