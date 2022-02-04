using NUnit.Framework;
using OracleScriptRunnerSettings;
using System.Linq;

namespace OracleScriptRunnerSettingsTest
{
    internal class DbConnectionStringTests
    {
        [Test]
        public void RecognizedValidConnectionStrings()
        {
            string[] validConnectionStrings = { "user/pw@db", "scott/tiger@(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=sales-server1)(PORT=1521))\r\n(CONNECT_DATA=(SERVICE_NAME=sales.us.acme.com)))", "user /password@//hostname/service_name", "user/password@//hostname:port/service_name" };
            string[] invalidConnectionStrings = { "foobar", "foo@bar", "foo/bar" };

            Assert.Multiple(() =>
            {
                Assert.True(validConnectionStrings.All(c =>
                {
                    var connection = new DbConnectionString()
                    {
                        ConnectionString = c
                    };

                    return connection.IsValidConnection();
                }), "Valid connections are not recognized");

                Assert.True(!invalidConnectionStrings.Any(c =>
                {
                    var connection = new DbConnectionString()
                    {
                        ConnectionString = c
                    };

                    return connection.IsValidConnection();
                }), "Invalid connections are not recognized");
            });
        }
        [Test]
        public void CheckToStringHasNoPws()
        {
            var c = new DbConnectionString()
            {
                ConnectionString = "abc/pw@test"
            };

            Assert.That(c.ToString(), !Contains.Substring("pw"));
            Assert.That(c.ToString(), !Contains.Substring("/"));
        }
    }
}
