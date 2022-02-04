namespace OracleScriptRunner
{
    public interface ISqlScript
    {
        public string PromptName { get; set; }
        public string FilePath { get; set; }
    }
}
