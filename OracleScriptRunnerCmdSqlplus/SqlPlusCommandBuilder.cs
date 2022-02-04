using OracleScriptRunner;
using System;
using System.Collections.Generic;
using System.IO;

namespace OracleScriptRunnerCmdSqlplus
{
    public class SqlPlusCommandBuilder : ISqlCommandsBuilder
    {
        public IEnumerable<ISqlScript> Files { get; set; }

        public IList<string> GetCommands(bool withHeaderPrompt)
        {
            IList<string> sqlCommands = new List<string>();

            foreach (var f in Files)
            {
                if (withHeaderPrompt && !string.IsNullOrEmpty(f.PromptName))
                    sqlCommands.Add(
                        $"prompt executing {Path.GetFileName(f.PromptName)}");

                sqlCommands.Add($"@{Path.GetFileName(f.FilePath)}");
            }

            return sqlCommands;
        }

        public string GetCommandsAsFileText(bool withHeaderPrompt)
        {
            return string.Join(Environment.NewLine, GetCommands(withHeaderPrompt));
        }
    }
}
