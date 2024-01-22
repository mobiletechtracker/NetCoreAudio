using NetCoreAudio;

namespace DemoHarness
{
    internal static class RecorderHarness
    {
        public static async Task InteractWithRecorder()
        {
            var recorder = new Recorder();
            recorder.RecordingFinished += OnRecordingFinished;

            ShowFileEntryPrompt();
            var fileName = Console.ReadLine();
            ShowInstruction();

            while (true)
            {
                var command = Console.ReadLine();

                try
                {
                    switch (command)
                    {
                        case "record":
                            Console.WriteLine($"Recording into {fileName}");
                            await recorder.Record(fileName);
                            Console.WriteLine(recorder.Recording ? "Recording started" : "Could not start recording");
                            break;
                        case "stop":
                            await recorder.Stop();
                            Console.WriteLine(!recorder.Recording ? "Recording stopped" : "Could not stop the recording");
                            break;
                        case "exit":
                            break;
                        default:
                            Console.WriteLine("Unknown command!");
                            break;
                    }

                    if (command == "exit") break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static void ShowFileEntryPrompt()
        {
            Console.WriteLine("Please enter the full path to the file where you want to store your recording:");
        }

        static void ShowInstruction()
        {
            Console.WriteLine("You can manipulate the recorder with the following commands:");
            Console.WriteLine("record - Record audio into a file");
            Console.WriteLine("stop - Stop the recording");
            Console.WriteLine("exit - Exit the app");
        }

        static void OnRecordingFinished(object sender, EventArgs e)
        {
            Console.WriteLine("Recording finished");
        }
    }
}
