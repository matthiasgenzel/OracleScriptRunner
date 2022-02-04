using OracleScriptRunner;

namespace OracleScriptRunnerFileBuilderTest.Mocks
{
    internal class OutputFileSettingsMock : IOutputFileSettings
    {
        public string FileNameExecutable => "exec.bat";

        public string SqlScriptName => "script_";
        public string SqlScriptExtension => ".sql";

        public string FileNameExecutableSql => "script.sql";
    }
}
