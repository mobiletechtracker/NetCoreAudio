namespace NetCoreAudio.Recorders
{
    internal class LinuxRecorder : UnixRecorderBase
    {
        protected override string GetBashCommand(string fileName)
        {
            return $"ffmpeg -f avfoundation -i \":1\" {fileName}";
        }
    }
}
