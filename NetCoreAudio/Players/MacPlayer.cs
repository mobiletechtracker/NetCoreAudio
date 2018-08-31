using NetCoreAudio.Interfaces;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NetCoreAudio.Players
{
    internal class MacPlayer : IPlayer
    {
        private Process _process = null;

        private const string KillProcessCommand = "ps - A | grep - m1 $1 | awk '{print $1}' | kill {0}";

        public async Task Play(string fileName)
        {
            await Stop();
            _process = StartBashProcess($"afplay {fileName}");
        }

        public Task Pause()
        {
            try
            {
                var tempProcess = StartBashProcess(string.Format(KillProcessCommand, "-17"));
                tempProcess.WaitForExit();
            }
            catch
            { }

            return Task.CompletedTask;
        }

        public Task Resume()
        {
            try
            {
                var tempProcess = StartBashProcess(string.Format(KillProcessCommand, "-19"));
                tempProcess.WaitForExit();
            }
            catch
            { }

            return Task.CompletedTask;
        }

        public Task Stop()
        {
            if (_process != null)
            {
                _process.Dispose();
                _process = null;
            }
            return Task.CompletedTask;
        }

        private Process StartBashProcess(string command)
        {
            var escapedArgs = command.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            return process;
        }
    }
}
