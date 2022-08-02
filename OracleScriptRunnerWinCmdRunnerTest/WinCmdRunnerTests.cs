using NUnit.Framework;
using System;
using System.IO;
using OracleScriptRunnerWinCmdRunner;
using System.Collections.Generic;
using OracleScriptRunnerFileRunner;
using OracleScriptRunner;

namespace OracleScriptRunnerWinCmdRunnerTest
{
    public class WinCmdRunnerTests
    {
        internal static string WorkPath => Path.Combine(Path.GetTempPath(), "ScriptRunnerTest");
        internal static string ExecFilePath1 => Path.Combine(WorkPath, "exec1.bat");
        internal static string ExecFilePath2 => Path.Combine(WorkPath, "exec2.bat");

        private string _echo1 = "asdc09ue901";
        private string _echo2 = "c0jinaopj212";
        private string _echo3 = "vu798hfoihn";
        private string _echo4 = "0vjoadjl";

        [SetUp]
        public void SetUp()
        {
            DeleteWorkDir();
            BuildFiles();            
        }

        public void BuildFiles ()
        {
            Directory.CreateDirectory(WorkPath);

            var fileText = "echo " + _echo1 + Environment.NewLine
                + "PING localhost -n 2 >NUL" + Environment.NewLine
                + "echo " + _echo2
                + "echo %1";
            File.WriteAllText(ExecFilePath1, fileText);

            fileText = "@echo " + _echo3 + Environment.NewLine
                + "echo " + _echo4
                + "echo %1";
            File.WriteAllText(ExecFilePath2, fileText);
        }

        [TearDown]
        public void TearDown()
        {
            DeleteWorkDir();
        }

        private void DeleteWorkDir()
        {
            if (Directory.Exists(WorkPath))
                Directory.Delete(WorkPath, true);
        }

        [Test]
        public void TestScriptExecuted()
        {
            var fileRunner = new WinCmdRunner();

            var result = fileRunner.RunProgram(new List<ITerminalCommand>()
            {
                new TerminalCommandMock() { Text = ExecFilePath1 },
                new TerminalCommandMock() { Text = ExecFilePath2 }
            });

            TestContext.WriteLine(result);

            Assert.Multiple(() =>
            {
                // new line to find just echoed text
                Assert.IsTrue(result.Contains(Environment.NewLine + _echo1));
                Assert.IsTrue(result.Contains(Environment.NewLine + _echo2));
                Assert.IsTrue(result.Contains(Environment.NewLine + _echo3));
                Assert.IsTrue(result.Contains(Environment.NewLine + _echo4));
            });
        }
    }
}