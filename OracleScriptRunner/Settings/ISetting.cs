namespace OracleScriptRunner.Settings
{
    public interface ISetting
    {
        string Name { get; set; }
        string Text { get; set; }
        void Load();
        void Save();
        bool Exists();
    }
}