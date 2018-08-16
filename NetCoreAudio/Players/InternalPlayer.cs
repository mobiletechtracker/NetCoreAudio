using NetCoreAudio.Interfaces;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreAudio.Players
{
    internal class WindowsPlayer : IPlayer
    {
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string command, StringBuilder stringReturn, int returnLength, IntPtr hwndCallback);

        public Task Play(string fileName)
        {
            ExecuteMsiCommand("Close All");
            ExecuteMsiCommand($"Open {fileName} Type MPEGVideo Alias myDevice");
            ExecuteMsiCommand("Play myDevice");

            return Task.CompletedTask;
        }

        public Task Pause()
        {
            ExecuteMsiCommand("Pause myDevice");
            return Task.CompletedTask;
        }

        public Task Resume()
        {
            ExecuteMsiCommand("Resume myDevice");
            return Task.CompletedTask;
        }

        public Task Stop()
        {
            ExecuteMsiCommand("Close myDevice");
            return Task.CompletedTask;
        }

        private Task ExecuteMsiCommand(string commandString)
        {
            var result = mciSendString(commandString, null, 0, IntPtr.Zero);

            if (result != 0)
            {
                throw new Exception($"Error executing MSI command. Error code: {result}");
            }

            return Task.CompletedTask;
        }
    }
}
