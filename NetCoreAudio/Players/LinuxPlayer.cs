using NetCoreAudio.Interfaces;

namespace NetCoreAudio.Players
{
    internal class LinuxPlayer : UnixPlayerBase, IPlayer
    {
        protected override string BashToolName
        {
            get
            {
                return "aplay";
            }
        }
    }
}
