using System.Collections.Generic;

namespace OracleScriptRunner.Settings
{
    public interface IDbConnectionStringCollection
    {
        IList<IDbConnectionString> DbConnectionStrings { get; }
        void LoadFromSetting();
        void LoadFromString(string s);
        void SaveToSetting();
    }
}