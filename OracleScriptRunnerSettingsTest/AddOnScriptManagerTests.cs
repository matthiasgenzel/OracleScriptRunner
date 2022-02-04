using NUnit.Framework;
using OracleScriptRunnerSettings;
using OracleScriptRunnerSettingsTest.Mocks;

namespace OracleScriptRunnerSettingsTest
{
    internal class AddOnScriptManagerTests
    {
        AddOnScriptsManager _manager;
        const string ScriptText = "select * from dual";

        [SetUp]
        public void SetUp()
        {
            _manager = new AddOnScriptsManager(
                new SettingMock()
                {
                    Name = "pre"
                }, new SettingMock()
                {
                    Name = "post"
                });
        }

        [Test]
        public void BuildAndSavePreScript()
        {
            _manager.SetPreScript(ScriptText);

            Assert.Multiple(() =>
            {
                Assert.True(_manager.HasPreScript(), "no pre script found");
                Assert.AreEqual(ScriptText, _manager.PreScript.Text, "pre script text is not as expected");
            });
        }

        [Test]
        public void BuildAndSavePostScript()
        {
            _manager.SetPostScript(ScriptText);

            Assert.Multiple(() =>
            {
                Assert.True(_manager.HasPostScript(), "no post script found");
                Assert.AreEqual(ScriptText, _manager.PostScript.Text, "post script text is not as expected");
            });
        }
    }
}
