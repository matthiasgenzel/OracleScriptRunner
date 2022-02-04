using OracleScriptRunner.Settings;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("OracleScriptRunnerSettingsTest")]
namespace OracleScriptRunnerSettings
{
    internal class DbConnectionString : IDbConnectionString
    {
        /*
         * sqlplus allows a lot of connections strings
         * they have just the / and @ as common characters
         * we simply allow everything and parse for those chars
         */
        public string ConnectionString { get; set; }


        public bool IsValidConnection()
        {
            return ConnectionString.Contains("/") && ConnectionString.Contains("@");
        }

        public override string ToString()
        {
            var indexOfSlash = ConnectionString.IndexOf("/");
            var indexOfAt = ConnectionString.IndexOf("@");

            if (indexOfSlash > 0 && indexOfAt > 0)
            {
                var userName = ConnectionString[..indexOfSlash];
                var db = ConnectionString[indexOfAt..];
                return userName + db;
            }
            return ConnectionString;
        }
    }
}
