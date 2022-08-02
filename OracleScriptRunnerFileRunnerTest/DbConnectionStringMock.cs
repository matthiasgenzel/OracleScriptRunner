using OracleScriptRunner.Settings;

namespace OracleScriptRunnerFileRunnerTest
{
    internal class DbConnectionStringMock : IDbConnectionString
    {
        public string ConnectionString { get; set; }

        public bool IsValidConnection()
        {
            return true;
        }
    }
}
