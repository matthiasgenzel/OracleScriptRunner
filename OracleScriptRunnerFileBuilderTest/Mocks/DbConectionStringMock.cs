using OracleScriptRunner.Settings;

namespace OracleScriptRunnerFileBuilderTest.Mocks
{
    internal class DbConectionStringMock : IDbConnectionString
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