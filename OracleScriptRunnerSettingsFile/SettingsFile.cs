using OracleScriptRunner.Settings;
using System;
using System.Diagnostics;
using System.IO;

namespace OracleScriptRunnerSettingsFile
{
    public class SettingsFile : ISetting
    {
        public static string SettingsDirectoryPath { get; private set; }

        static SettingsFile()
        {
            var processFileName = Process.GetCurrentProcess().MainModule!.FileName;
            SettingsDirectoryPath =
                Path.Combine(Path.GetDirectoryName(processFileName)!, "settings");
            if (!Directory.Exists(SettingsDirectoryPath))
                Directory.CreateDirectory(SettingsDirectoryPath);
        }

        public string Name { get; set; }
        public string Text { get; set; }

        public string FullFilePath => Path.Combine(SettingsDirectoryPath, Name);

        private string GetFileText()
        {
            if (Exists())
                return File.ReadAllText(FullFilePath);

            return null;
        }

        public void Load()
        {
            Text = GetFileText();
        }

        public void Save()
        {
            if (HasfileChanged())
            {
                if (Exists())
                    BackupOldFile();

                File.WriteAllText(FullFilePath, Text);
            }
        }

        private bool HasfileChanged()
        {
            return !Exists() || Text?.Equals(GetFileText() ?? string.Empty) == false;
        }

        private void BackupOldFile()
        {
            File.Move(FullFilePath, FullFilePath + ".old_" + DateTime.Now.ToString("yyyyMMddHHmmssfff"));
        }

        public bool Exists()
        {
            return File.Exists(FullFilePath);
        }

    }
}
