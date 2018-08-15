using System.Threading.Tasks;

namespace NetCoreAudio.Interfaces
{
    public interface IPlayer
    {
        Task Play(string fileName);
        Task Pause();
        Task Resume();
        Task Stop();
    }
}
