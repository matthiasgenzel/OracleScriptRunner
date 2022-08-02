using OracleScriptRunner;
using OracleScriptRunnerFileRunner;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OracleScriptRunnerWinCmdRunner
{
    public class WinCmdRunner : ITerminalRunner
    {
        /*
           runs programs on win cmd line and returns complete output
        */
        public string RunProgram(IEnumerable<ITerminalCommand> executionFilePaths)
        {
            Process cmd = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            };

            cmd.Start();

            foreach (var executionFile in executionFilePaths)
                cmd.StandardInput.WriteLine(executionFile.Text);

            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();

            return cmd.StandardOutput.ReadToEnd();
        }
    }
}
