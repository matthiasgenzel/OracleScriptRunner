using OracleScriptRunner;
using OracleScriptRunner.Settings;
using OracleScriptRunnerFileBuilder.App;
using OracleScriptRunnerFileBuilder.Impl;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("OracleScriptRunnerFileBuilderTest")]
namespace OracleScriptRunnerFileBuilder
{
    public class TerminalExecFileBuilder<TTerminalCommandsBuilder, TSqlCommandsBuilder, TSqlScript>
        : ITerminalExecFileBuilder<TSqlScript>
        where TTerminalCommandsBuilder : ITerminalCommandsBuilder, new()
        where TSqlCommandsBuilder : ISqlCommandsBuilder, new()
        where TSqlScript : ISqlScript, new()
    {
        public const string LogFilePath = ".\\__log.txt";

        private IList<ICopyableFile> _workSqlFiles = new List<ICopyableFile>();
        private string _executeSqlFilePath;

        public IOutputFileSettings OutputFileSettings { get; set; }
        public IEnumerable<string> SqlFilePaths { get; set; }
        public IEnumerable<IDbConnectionString> ConnectionStrings { get; set; }
        public IAddOnScriptsManager AddOnScriptsManager { get; set; }

        string _emptyDirectory;
        public string EmptyDirectory
        {
            get
            {
                return _emptyDirectory;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    ThrowErrorOnNotEmptyDirectory(value);

                _emptyDirectory = value;
            }
        }
        private void ThrowErrorOnNotEmptyDirectory(string emptyDirectory)
        {
            if (Directory.EnumerateFileSystemEntries(emptyDirectory).Any())
            {
                throw new EDirectoryNotEmpty(emptyDirectory);
            }
        }

        public void BuildExecutionFiles()
        {
            AddScriptsToWorkFiles<CopyableFile>();
            CopyAndRenameFilesToWorkingDir();

            BuildExecuteSqlFile();

            if (ConnectionStrings?.Any() == true)
                BuildExecuteBatchFile();
        }

        private void AddScriptsToWorkFiles<TCopyableFile>()
            where TCopyableFile : ICopyableFile, new()
        {
            if (AddOnScriptsManager?.HasPreScript() == true)
            {
                _workSqlFiles.Add(new TCopyableFile()
                {
                    OriginalPath = "pre.sql",
                    Text = AddOnScriptsManager.PreScript.Text
                });
            }

            if (SqlFilePaths?.Count() > 0)
            {
                foreach (var f in SqlFilePaths)
                    _workSqlFiles.Add(new TCopyableFile() { CurrentPath = f, OriginalPath = f });
            }

            if (AddOnScriptsManager?.HasPostScript() == true)
            {
                _workSqlFiles.Add(new TCopyableFile()
                {
                    OriginalPath = "post.sql",
                    Text = AddOnScriptsManager.PostScript.Text
                });
            }
        }

        private void CopyAndRenameFilesToWorkingDir()
        {
            if (_workSqlFiles != null)
            {
                int i = 0;
                foreach (var f in _workSqlFiles)
                {
                    f.CopyTo(EmptyDirectory, $"{OutputFileSettings.SqlScriptName}{++i}{OutputFileSettings.SqlScriptExtension}");
                }
            }
        }

        private void BuildExecuteSqlFile()
        {
            List<ISqlScript> files = new List<ISqlScript>();
            foreach (var file in _workSqlFiles)
            {
                files.Add(new TSqlScript()
                {
                    FilePath = file.CurrentPath,
                    PromptName = file.OriginalPath
                });
            }

            var cBuilder = new TSqlCommandsBuilder()
            {
                Files = files
            };

            var fileText = cBuilder.GetCommandsAsFileText(true);

            _executeSqlFilePath = Path.Combine(EmptyDirectory, OutputFileSettings.FileNameExecutableSql);

            File.WriteAllText(_executeSqlFilePath, fileText);
        }

        private void BuildExecuteBatchFile()
        {
            var commands = new List<ITerminalCommand>();

            var tBuilder = new TTerminalCommandsBuilder()
            {
                LogFilePath = LogFilePath,
                SqlFilePath = _executeSqlFilePath
            };

            foreach (var c in ConnectionStrings.Where((cs) => cs.IsValidConnection()))
            {
                commands.AddRange(tBuilder.GetTerminalCommands(c, false, true));
            }

            File.WriteAllLines(Path.Combine(EmptyDirectory, OutputFileSettings.FileNameExecutable), commands.Select((c) => c.Text));
        }
    }
}