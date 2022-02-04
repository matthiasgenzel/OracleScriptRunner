using NUnit.Framework;
using OracleScriptRunnerSettingsFile;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OracleScriptRunnerSettingsFileTest
{
    public class SettingsFileTests
    {
        private string _settingsDirectoryPath;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _settingsDirectoryPath = SettingsFile.SettingsDirectoryPath;
            TestContext.WriteLine($"settings directory: {_settingsDirectoryPath}");
        }

        [SetUp]
        public void SetUp()
        {
            DeleteAllFilesInSettingsPath();
        }

        private void DeleteAllFilesInSettingsPath()
        {
            var filePaths = Directory.GetFiles(_settingsDirectoryPath);
            foreach (var filePath in filePaths)
                File.Delete(filePath);
        }

        public void AssertFileIsEqual(string text, string path)
        {
            Assert.IsTrue(File.Exists(path), $"file {path} doesn't exist");

            var currentText = File.ReadAllText(path);
            Assert.AreEqual(text, currentText, "text does not match");
        }

        [Test]
        public void SaveNewFile()
        {
            const string testText = "foo bar";
            var filePath = Path.Combine(_settingsDirectoryPath, "settings.txt");
            var f = new SettingsFile
            {
                Name = filePath,
                Text = testText
            };

            f.Save();

            AssertFileIsEqual(testText, filePath);
        }
        [Test]
        public void ReadFile()
        {
            const string testText = "foo bar foo bar";
            var fileName = "file.txt";
            var filePath = Path.Combine(_settingsDirectoryPath, fileName);

            File.WriteAllText(filePath, testText);

            var f = new SettingsFile
            {
                Name = filePath
            };
            f.Load();

            Assert.AreEqual(testText, f.Text, "loaded text doesn't match test");
        }
        [Test]
        public void OverrideFile()
        {
            const string oldText = "foo bar";
            const string newText = "foo bar 2";

            const string fileName = "settings.txt";
            var filePath = Path.Combine(_settingsDirectoryPath, fileName);
            var f = new SettingsFile
            {
                Name = filePath
            };

            f.Text = oldText;
            f.Save();

            f.Text = newText;
            f.Save();

            Assert.IsTrue(File.Exists(filePath), $"settings file doesn't exist {filePath}");
            Assert.AreEqual(newText, File.ReadAllText(filePath), "current file doesn't match text");

            var backupFilePath = GetBackupFileNames(fileName).FirstOrDefault();
            Assert.IsTrue(File.Exists(backupFilePath), $"backup file doesn't exist {backupFilePath}");
            Assert.AreEqual(oldText, File.ReadAllText(backupFilePath), "backup file doesn't match text");
        }

        [Test]
        public void NoSaveOnUnchangedFile()
        {
            // build file
            const string oldText = "foo bar";

            const string fileName = "settings.txt";
            var filePath = Path.Combine(_settingsDirectoryPath, fileName);
            var f = new SettingsFile
            {
                Name = filePath
            };

            f.Text = oldText;
            f.Save();

            // wait shortly - rewrite with same text
            var dateTimeLastWrite = File.GetLastWriteTime(filePath);

            System.Threading.Thread.Sleep(5);

            f.Text = oldText;
            f.Save();

            Assert.AreEqual(dateTimeLastWrite, File.GetLastWriteTime(filePath), "file written, without text changes");
        }

        private IEnumerable<string> GetBackupFileNames(string fileName)
        {
            var dir = new DirectoryInfo(_settingsDirectoryPath);
            return dir.GetFiles(Path.GetFileName(fileName + ".old_*")!).Select(f => f.FullName);
        }

        [Test]
        public void FileExistsWorks()
        {
            const string testText = "foo bar";
            var filePath = Path.Combine(_settingsDirectoryPath, "settings.txt");
            var f = new SettingsFile
            {
                Name = filePath,
                Text = testText
            };

            Assert.IsFalse(f.Exists(), "file shouldn't exist, yet");

            f.Save();

            Assert.IsTrue(f.Exists(), "file should exist now");
        }
    }
}