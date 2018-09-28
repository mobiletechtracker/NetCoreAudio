using NetCoreAudio.Interfaces;

namespace NetCoreAudio.Players
{
    internal class MacPlayer : UnixPlayerBase, IPlayer
	{
		protected override string BashToolName
		{
			get
			{
				return "afplay";
			}
		}
	}
}
