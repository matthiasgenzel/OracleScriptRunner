using NUnit.Framework;
using OracleScriptRunner;
using OracleScriptRunner.Settings;
using OracleScriptRunnerFileBuilder;
using OracleScriptRunnerFileBuilderTest.Mocks;
using OracleScriptRunnerSettingsFile;
using System;
using System.Collections.Generic;
using System.IO;

namespace OracleScriptRunnerFileBuilderTest
{
    internal class TerminalExecFileBuilderTest
    {
        private string _settingsDir;
        private string _execSqlFilePath;
        private string _execBatFilePath;

        private string _baseDir;
        private string _workingDir;

        private readonly IOutputFileSettings outputFileSettings = new OutputFileSettingsMock();
        private List<string> _files;

        public const string SkipSetup = "SkipSetup";

        //private IList<ISqlFile> _files;
        private IDbConnectionStringCollection _connCollection;

        private ITerminalExecFileBuilder<SqlScriptMock> _builder;

        [SetUp]
        public void SetUp()
        {
            BuildWorkingDir();
            BuildBaseDir();
            BuildConnectionManager();

            _settingsDir = SettingsFile.SettingsDirectoryPath;

            BuildExecFíleBuilder();
        }

        private void BuildWorkingDir()
        {
            _workingDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "script");
            if (!Directory.Exists(_workingDir))
                Directory.CreateDirectory(_workingDir);

            _execSqlFilePath = Path.Combine(_workingDir, outputFileSettings.FileNameExecutableSql);
            _execBatFilePath = Path.Combine(_workingDir, outputFileSettings.FileNameExecutable);

            TestContext.WriteLine($"working directory: {_workingDir}");
        }

        private void BuildBaseDir()
        {
            _baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "script_base");
            if (!Directory.Exists(_baseDir))
                Directory.CreateDirectory(_baseDir);

            _files = new List<string>
            {
                Path.Combine(_baseDir, "file1.sql"),
                Path.Combine(_baseDir, "file2.sql"),
                Path.Combine(_baseDir, "file3.sql")
            };

            foreach (var f in _files)
            {
                File.WriteAllText(f, "file text");
            }

