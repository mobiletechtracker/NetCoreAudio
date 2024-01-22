using System.Runtime.InteropServices;
using System.Text;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace NetCoreAudio.Utils
{
    internal static class WindowsUtil
    {
        [DllImport("winmm.dll")]
        private static extern int mciSendString(
            string command,
            StringBuilder stringReturn,
            int returnLength,
            IntPtr hwndCallback);

        [DllImport("winmm.dll")]
        private static extern int mciGetErrorString(
            int errorCode,
            StringBuilder errorText,
            int errorTextSize);

        [DllImport("winmm.dll")]
        public static extern int waveOutSetVolume(
            IntPtr hwo,
            uint dwVolume);

        [DllImport("winmm.dll")]
        public static extern int waveInOpen(
            ref IntPtr lphWaveIn,
            uint DEVICEID,
            ref WaveFormat lpWaveFormat,
            IntPtr dwCallback,
            uint dwInstance,
            uint dwFlags);

        public static Task ExecuteMciCommand(
            string commandString, Timer playbackTimer = null)
        {
            var sb = new StringBuilder();

            var result = mciSendString(commandString, sb, 1024 * 1024, IntPtr.Zero);

            if (result != 0)
            {
                var errorSb = new StringBuilder(
                    $"Error executing MCI command '{commandString}'. Error code: {result}.");
                var sb2 = new StringBuilder(128);

                mciGetErrorString(result, sb2, 128);
                errorSb.Append($" Message: {sb2}");

                throw new Exception(errorSb.ToString());
            }

            if (playbackTimer != null && 
                int.TryParse(sb.ToString(), out var length))
                playbackTimer.Interval = length;

            return Task.CompletedTask;
        }

        public static Task SetVolume(byte percent)
        {
            // Calculate the volume that's being set
            int newVolume = ushort.MaxValue / 100 * percent;
            // Set the same volume for both the left and the right channels
            uint newVolumeAllChannels =
                ((uint)newVolume & 0x0000ffff) | ((uint)newVolume << 16);
            // Set the volume
            waveOutSetVolume(IntPtr.Zero, newVolumeAllChannels);

            return Task.CompletedTask;
        }
    }

    public struct WaveFormat
    {
        public short wFormatTag;
        public short nChannels;
        public uint nSamplesPerSec;
        public uint nAvgBytesPerSec;
        public short nBlockAlign;
        public short wBitsPerSample;
        public short cbSize;
    }
}
