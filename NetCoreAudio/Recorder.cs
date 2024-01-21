using NetCoreAudio.Interfaces;
using NetCoreAudio.Recorders;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace NetCoreAudio
{
    public class Recorder : IRecorder
    {
        private readonly IRecorder _internalRecorder;

        /// <summary>
        /// Internally, sets the Recording flag to false. Additional handlers can be attached to it to handle any custom logic.
        /// </summary>
        public event EventHandler RecordingFinished;

        /// <summary>
        /// Indicates that the audio is currently being recorded.
        /// </summary>
        public bool Recording => _internalRecorder.Recording;

        public Recorder()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                _internalRecorder = new WindowsRecorder();
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                _internalRecorder = new LinuxRecorder();
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                _internalRecorder = new MacRecorder();
            else
                throw new Exception("No implementation exist for the current OS");

            _internalRecorder.RecordingFinished += OnRecordingFinished;
        }

        /// <summary>
        /// Will stop any current recording and will start a new recording into a specified file. The fileName parameter can be an absolute path or a path relative to the directory where the library is located. Sets Recording flag to true.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task Record(string fileName)
        {
            await _internalRecorder.Record(fileName);
        }

        /// <summary>
        /// Stops any current playback and clears the buffer. Sets Playing and Paused flags to false.
        /// </summary>
        /// <returns></returns>
        public async Task Stop()
        {
            await _internalRecorder.Stop();
        }

        private void OnRecordingFinished(object sender, EventArgs e)
        {
            RecordingFinished?.Invoke(this, e);
        }
    }
}