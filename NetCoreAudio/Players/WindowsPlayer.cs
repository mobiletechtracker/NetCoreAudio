using NetCoreAudio.Interfaces;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace NetCoreAudio.Players
{
    internal class WindowsPlayer : IPlayer
    {
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string command, StringBuilder stringReturn, int returnLength, IntPtr hwndCallback);

		private Timer _playbackTimer;
		private Stopwatch _playStopwatch;

        public event EventHandler PlaybackFinished;

        public bool Playing { get; private set; }
		public bool Paused { get; private set; }

		public Task Play(string fileName)
        {
            _playbackTimer = new Timer();
            _playbackTimer.AutoReset = false;
            _playStopwatch = new Stopwatch();
            ExecuteMsiCommand("Close All");
            ExecuteMsiCommand($"Open \"{fileName}\" Type MPEGVideo Alias myDevice");
			ExecuteMsiCommand("Status myDevice Length");
			ExecuteMsiCommand("Play myDevice");
			Paused = false;
			Playing = true;
            _playbackTimer.Elapsed += HandlePlaybackFinished;
            _playbackTimer.Start();
            _playStopwatch.Start();

			return Task.CompletedTask;
        }

        public Task Pause()
        {
			if (Playing && !Paused)
			{
				ExecuteMsiCommand("Pause myDevice");
				Paused = true;
				_playbackTimer.Stop();
				_playStopwatch.Stop();
				_playbackTimer.Interval -= _playStopwatch.ElapsedMilliseconds;
			}

			return Task.CompletedTask;
        }

        public Task Resume()
        {
			if (Playing && Paused)
			{
				ExecuteMsiCommand("Resume myDevice");
				Paused = false;
				_playbackTimer.Start();
                _playStopwatch.Reset();
                _playStopwatch.Start();
			}
            return Task.CompletedTask;
        }

        public Task Stop()
        {
			if (Playing)
			{
				ExecuteMsiCommand("Close myDevice");
				Playing = false;
				Paused = false;
				_playbackTimer.Stop();
				_playStopwatch.Stop();
			}
			return Task.CompletedTask;
        }

		private void HandlePlaybackFinished(object sender, ElapsedEventArgs e)
		{
			Playing = false;
            PlaybackFinished?.Invoke(this, e);
            _playbackTimer.Dispose();
            _playbackTimer = null;
        }

		private Task ExecuteMsiCommand(string commandString)
        {
			var sb = new StringBuilder();

            var result = mciSendString(commandString, sb, 1024 * 1024, IntPtr.Zero);

            if (result != 0)
            {
                throw new Exception($"Error executing MSI command. Error code: {result}");
            }

			if (commandString.ToLower().StartsWith("status") && int.TryParse(sb.ToString(), out var length))
				_playbackTimer.Interval = length;

			return Task.CompletedTask;
        }
    }
}
