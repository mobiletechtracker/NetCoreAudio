using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NetCoreAudio;

namespace AudioPlayer;

internal partial class MainModel : ObservableObject
{
    private readonly Player _player;

    public MainModel()
    {
        _player = new();
        //_player.PlaybackFinished += OnPlaybackFinished;
    }

    [ObservableProperty]
    private string _fileName = string.Empty;

    [ObservableProperty]
    private bool _playing = false;

    [ObservableProperty]
    private bool _paused = false;

    [RelayCommand]
    private async Task Play()
    {
        await _player.Play(FileName);
        UpdateStatuses();
    }

    [RelayCommand]
    private async Task Pause()
    {
        await _player.Pause();
        UpdateStatuses();
    }

    [RelayCommand]
    private async Task Resume()
    {
        await _player.Resume();
        UpdateStatuses();
    }

    [RelayCommand]
    private async Task Stop()
    {
        await _player.Stop();
        UpdateStatuses();
    }

    private void UpdateStatuses()
    {
        Playing = _player.Playing;
        Paused = _player.Paused;
    }
}
