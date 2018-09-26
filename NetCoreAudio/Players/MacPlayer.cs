using NetCoreAudio.Interfaces;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NetCoreAudio.Players
{
    internal class MacPlayer : IPlayer
    {
        private Process _process = null;

        private const string KillProcessCommand = "ps -A | grep -m1 'afplay' | awk '{print $1}' | kill {0}";

        public event EventHandler PlaybackFinished;

        public bool Playing { get; private set; }

		public bool Paused { get; private set; }

		public async Task Play(string fileName)
        {
            await Stop();
            _process = StartBashProcess($"afplay '{fileName}'");
			_process.Exited += HandlePlaybackFinished;
			Playing = true;
		}

        public Task Pause()
        {
			if (Playing && !Paused)
			{
				var tempProcess = StartBashProcess(string.Format(KillProcessCommand, "-17"));
				tempProcess.WaitForExit();
				Paused = true;
			}

            return Task.CompletedTask;
        }

        public Task Resume()
        {
			if (Playing && Paused)
			{
				var tempProcess = StartBashProcess(string.Format(KillProcessCommand, "-19"));
				tempProcess.WaitForExit();
				Paused = false;
			}

            return Task.CompletedTask;
        }

        public Task Stop()
        {
			if (_process != null)
			{
				_process.Kill();
				_process.Dispose();
				_process = null;
			}

			Playing = false;
			Paused = false;

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

		private void HandlePlaybackFinished(object sender, EventArgs e)
		{
			Playing = false;
            PlaybackFinished?.Invoke(this, e);
        }
	}
}
