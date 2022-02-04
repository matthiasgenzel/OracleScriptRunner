using OracleScriptRunner.Settings;

namespace OracleScriptRunnerSettingsTest.Mocks
{
    internal class SettingMock : ISetting
    {
        public string Name { get; set; }
        public string Text { get; set; }

        public bool Exists() => Name != null;

        public void Load()
        {
        }

        public void Save()
        {
        }
    }
}