            TestContext.WriteLine($"base directory: {_baseDir}");
        }

        private void BuildConnectionManager()
        {
            _connCollection = new DbConnectionStringCollectionMock();
            _connCollection.LoadFromString("user1/pw1@db1" + Environment.NewLine +
                "user2/pw2@db2" + Environment.NewLine +
                "user3/pw3@db3" + Environment.NewLine +
                "user4/pw2@db4" + Environment.NewLine +
                "FAKE");
        }

        private void BuildExecFíleBuilder()
        {
            _builder = new TerminalExecFileBuilder<TerminalCommandsBuilderMock, SqlCommandsBuilderMock, SqlScriptMock>()
            {
                SqlFilePaths = _files,
                AddOnScriptsManager = new AddOnScriptsManagerMock(),
                ConnectionStrings = _connCollection.DbConnectionStrings,
                EmptyDirectory = _workingDir,
                OutputFileSettings = outputFileSettings
            };
        }

        [TearDown]
        public void TearDown()
        {
            Directory.Delete(_baseDir, true);
            DeleteAllFilesInWorkAndSettingsPath();
        }

        private void DeleteAllFilesInWorkAndSettingsPath()
        {
            FileSupport.DeleteAllFilesInDirectory(_workingDir);
            FileSupport.DeleteAllFilesInDirectory(_settingsDir);
        }

        [Test]
        [Category(SkipSetup)]
        public void CheckBuild()
        {
            var builderTest = new TerminalExecFileBuilder<TerminalCommandsBuilderMock, SqlCommandsBuilderMock, SqlScriptMock>()
            {
                SqlFilePaths = _files,
                AddOnScriptsManager = GetAddScriptMock("a", "b"),
                ConnectionStrings = _connCollection.DbConnectionStrings,
                EmptyDirectory = _workingDir,
                OutputFileSettings = outputFileSettings
            };
        }

        private static AddOnScriptsManagerMock GetAddScriptMock(string preScriptText, string postScriptText)
        {
            var addOnMock = new AddOnScriptsManagerMock();
            addOnMock.SetPreScript(preScriptText);
            addOnMock.SetPostScript(postScriptText);
            return addOnMock;
        }

        [Test]
        public void ThrowExceptionOnNotEmptyDirectory()
        {
            File.WriteAllText(Path.Combine(_workingDir, "foo.txt"), "bar");

            Assert.Throws<EDirectoryNotEmpty>(() =>
            {
                var b = new TerminalExecFileBuilder<TerminalCommandsBuilderMock, SqlCommandsBuilderMock, SqlScriptMock>();
                b.EmptyDirectory = _workingDir;
            });
        }

        [Test]
        public void TestFilesCopied()
        {
            _builder.BuildExecutionFiles();

            Assert.Multiple(() =>
            {
                int i = 0;
                foreach (var f in _files)
                {
                    var expectedFilePath = Path.Combine(_workingDir,
                        $"{outputFileSettings.SqlScriptName}{++i}{outputFileSettings.SqlScriptExtension}");
                    Assert.IsTrue(File.Exists(expectedFilePath), "script was not at expected Position");
                }
            });
        }

        [Test]
        public void TestBuildScript()
        {
            const string preScriptText = "pre script test";
            const string postScriptText = "post script test";

            _builder.AddOnScriptsManager = GetAddScriptMock(preScriptText, postScriptText);
            _builder.BuildExecutionFiles();

            IList<string> awaitedCommands = new List<string>();

            awaitedCommands = AddSqlFilesToCommands(awaitedCommands, _builder);

            AssertAreFileAndTextEqual(_execSqlFilePath,
                string.Join(Environment.NewLine, awaitedCommands),
                "execution script doesn't match");
        }

        private IList<string> AddSqlFilesToCommands(IList<string> commands, ITerminalExecFileBuilder<SqlScriptMock> builder)
        {
            int i = 0;
            string getFileNameCall() =>
                $"@{outputFileSettings.SqlScriptName}{++i}{outputFileSettings.SqlScriptExtension}";

            if (builder.AddOnScriptsManager?.HasPreScript() == true)
            {
                commands.Add($"prompt executing {builder.AddOnScriptsManager.PreScript.Name}");
                commands.Add(getFileNameCall());
            }

            foreach (var f in _files)
            {
                commands.Add($"prompt executing {Path.GetFileName(f)}");
                commands.Add(getFileNameCall());
            }

            if (builder.AddOnScriptsManager?.HasPostScript() == true)
            {
                commands.Add($"prompt executing {builder.AddOnScriptsManager.PostScript.Name}");
                commands.Add(getFileNameCall());
            }

            return commands;
        }

        private void AssertAreFileAndTextEqual(string path, string text, string misMatchText)
        {
            Assert.True(File.Exists(path), "file doesn't exist");

            string file = File.ReadAllText(path);

            TestContext.WriteLine("awaited" + Environment.NewLine + text);
            TestContext.WriteLine("got" + Environment.NewLine + file);

            Assert.AreEqual(text.Trim(), file.Trim(), misMatchText);
        }

        [Test]
        public void TestBuildBatchFile()
        {
            _builder.BuildExecutionFiles();

            var awaitedCommands = new List<string>();

            foreach (var c in _connCollection.DbConnectionStrings)
            {
                if (c.IsValidConnection())
                {
                    awaitedCommands.AddRange(
                        getAwaitedTerminalCommands(c, outputFileSettings.FileNameExecutableSql));
                }
            }

            AssertAreFileAndTextEqual(_execBatFilePath,
                string.Join(Environment.NewLine, awaitedCommands),
                "execution batch doesn't match");
        }

        private static IEnumerable<string> getAwaitedTerminalCommands(IDbConnectionString c, string fileName)
        {
            string logFilePath =
                TerminalExecFileBuilder<TerminalCommandsBuilderMock, SqlCommandsBuilderMock, SqlScriptMock>
                .LogFilePath;
            var commands = new List<string>
            {
                $"header",
                $"connstring: {c.ConnectionString} || log to: {logFilePath} || {fileName}"
            };

            return commands;
        }

        [Test]
        public void TestBuildNoBatchFileWoConnections()
        {
            _builder.ConnectionStrings = null;
            _builder.BuildExecutionFiles();

            Assert.False(File.Exists(_execBatFilePath), "Batch file should not be build, without connectionstrings!");
        }
    }
}
