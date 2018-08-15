using NetCoreAudio.Interfaces;
using NetCoreAudio.Players;
using System.Threading.Tasks;

namespace NetCoreAudio
{
    public class Player : IPlayer
    {
        private readonly IPlayer internalPlayer;

        public Player()
        {
            internalPlayer = new WindowsPlayer();
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
