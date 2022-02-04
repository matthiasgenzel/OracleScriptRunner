using OracleScriptRunner.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OracleScriptRunnerSettings
{
    public class DbConnectionStringCollection : IDbConnectionStringCollection
    {
        private readonly ISetting _setting;

        public IList<IDbConnectionString> DbConnectionStrings { get; }

        public DbConnectionStringCollection(ISetting setting)
        {
            DbConnectionStrings = new List<IDbConnectionString>();
            _setting = setting;
        }

        public void LoadFromSetting()
        {
            DbConnectionStrings.Clear();

            if (_setting.Exists())
            {
                _setting.Load();
                LoadFromString(_setting.Text);
            }
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
                    var connection = new DbConnectionString
                    {
                        ConnectionString = line
                    };

                    DbConnectionStrings.Add(connection);
                }
            }
        }
        public void SaveToSetting()
        {
            _setting.Text = string.Join(Environment.NewLine, DbConnectionStrings.Select((c) => c.ConnectionString));
            _setting.Save();
        }
    }
}
