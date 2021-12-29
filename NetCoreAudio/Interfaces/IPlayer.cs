using System;
using System.Threading.Tasks;

namespace NetCoreAudio.Interfaces
{
    public interface IPlayer
    {
        event EventHandler PlaybackFinished;

        bool Playing { get; }
        bool Paused { get; }

        Task Play(string fileName);
        Task Pause();
        Task Resume();
        Task Stop();
        Task SetVolume(byte percent);
    }
}
