using NetCoreAudio;

namespace DemoHarness
{
    internal static class PlayerHarness
    {
        public static async Task InteractWithPlayer()
        {
            var player = new Player();
            player.PlaybackFinished += OnPlaybackFinished;

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
                        case "play":
                            Console.WriteLine($"Playing {fileName}");
                            await player.Play(fileName);
                            Console.WriteLine(player.Playing ? "Playback started" : "Could not start the playback");
                            break;
                        case "pause":
                            await player.Pause();
                            Console.WriteLine(player.Paused ? "Playback paused" : "Could not pause playback");
                            break;
                        case "resume":
                            await player.Resume();
                            Console.WriteLine(player.Playing && !player.Paused ? "Playback resumed" : "Could not resume playback");
                            break;
                        case "stop":
                            await player.Stop();
                            Console.WriteLine(!player.Playing ? "Playback stopped" : "Could not stop the playback");
                            break;
                        case "change":
                            await player.Stop();
                            ShowFileEntryPrompt();
                            fileName = Console.ReadLine();
                            ShowInstruction();
                            break;
                        case "volume":
                            Console.WriteLine("Enter new volume in percent");
                            byte volume = Convert.ToByte(Console.ReadLine());
                            await player.SetVolume(volume);
                            ShowInstruction();
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
            Console.WriteLine("Please enter the full path to the file you would like to play:");
        }

        static void ShowInstruction()
        {
            Console.WriteLine("You can manipulate the player with the following commands:");
            Console.WriteLine("play - Play the specified file from the start");
            Console.WriteLine("pause - Pause the playback");
            Console.WriteLine("resume - Resume the playback");
            Console.WriteLine("stop - Stop the playback");
            Console.WriteLine("change - Change the file name");
            Console.WriteLine("volume - Set the volume");
            Console.WriteLine("exit - Exit the app");
        }

        static void OnPlaybackFinished(object sender, EventArgs e)
        {
            Console.WriteLine("Playback finished");
        }
    }
}
