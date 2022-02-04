using OracleScriptRunner.Settings;
using System.Collections.Generic;

namespace OracleScriptRunner
{
    public interface ITerminalExecFileBuilder<TSqlScript>
        where TSqlScript : ISqlScript
    {
        public IOutputFileSettings OutputFileSettings { get; set; }
        IAddOnScriptsManager AddOnScriptsManager { get; set; }
        IEnumerable<IDbConnectionString> ConnectionStrings { get; set; }
        string EmptyDirectory { get; set; }
        IEnumerable<string> SqlFilePaths { get; set; }

        void BuildExecutionFiles();
    }
}