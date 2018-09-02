using NetCoreAudio.Interfaces;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NetCoreAudio.Players
{
    internal class LinuxPlayer : IPlayer
    {
        private Process _process = null;

		public bool Playing => throw new System.NotImplementedException();

		public bool Paused => throw new System.NotImplementedException();

		public async Task Play(string fileName)
        {
            await Stop();
            _process = StartAplayPlayback(fileName);
        }

        public async Task Pause()
        {
            await PauseOrResume();
        }

        public async Task Resume()
        {
            await PauseOrResume();
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

        private Process StartAplayPlayback(string fileName)
        {
            var escapedArgs = fileName.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"aplay {escapedArgs} -i\"",
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            return process;
        }

        private Task PauseOrResume()
        {
            if (_process != null)
            {
                // Space character pauses/resumes aplay running in interactive mode
                _process.StandardInput.Write(' ');
            }

            return Task.CompletedTask;
        }
    }
}
