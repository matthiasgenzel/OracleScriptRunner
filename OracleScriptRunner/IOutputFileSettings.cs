namespace OracleScriptRunner
{
    public interface IOutputFileSettings
    {
        string FileNameExecutable { get; }
        string SqlScriptExtension { get; }
        string FileNameExecutableSql { get; }
        string SqlScriptName { get; }
    }
}