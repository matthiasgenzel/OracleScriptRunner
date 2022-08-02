using NUnit.Framework;
using OracleScriptRunnerCmdSqlplus;
using OracleScriptRunnerCmdSqlplusTest.Mocks;
using System;
using System.Collections.Generic;

namespace OracleScriptRunnerCmdSqlplusTest
{
    public class SqlPlusCommandBuilderTest
    {
        private readonly string orgPathName = "orgpath_";
        private readonly string currentPathName = "currentpath_";
        private readonly int cntFiles = 10;

        private SqlPlusCommandBuilder GetBuilder()
        {
            var builder = new SqlPlusCommandBuilder();
            var files = new List<SqlScriptMock>();
            for (int i = 0; i < cntFiles; i++)
            {
                files.Add(new SqlScriptMock()
                {
                    PromptName = orgPathName + i.ToString(),
                    FilePath = currentPathName + i.ToString()
                });
            }

            builder.Files = files;
            return builder;
        }

        [Test]
        public void TestGetAllCommands()
        {
            var builder = GetBuilder();
            var commands = builder.GetCommands(true);

            Assert.AreEqual(cntFiles * 3, commands.Count, "wrong number of sql commands");
            Assert.Multiple(() =>
            {
                int i = 0;
                foreach (var file in builder.Files)
                {
                    Assert.AreEqual($"prompt executing {file.PromptName}", commands[i * 3]);
                    Assert.AreEqual($"@{file.FilePath}", commands[i * 3 + 1]);
                    Assert.AreEqual($"/", commands[i * 3 + 2]);

                    i++;
                }
            });
        }

        [Test]
        public void TestGetAllCommandsWoHeader()
        {
            var builder = GetBuilder();
            var commands = builder.GetCommands(false);

            Assert.AreEqual(cntFiles * 2, commands.Count, "wrong number of sql commands");
            Assert.Multiple(() =>
            {
                int i = 0;
                foreach (var file in builder.Files)
                {
                    Assert.AreEqual($"@{file.FilePath}", commands[i * 2]);
                    Assert.AreEqual($"/", commands[i * 2 +1]);

                    i++;
                }
            });
        }

        [Test]
        public void TestGetAllCommandsAsText()
        {
            var builder = GetBuilder();
            var text = builder.GetCommandsAsFileText(true);

            var testText = string.Empty;

            foreach (var file in builder.Files)
            {
                testText += $"prompt executing {file.PromptName}" + Environment.NewLine;
                testText += $"@{file.FilePath}" + Environment.NewLine;
                testText += $"/" + Environment.NewLine;
            }

            Assert.AreEqual(testText.Trim(), text);
        }
    }
}