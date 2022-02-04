using OracleScriptRunner.Settings;
using System.Collections.Generic;
using System.IO;

namespace OracleScriptRunnerFileBuilderTest.Mocks
{
    internal class DbConnectionStringCollectionMock : IDbConnectionStringCollection
    {
        public IList<IDbConnectionString> DbConnectionStrings { get; } = new List<IDbConnectionString>();

        public void LoadFromSetting()
        {
            throw new System.NotImplementedException();
        }

        public void LoadFromString(string s)
        {
            DbConnectionStrings.Clear();
            using var reader = new StringReader(s);

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Length > 0)
                {
                    var connection = new DbConectionStringMock()
                    {
                        ConnectionString = line
                    };

                    DbConnectionStrings.Add(connection);
                }
            }
        }

        public void SaveToSetting()
        {
            throw new System.NotImplementedException();
        }
    }
}