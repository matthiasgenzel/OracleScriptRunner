using System.Collections.Generic;

namespace OracleScriptRunner
{
    public interface ISqlCommandsBuilder
    {
        IEnumerable<ISqlScript> Files { get; set; }

        public string GetCommandsAsFileText(bool withHeaderPrompt);
        public IList<string> GetCommands(bool withHeaderPrompt);
    }
}