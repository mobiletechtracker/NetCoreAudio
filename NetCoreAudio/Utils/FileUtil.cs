using System.IO;

namespace NetCoreAudio.Utils
{
    internal static class FileUtil
    {
        private const string TempDirName = "temp";

        public static string CheckFileToPlay(string originalFileName)
        {
            var fileNameToReturn = originalFileName;
            if (originalFileName.Contains(" "))
            {
                Directory.CreateDirectory(TempDirName);
                fileNameToReturn = TempDirName + Path.DirectorySeparatorChar + 
                    Path.GetFileName(originalFileName).Replace(" ", "");
                File.Copy(originalFileName, fileNameToReturn);
            }

            return fileNameToReturn;
        }

        public static void ClearTempFiles()
        {
            if (Directory.Exists(TempDirName))
                Directory.Delete(TempDirName, true);
        }
    }
}
