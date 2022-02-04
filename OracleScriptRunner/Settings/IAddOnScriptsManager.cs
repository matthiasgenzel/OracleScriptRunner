namespace OracleScriptRunner.Settings
{
    public interface IAddOnScriptsManager
    {
        ISetting PostScript { get; }
        ISetting PreScript { get; }

        bool HasPostScript();
        bool HasPreScript();
        void SetPostScript(string scriptText);
        void SetPreScript(string scriptText);
    }
}