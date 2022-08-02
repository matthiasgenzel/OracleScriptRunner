using OracleScriptRunner;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleScriptRunnerFileRunnerTest
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
