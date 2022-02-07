using NUnit.Framework;
using OracleScriptRunner.Settings;
using OracleScriptRunnerCmdSqlplus;
using OracleScriptRunnerCmdSqlplusTest.Mocks;
using System.IO;
using System.Linq;

namespace OracleScriptRunnerCmdSqlplusTest
{
    internal class WinCmdCommandsBuilderTest
    {
        WinCmdCommandsBuilder winCmdCommandsBuilder;
        private string logFilePath = "logFile.txt";
        private string sqlFilePath = @"C:\foo\bar\fake\script.sql";
        private IDbConnectionString dbConnectionString;

        [SetUp]
        public void SetUp()
        {
            winCmdCommandsBuilder = new WinCmdCommandsBuilder()
            {
                LogFilePath = logFilePath,
                SqlFilePath = sqlFilePath
            };

            dbConnectionString = new DbConnectionStringMock()
            {
                ConnectionString = "foo/bar@fakedb"
            };
        }

        private string GetHeaderLine(bool useLog)
        {
            if (useLog)
                return $"echo {dbConnectionString.ToString()} >> {logFilePath}";
            else
                return $"echo {dbConnectionString.ToString()}";
        }

        private string GetExecLine(bool useLog, bool fullFilePath)
        {
            var filePath = string.Empty;
            var firstCommand = string.Empty;

            if (!string.IsNullOrEmpty(winCmdCommandsBuilder.SqlFilePath))
            {
                filePath = "@" + (fullFilePath ? sqlFilePath : Path.GetFileName(sqlFilePath));
                firstCommand = "echo quit | ";
            }

            if (useLog)
                return $"{firstCommand}sqlplus -L -S {dbConnectionString.ConnectionString} {filePath} >> {logFilePath}";
            else
                return $"{firstCommand}sqlplus -L -S {dbConnectionString.ConnectionString} {filePath}";
        }

        [Test]
        public void FullTerminalCommands()
        {
            var cmd = winCmdCommandsBuilder.GetTerminalCommands(
                dbConnectionString: dbConnectionString,
                useFullFileName: true,
                doWriteHeader: true)
                .Select(c => c.Text)
                .ToArray();

            Assert.AreEqual(GetHeaderLine(true).Trim(), cmd[0]);
            Assert.AreEqual(GetExecLine(true, true).Trim(), cmd[1]);
        }

        [Test]
        public void TerminalCommandsWOHeader()
        {
            var cmd = winCmdCommandsBuilder.GetTerminalCommands(
                dbConnectionString: dbConnectionString, 
                useFullFileName: true, 
                doWriteHeader: false)
                .Select(c => c.Text)
                .ToArray();

            Assert.AreEqual(GetExecLine(useLog: true, fullFilePath: true).Trim(), cmd[0]);
        }

        [Test]
        public void TerminalCommandsWithShortFileName()
        {
            var cmd = winCmdCommandsBuilder.GetTerminalCommands(
                dbConnectionString: dbConnectionString,
                useFullFileName: false,
                doWriteHeader: true)
                .Select(c => c.Text)
                .ToArray();

            Assert.AreEqual(GetHeaderLine(useLog: true).Trim(), cmd[0]);
            Assert.AreEqual(GetExecLine(useLog: true, fullFilePath: false).Trim(), cmd[1]);
        }

        [Test]
        public void TerminalCommandsWOFileName()
        {
            winCmdCommandsBuilder.SqlFilePath = string.Empty;
            winCmdCommandsBuilder.LogFilePath = string.Empty;

            var cmd = winCmdCommandsBuilder.GetTerminalCommands(
                dbConnectionString: dbConnectionString,
                useFullFileName: false,
                doWriteHeader: true)
                .Select(c => c.Text)
                .ToArray();

            Assert.AreEqual(GetHeaderLine(useLog: false).Trim(), cmd[0]);
            Assert.AreEqual(GetExecLine(useLog: false, fullFilePath: false).Trim(), cmd[1]);
        }
    }
}
