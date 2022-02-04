
using OracleScriptRunner;

namespace OracleScriptRunnerCmdSqlplus
{
    public class CmdSqlPlusOutputFileSettings : IOutputFileSettings
    {
        public string SqlScriptName { get => "script_"; }
        public string SqlScriptExtension { get => ".sql"; }

        public string FileNameExecutableSql { get => "_execute.sql"; }
        public string FileNameExecutable { get => "_execute.bat"; }
    }
}
