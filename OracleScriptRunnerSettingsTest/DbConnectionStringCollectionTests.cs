using NUnit.Framework;
using OracleScriptRunnerSettings;
using OracleScriptRunnerSettingsTest.Mocks;
using System;

namespace OracleScriptRunnerSettingsTest
{
    internal class DbConnectionStringCollectionTests
    {
        private const string connectionStrings = @"foo/bar@a
foo2/bar@c
foo3/bar/d";


        private static string GetConnectionListString(string[] connections)
        {
            return string.Join(Environment.NewLine, connections);
        }

        private DbConnectionStringCollection GetDbConnectionStringCollection(string settingsText = null)
        {
            var setting = new SettingMock()
            {
                Name = "connections.txt",
                Text = settingsText
            };
            return new DbConnectionStringCollection(setting);
        }

        [Test]
        public void LoadConnectionsFromString()
        {
            var c = GetDbConnectionStringCollection();
            c.LoadFromString(connectionStrings);
            Assert.AreEqual(3, c.DbConnectionStrings.Count, "not all strings loaded");
        }

        [Test]
        public void LoadFromSettings()
        {
            var c = GetDbConnectionStringCollection(connectionStrings);
            c.LoadFromSetting();
            Assert.AreEqual(3, c.DbConnectionStrings.Count, "not all strings loaded");
        }

        [Test]
        public void SaveToSettings()
        {
            var c = GetDbConnectionStringCollection();
            c.LoadFromString(connectionStrings);
            c.SaveToSetting();
            c.LoadFromSetting();
            Assert.AreEqual(3, c.DbConnectionStrings.Count, "not all strings loaded");
        }
    }
}