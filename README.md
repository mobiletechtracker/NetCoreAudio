# NetCoreAudio

The library allows playback of audio files on .NET Core on any operating system with minimal dependencies

## Usage

The library exposes Player class, which detects the OS the library is running on and abstracts away any OS-specific audio playback implementations.

### MP3 Playback on linux

The library relies on the mpg123 audio player for playback of .mp3 files on linux.

## Properties

### bool Playing

Indicates that the audio is currently playing.

### bool Paused

Indicates that the audio playback is currently paused.

## Methods

### Play(string fileName)

Will stop any current playback and will start playing the specified audio file. The fileName parameter can be an absolute path or a path relative to the directory where the library is located. Sets Playing flag to true. Sets Paused flag to false.

### Pause()

Pauses any ongoing playback. Sets Paused flag to true. Doesn't modify Playing flag.

### Resume()

Resumes any paused playback. Sets Paused flag to false. Doesn't modify Playing flag.

### Stop()

Stops any current playback and clears the buffer. Sets Playing and Paused flags to false.

Stopped playback cannot be resumed. If the same file needs to be played again, it can only be played from the beginning.

### SetVolume()

Sets the playing volume as percent

## Events

### EventHandler PlaybackFinished

Internally, sets Playing flag to false. Additional handlers can be attached to it to handle any custom logic.
