using NetCoreAudio.Interfaces;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NetCoreAudio.Players
{
    internal class LinuxPlayer : IPlayer
    {
        private Process _process = null;

        public event EventHandler PlaybackFinished;

        public bool Playing { get; private set; }

		public bool Paused { get; private set; }

		public async Task Play(string fileName)
        {
            await Stop();
            _process = StartAplayPlayback(fileName);
			Playing = true;
        }

        public async Task Pause()
        {
			if (Playing && !Paused)
			{
				await PauseOrResume();
				Paused = true;
			}
        }

        public async Task Resume()
        {
			if (Playing && Paused)
			{
				await PauseOrResume();
				Paused = false;
			}
        }

        public Task Stop()
        {
            if (_process != null)
            {
                _process.Dispose();
                _process = null;
            }

			Playing = false;
			Paused = false;

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
			process.Exited += HandlePlaybackFinished;
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

		private void HandlePlaybackFinished(object sender, EventArgs e)
		{
			Playing = false;
            PlaybackFinished?.Invoke(this, e);
        }
	}
}
