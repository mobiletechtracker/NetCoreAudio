using NetCoreAudio.Interfaces;
using System.IO;

namespace NetCoreAudio.Players
{
    internal class LinuxPlayer : UnixPlayerBase, IPlayer
    {
        protected override string GetBashCommand(string fileName)
        {
            if (".mp3".Equals(Path.GetExtension(fileName)))
            {
                return "mpg123 -q";
            }
            else
            {
                return "aplay -q";
            }
        }
    }
}
