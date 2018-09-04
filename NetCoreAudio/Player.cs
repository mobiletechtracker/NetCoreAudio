using NetCoreAudio.Interfaces;
using NetCoreAudio.Players;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace NetCoreAudio
{
    public class Player : IPlayer
    {
        private readonly IPlayer _internalPlayer;

        public event EventHandler PlaybackFinished;

        public bool Playing => _internalPlayer.Playing;

		public bool Paused => _internalPlayer.Paused;

		public Player()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                _internalPlayer = new WindowsPlayer();
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                _internalPlayer = new LinuxPlayer();
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                _internalPlayer = new MacPlayer();
            else
                throw new Exception("No implementation exist for the current OS");

            _internalPlayer.PlaybackFinished += OnPlaybackFinished;
        }

        public async Task Play(string fileName)
        {
            await _internalPlayer.Play(fileName);
        }

        public async Task Pause()
        {
            await _internalPlayer.Pause();
        }

        public async Task Resume()
        {
            await _internalPlayer.Resume();
        }

        public async Task Stop()
        {
            await _internalPlayer.Stop();
        }

        private void OnPlaybackFinished(object sender, EventArgs e)
        {
            PlaybackFinished?.Invoke(this, e);
        }
    }
}
