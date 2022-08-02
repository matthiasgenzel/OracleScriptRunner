using NUnit.Framework;
using OracleScriptRunnerFileRunner;
using OracleScriptRunnerTestSupport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OracleScriptRunnerFileRunnerTest
{
    internal class SqlFileRunnerTest
    {
        internal static string ExecCommand = @"sqlplus";
        private string testConnection = "foo/bar@fakedb";
        private string sqlFilePath = "execute_sql.sql";

        [Test]
        public void TestScriptExecuted()
        {
            var fileRunner = new SqlFileRunner<
                TerminalCommandsBuilderMock<TerminalCommandMock>, 
                TerminalRunnerMock,
                TerminalOutputMock>()
            {
                SqlFilePath = sqlFilePath
            };
            
            var result = fileRunner.ExecuteForDbConnection(new DbConnectionStringMock()
            {
                ConnectionString = testConnection
            });
                        
            TestContext.WriteLine(result.Output);

            Assert.IsTrue(result.Output.Contains(ExecCommand));
            Assert.IsTrue(result.Output.Contains(testConnection));
            Assert.IsTrue(result.Output.Contains(sqlFilePath));
        }
    }
}
