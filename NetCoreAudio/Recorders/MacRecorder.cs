using System.IO;

namespace NetCoreAudio.Recorders
{
    internal class MacRecorder : UnixRecorderBase
    {
        protected override string GetBashCommand(string fileName)
        {
            string command = $"arecord -vv --format=cd ";

            if (Path.GetExtension(fileName).ToLower().Equals(".mp3"))
            {
                command += "--file-type raw | lame -r - ";
            }

            return command += fileName;
        }
    }
}
