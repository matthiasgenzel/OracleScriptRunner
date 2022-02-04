using OracleScriptRunner.Settings;

namespace OracleScriptRunnerFileBuilderTest.Mocks
{
    internal class SettingMock : ISetting
    {
        public string Name { get; set; }
        public string Text { get; set; }

        public bool Exists()
        {
            return !string.IsNullOrEmpty(Name);
        }

        public void Load()
        {
            // intentionally blank
        }

        public void Save()
        {
            // intentionally blank
        }
    }
}
