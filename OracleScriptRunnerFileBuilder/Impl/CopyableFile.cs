using OracleScriptRunnerFileBuilder.App;
using System.IO;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("OracleScriptRunnerFileBuilderTest")]
namespace OracleScriptRunnerFileBuilder.Impl
{
    internal class CopyableFile : ICopyableFile
    {
        public string OriginalPath { get; set; }
        public string CurrentPath { get; set; }
        public string Text { get; set; }

        public void InitPath(string path)
        {
            OriginalPath = path;
            CurrentPath = path;
        }

        public void CopyTo(string newPath, string newFileName)
        {
            CurrentPath = string.IsNullOrEmpty(newFileName)
                ? newPath
                : Path.Combine(newPath, newFileName);

            if (!string.IsNullOrEmpty(Text))
            {
                File.WriteAllText(CurrentPath, Text);
            }
            else
            {
                File.Copy(OriginalPath, CurrentPath);
            }
        }

        public override string ToString()
        {
            return OriginalPath;
        }
    }
}
