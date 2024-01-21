using NetCoreAudio.Interfaces;
using NetCoreAudio.Utils;
using System;
using System.Threading.Tasks;

namespace NetCoreAudio.Recorders
{
    internal class WindowsRecorder : IRecorder
    {
        private string _fileName;

        public bool Recording { get; set; }

        public event EventHandler RecordingFinished;

        public async Task Record(string fileName)
        {
            try
            {
                if (Recording)
                {
                    await Stop();
                }

                _fileName = fileName;

                Recording = true;
                await WindowsUtil.ExecuteMciCommand("open new Type waveaudio Alias recsound");
                await WindowsUtil.ExecuteMciCommand("record recsound");
            }
            catch
            {
                HandleRecordingFinished();
                throw;
            }
        }

        public async Task Stop()
        {
            if (!Recording)
                return;

            try
            {
                await WindowsUtil.ExecuteMciCommand($"save recsound {_fileName}");
                await WindowsUtil.ExecuteMciCommand("close recsound ");
                
            }
            finally
            {
                HandleRecordingFinished();
            }
        }

        internal void HandleRecordingFinished()
        {
            if (Recording)
            {
                Recording = false;
                RecordingFinished?.Invoke(this, new EventArgs());
            }
        }

    }
}
