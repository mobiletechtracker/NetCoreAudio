using NetCoreAudio.Interfaces;
using NetCoreAudio.Utils;
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
        public static extern int waveOutSetVolume(
            IntPtr hwo,
            uint dwVolume);

        private Timer _playbackTimer;
        private Stopwatch _playStopwatch;

        private string _fileName;

        public event EventHandler PlaybackFinished;

        public bool Playing { get; private set; }
        public bool Paused { get; private set; }

        public async Task Play(string fileName)
        {
            FileUtil.ClearTempFiles();
            _fileName = $"\"{FileUtil.CheckFileToPlay(fileName)}\"";
            _playbackTimer = new Timer
            {
                AutoReset = false
            };
            _playStopwatch = new Stopwatch();
            await WindowsUtil.ExecuteMciCommand("Close All");
            await WindowsUtil.ExecuteMciCommand($"Status {_fileName} Length", _playbackTimer);
            await WindowsUtil.ExecuteMciCommand($"Play {_fileName}");
            Paused = false;
            Playing = true;
            _playbackTimer.Elapsed += HandlePlaybackFinished;
            _playbackTimer.Start();
            _playStopwatch.Start();
        }

        public async Task Pause()
        {
            if (Playing && !Paused)
            {
                await WindowsUtil.ExecuteMciCommand($"Pause {_fileName}");
                Paused = true;
                _playbackTimer.Stop();
                _playStopwatch.Stop();
                _playbackTimer.Interval -= _playStopwatch.ElapsedMilliseconds;
            }
        }

        public async Task Resume()
        {
            if (Playing && Paused)
            {
                await WindowsUtil.ExecuteMciCommand($"Resume {_fileName}");
                Paused = false;
                _playbackTimer.Start();
                _playStopwatch.Reset();
                _playStopwatch.Start();
            }
        }

        public async Task Stop()
        {
            if (Playing)
            {
                await WindowsUtil.ExecuteMciCommand($"Stop {_fileName}");
                Playing = false;
                Paused = false;
                _playbackTimer.Stop();
                _playStopwatch.Stop();
                FileUtil.ClearTempFiles();
            }
        }

        private void HandlePlaybackFinished(object sender, ElapsedEventArgs e)
        {
            Playing = false;
            PlaybackFinished?.Invoke(this, e);
            _playbackTimer.Dispose();
            _playbackTimer = null;
        }

        public Task SetVolume(byte percent)
        {
            // Calculate the volume that's being set
            int newVolume = ushort.MaxValue / 100 * percent;
            // Set the same volume for both the left and the right channels
            uint newVolumeAllChannels =
                ((uint)newVolume & 0x0000ffff) | ((uint)newVolume << 16);
            // Set the volume
            waveOutSetVolume(IntPtr.Zero, newVolumeAllChannels);

            return Task.CompletedTask;
        }
    }
}
