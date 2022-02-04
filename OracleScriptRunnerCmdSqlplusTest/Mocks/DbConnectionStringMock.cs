using OracleScriptRunner.Settings;

namespace OracleScriptRunnerCmdSqlplusTest.Mocks
{
    internal class DbConnectionStringMock : IDbConnectionString
    {
        public string ConnectionString { get; set; }

        public bool IsValidConnection()
        {
            return true;
        }

        public override string ToString()
        {
            return ConnectionString;
        }
    }
}