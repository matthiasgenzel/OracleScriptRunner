namespace OracleScriptRunnerFileBuilder.App
{
    internal interface ICopyableFile
    {
        string OriginalPath { get; set; }
        string CurrentPath { get; set; }
        string Text { get; set; }
        void CopyTo(string newPath, string newFileName);
        string ToString();
    }
}