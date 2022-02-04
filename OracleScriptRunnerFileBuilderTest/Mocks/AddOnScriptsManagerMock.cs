using OracleScriptRunner.Settings;

namespace OracleScriptRunnerFileBuilderTest.Mocks
{
    class AddOnScriptsManagerMock : IAddOnScriptsManager
    {
        public ISetting PostScript { get; set; }

        public ISetting PreScript { get; set; }

        public AddOnScriptsManagerMock()
        {
            PreScript = new SettingMock() { Name = "pre.sql" };
            PostScript = new SettingMock() { Name = "post.sql" };
        }

        public bool HasPostScript()
        {
            return !string.IsNullOrEmpty(PostScript.Text);
        }

        public bool HasPreScript()
        {
            return !string.IsNullOrEmpty(PreScript.Text);
        }

        public void SetPostScript(string scriptText)
        {
            PostScript.Text = scriptText;
        }

        public void SetPreScript(string scriptText)
        {
            PreScript.Text = scriptText;
        }
    }
}
