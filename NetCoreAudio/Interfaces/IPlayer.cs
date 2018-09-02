using System.Threading.Tasks;

namespace NetCoreAudio.Interfaces
{
    public interface IPlayer
    {
		bool Playing { get; }
		bool Paused { get; }

		Task Play(string fileName);
        Task Pause();
        Task Resume();
        Task Stop();
    }
}
