using NetCoreAudio;

var player = new Player();
player.PlaybackFinished += OnPlaybackFinished;

Console.WriteLine("Welcome to the demo of NetCoreAudio package");
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
                player.Play(fileName).Wait();
                Console.WriteLine(player.Playing ? "Playback started" : "Could not start the playback");
                break;
            case "pause":
                player.Pause().Wait();
                Console.WriteLine(player.Paused ? "Playback paused" : "Could not pause playback");
                break;
            case "resume":
                player.Resume().Wait();
                Console.WriteLine(player.Playing && !player.Paused ? "Playback resumed" : "Could not resume playback");
                break;
            case "stop":
                player.Stop().Wait();
                Console.WriteLine(!player.Playing ? "Playback stopped" : "Could not stop the playback");
                break;
            case "change":
                player.Stop().Wait();
                ShowFileEntryPrompt();
                fileName = Console.ReadLine();
                ShowInstruction();
                break;
            case "volume":
                Console.WriteLine("Enter new volume in percent");
                byte volume = Convert.ToByte(Console.ReadLine());
                player.SetVolume(volume).Wait();
                ShowInstruction();
                break;
            case "exit":
                break;
            default:
                Console.WriteLine("Haven't got a clue, mate!");
                break;
        }

        if (command == "exit") break;
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
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