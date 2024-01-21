using DemoHarness;

Console.WriteLine("Welcome to the demo of NetCoreAudio package");
Console.WriteLine("Type 'player' if you want to test the audio playback functionality");
Console.WriteLine("Type 'recorder' if you want to test the audio recording functionality");
Console.WriteLine("Type anything else to exit");

var command = Console.ReadLine();

switch(command)
{
    case "player":
        await PlayerHarness.InteractWithPlayer();
        break;
    case "recorder":
        await RecorderHarness.InteractWithRecorder();
        break;
    default:
        break;
}