using NetCoreAudio.Interfaces;
using NetCoreAudio.Utils;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NetCoreAudio.Recorders
{
    internal abstract class UnixRecorderBase : IRecorder
    {
        private Process _process = null;

        public event EventHandler RecordingFinished;

        public bool Recording { get; set; }

        protected abstract string GetBashCommand(string fileName);

        public async Task Record(string filePath)
        {
            await Stop();
            var BashToolName = GetBashCommand(filePath);
            _process = BashUtil.StartBashProcess(
                $"{BashToolName} '{filePath}'");
            _process.EnableRaisingEvents = true;
            _process.Exited += HandleRecordingFinished;
            _process.ErrorDataReceived += HandleRecordingFinished;
            _process.Disposed += HandleRecordingFinished;
            Recording = true;
        }

        public Task Stop()
        {
            if (_process != null)
            {
                _process.Kill();
                _process.Dispose();
                _process = null;
            }

            Recording = false;

            return Task.CompletedTask;
        }

        internal void HandleRecordingFinished(object sender, EventArgs e)
        {
            if (Recording)
            {
                Recording = false;
                RecordingFinished?.Invoke(this, e);
            }
        }
    }
}
