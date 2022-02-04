namespace OracleScriptRunner.Settings
{
    public interface IDbConnectionString
    {
        string ConnectionString { get; set; }
        bool IsValidConnection();
        string ToString();
    }
}