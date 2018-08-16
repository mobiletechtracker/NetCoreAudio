using NetCoreAudio.Interfaces;
using NetCoreAudio.Players;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace NetCoreAudio
{
    public class Player : IPlayer
    {
        private readonly IPlayer internalPlayer;

        public Player()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                internalPlayer = new WindowsPlayer();
            else
                throw new Exception("Currently, no implementation exist for other platforms"); // Only while they haven't been added
        }

        public async Task Play(string fileName)
        {
            await internalPlayer.Play(fileName);
        }

        public async Task Pause()
        {
            await internalPlayer.Pause();
        }

        public async Task Resume()
        {
            await internalPlayer.Resume();
        }

        public async Task Stop()
        {
            await internalPlayer.Stop();
        }
    }
}
