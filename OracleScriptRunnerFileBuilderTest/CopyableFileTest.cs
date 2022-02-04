using NUnit.Framework;
using OracleScriptRunnerFileBuilder.Impl;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace OracleScriptRunnerFileBuilderTest
{
    internal class CopyableFileTest
    {
        string workDirectoryPath = string.Empty;
        string copyDirectoryPath = string.Empty;

        [SetUp]
        public void Setup()
        {
            var processFileName = Process.GetCurrentProcess().MainModule!.FileName;

            workDirectoryPath =
                Path.Combine(Path.GetDirectoryName(processFileName)!, "test");
            if (!Directory.Exists(workDirectoryPath))
                Directory.CreateDirectory(workDirectoryPath);

            copyDirectoryPath =
                Path.Combine(Path.GetDirectoryName(processFileName)!, "testCopy");
            if (!Directory.Exists(copyDirectoryPath))
                Directory.CreateDirectory(copyDirectoryPath);
        }

        [TearDown]
        public void TearDown()
        {
            Directory.Delete(workDirectoryPath, true);
            Directory.Delete(copyDirectoryPath, true);
        }

        private List<CopyableFile> GetTestFiles()
        {
            const int cntFiles = 10;
            var files = new List<CopyableFile>();

            for (int i = 0; i < cntFiles; i++)
            {
                var f = new CopyableFile();
                f.InitPath(Path.Combine(workDirectoryPath, $"file_{i}"));
                files.Add(f);

                File.WriteAllText(f.OriginalPath, i.ToString());
            }

            return files;
        }

        [Test]
        public void FileCopyWorks()
        {
            // build files
            var files = GetTestFiles();

            // copy files
            int i = 0;
            files.ForEach((f) =>
            {
                f.CopyTo(copyDirectoryPath, $"script_{i++}");
            });

            int cntFiles = 0;

            Assert.Multiple(() =>
            {
                // check if both files are there
                files.ForEach(f => AssertFilesExist(f, cntFiles++));

                files.ForEach((f) =>
                {
                    Assert.AreEqual(File.ReadAllText(f.OriginalPath), File.ReadAllText(f.CurrentPath), "files must be equal");
                });
            });
        }

        private void AssertFilesExist(CopyableFile f, int noFiles)
        {
            Assert.IsTrue(File.Exists(f.OriginalPath), "original file must exist");
            Assert.IsTrue(File.Exists(f.CurrentPath), "current file must exist");

            Assert.AreEqual(noFiles.ToString(), File.ReadAllText(f.CurrentPath), "file number must be file text");
            Assert.AreEqual(File.ReadAllText(f.OriginalPath), File.ReadAllText(f.CurrentPath), "file texts must math");
        }

        [Test]
        public void InitPathWorks()
        {
            var path = "test/Path/123";

            var f = new CopyableFile();
            f.InitPath(path);

            Assert.AreEqual(path, f.OriginalPath, "init must set both paths to the same path");
            Assert.AreEqual(path, f.CurrentPath, "init must set both paths to the same path");
        }

        [Test]
        public void ToStringReturnsOriginalPath()
        {
            var path = "test/Path/123";

            var f = new CopyableFile()
            {
                OriginalPath = path
            };

            Assert.AreEqual(path, f.ToString(), "toString must return the original path");
        }

        [Test]
        public void TextToFileWorks()
        {
            string testText = "as0cßh912hreoinoinhc9819";
            string path = Path.Combine(copyDirectoryPath, "test.sql");

            var f = new CopyableFile
            {
                Text = testText
            };

            f.CopyTo(copyDirectoryPath, path);

            Assert.AreEqual(testText, File.ReadAllText(path));
        }
    }
}
