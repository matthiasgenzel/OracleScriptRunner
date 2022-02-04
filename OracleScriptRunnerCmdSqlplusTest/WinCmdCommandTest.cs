using NUnit.Framework;
using OracleScriptRunnerCmdSqlplus;
using System.Collections.Generic;

namespace OracleScriptRunnerCmdSqlplusTest
{
    internal class WinCmdCommandTest
    {
        [Test]
        public void TestFindsExecutableCommand()
        {
            var commands = new List<WinCmdCommand>
            {
                new WinCmdCommand() { Text = "123sqlplus 8ud198oci" },
                new WinCmdCommand() { Text = "sqlplus 8ud198oci" },
                new WinCmdCommand() { Text = "echo bla sqlplus 8ud198oci" }
            };

            commands.ForEach(command => Assert.IsTrue(command.IsExecutingCommand()));
        }

        [Test]
        public void TestFindsNotExecutableCommand()
        {
            var commands = new List<WinCmdCommand>
            {
                new WinCmdCommand() { Text = "123sql 8ud198oci" },
                new WinCmdCommand() { Text = "@blub.sql" },
                new WinCmdCommand() { Text = "foo bar" }
            };

            commands.ForEach(command => Assert.IsFalse(command.IsExecutingCommand()));
        }
    }
}
