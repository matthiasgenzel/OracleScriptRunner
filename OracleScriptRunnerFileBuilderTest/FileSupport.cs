using System.IO;

namespace OracleScriptRunnerFileBuilderTest
{
    public static class FileSupport
    {
        public static void DeleteAllFilesInDirectory(string path)
        {
            var filePaths = Directory.GetFiles(path);
            foreach (var filePath in filePaths)
                File.Delete(filePath);
        }
    }
}
