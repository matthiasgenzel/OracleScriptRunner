using OracleScriptRunner.Settings;
using System;

namespace OracleScriptRunnerSettingsTest.Mocks
{
    internal class DbConnectionStringMock : IDbConnectionString
    {
        public string ConnectionString { get; set; }

        public bool IsValidConnection()
        {
            throw new NotImplementedException();
        }
    }
}
