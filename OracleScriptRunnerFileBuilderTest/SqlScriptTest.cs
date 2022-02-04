using NUnit.Framework;
using OracleScriptRunnerFileBuilder.Impl;

namespace OracleScriptRunnerFileBuilderTest
{
    internal class SqlScriptTest
    {
        [Test]
        public void TestInit()
        {
            var path = "file/path/script.sql";
            var label = "this is a label";

            SqlScript script = new SqlScript()
            {
                FilePath = path,
                PromptName = label
            };

            Assert.AreEqual(path, script.FilePath);
            Assert.AreEqual(label, script.PromptName);
        }

        [Test]
        public void TestToStringReturnsFilePath()
        {
            var path = "file/path/script.sql";
            var label = "this is a label";

            SqlScript script = new SqlScript()
            {
                FilePath = path,
                PromptName = label
            };

            Assert.AreEqual(path, script.ToString());
        }
    }
}
