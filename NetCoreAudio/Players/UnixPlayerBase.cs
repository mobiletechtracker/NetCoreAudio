using NetCoreAudio.Interfaces;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NetCoreAudio.Players
{
	internal class UnixPlayerBase : IPlayer
	{
		private Process _process = null;

		internal const string PauseProcessCommand = "killall -STOP {0}";
		internal const string ResumeProcessCommand = "killall -CONT {0}";

		protected virtual string BashToolName { get; }

		public event EventHandler PlaybackFinished;

		public bool Playing { get; private set; }

		public bool Paused { get; private set; }

		public async Task Play(string fileName)
		{
			await Stop();
			_process = StartBashProcess($"{BashToolName} '{fileName}'");
			_process.Exited += HandlePlaybackFinished;
			_process.ErrorDataReceived += HandlePlaybackFinished;
			_process.Disposed += HandlePlaybackFinished;
			Playing = true;
		}

		public Task Pause()
		{
			if (Playing && !Paused)
			{
				var tempProcess = StartBashProcess(string.Format(PauseProcessCommand, BashToolName));
				tempProcess.WaitForExit();
				Paused = true;
			}

			return Task.CompletedTask;
		}

		public Task Resume()
		{
			if (Playing && Paused)
			{
				var tempProcess = StartBashProcess(string.Format(ResumeProcessCommand, BashToolName));
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

		protected Process StartBashProcess(string command)
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

		internal void HandlePlaybackFinished(object sender, EventArgs e)
		{
			if (Playing)
			{
				Playing = false;
				PlaybackFinished?.Invoke(this, e);
			}
		}
	}
}
