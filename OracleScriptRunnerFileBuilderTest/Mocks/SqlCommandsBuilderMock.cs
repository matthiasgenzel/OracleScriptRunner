using OracleScriptRunner;
using System;
using System.Collections.Generic;
using System.IO;

namespace OracleScriptRunnerFileBuilderTest.Mocks
{
    internal class SqlCommandsBuilderMock : ISqlCommandsBuilder
    {
        public IEnumerable<ISqlScript> Files { get; set; }

        public IList<string> GetCommands(bool withHeaderPrompt)
        {
            var commands = new List<string>();
            foreach (var f in Files)
            {
                if (withHeaderPrompt)
                    commands.Add("prompt executing " + Path.GetFileName(f.PromptName));

                commands.Add("@" + Path.GetFileName(f.FilePath));
            }

            return commands;
        }

        public string GetCommandsAsFileText(bool withHeaderPrompt)
        {
            return string.Join(Environment.NewLine, GetCommands(withHeaderPrompt));
        }
    }
}