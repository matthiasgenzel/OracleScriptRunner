using OracleScriptRunner;

namespace OracleScriptRunnerFileBuilder.Impl
{
    public class SqlScript : ISqlScript
    {
        public string PromptName { get; set; }

        public string FilePath { get; set; }

        public override string ToString()
        {
            return FilePath;
        }
    }
}
