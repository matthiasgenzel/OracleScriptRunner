using OracleScriptRunner.Settings;

namespace OracleScriptRunnerSettings
{
    public class AddOnScriptsManager : IAddOnScriptsManager
    {
        public ISetting PreScript { get; }
        public ISetting PostScript { get; }

        public AddOnScriptsManager(ISetting preScriptSetting, ISetting postScriptSetting)
        {
            PreScript = preScriptSetting;
            PreScript.Load();

            PostScript = postScriptSetting;
            PostScript.Load();
        }

        public void SetPreScript(string scriptText)
        {
            PreScript.Text = scriptText;
            PreScript.Save();
        }

        public bool HasPreScript()
        {
            return PreScript.Exists() && !string.IsNullOrEmpty(PreScript.Text);
        }

        public void SetPostScript(string scriptText)
        {
            PostScript.Text = scriptText;
            PostScript.Save();
        }

        public bool HasPostScript()
        {
            return PostScript.Exists() && !string.IsNullOrEmpty(PostScript.Text);
        }
    }
}
