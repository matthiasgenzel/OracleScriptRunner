using System;

namespace OracleScriptRunnerFileBuilder
{
    [Serializable]
    public class EDirectoryNotEmpty : Exception
    {
        public string Directory { get; private set; }
        public EDirectoryNotEmpty() : base()
        {
        }
        public EDirectoryNotEmpty(string directory) : base(message: $"{directory} must be empty")
        {
            Directory = directory;
        }
    }
}