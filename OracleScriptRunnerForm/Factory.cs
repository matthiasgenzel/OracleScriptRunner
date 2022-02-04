using OracleScriptRunner;
using OracleScriptRunner.Settings;
using OracleScriptRunnerCmdSqlplus;
using OracleScriptRunnerFileBuilder;
using OracleScriptRunnerFileBuilder.Impl;
using OracleScriptRunnerSettings;
using OracleScriptRunnerSettingsFile;
using System.Collections.Generic;

namespace OracleScriptRunnerForm
{
    internal class Factory
    {
        internal static ISetting CreateSetting(string name)
        {
            return new SettingsFile()
            {
                Name = name
            };
        }

        internal static IDbConnectionStringCollection CreateDbConnectionStringCollection()
        {
            var setting = CreateSetting("connections.txt");

            return new DbConnectionStringCollection(setting);
        }

        internal static IAddOnScriptsManager CreateAddOnScriptsManager()
        {
            var pre = CreateSetting("pre.sql");
            var post = CreateSetting("post.sql");

            return new AddOnScriptsManager(pre, post);
        }

        internal static ITerminalExecFileBuilder<SqlScript> CreateTerminExecFileBuilder(
            IAddOnScriptsManager addOnScriptsManager,
            IEnumerable<IDbConnectionString> connectionStrings,
            string emptyDirectory,
            IEnumerable<string> sqlFilePaths)
        {
            return new TerminalExecFileBuilder<WinCmdCommandsBuilder, SqlPlusCommandBuilder, SqlScript>()
            {
                AddOnScriptsManager = addOnScriptsManager,
                ConnectionStrings = connectionStrings,
                EmptyDirectory = emptyDirectory,
                OutputFileSettings = new CmdSqlPlusOutputFileSettings(),
                SqlFilePaths = sqlFilePaths
            };
        }
    }
}
