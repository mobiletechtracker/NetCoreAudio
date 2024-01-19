using System;
using System.Threading.Tasks;

namespace NetCoreAudio.Interfaces
{
    public interface IRecorder
    {
        event EventHandler RecordingFinished;

        bool Recording { get; }

        Task Record(string fileName);
        Task Stop();
    }
}
