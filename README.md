# NetCoreAudio

The library allows playback of audio files on .NET on any supported operating system (Windows, macOS, Linux) and any CPU architecture (AMD, ARM, etc.) with minimal dependencies.

## Prerequisites

The library can be downloaded and installed like any other NuGet package. On Windows, there are no further prerequisites, as it depends entirely on OS components for its functionality. On other operating systems, however, additional utilities may have to be installed before the library can fully function.

### Mac dependencies

On Mac, the library depends on [afplay](https://ss64.com/mac/afplay.html) for its audio playback functionality, which should already be included in the operating system.

However, for audio recording functionality, we need to install the [ffmpeg](https://ffmpeg.org/) tool.

### Linux dependencies

Linux implementation depends on [ALSA](https://www.alsa-project.org/wiki/Main_Page) architecture. Therefore, the machine needs to have the following utilities installed, which may or may not be included in the distro:

* [aplay](https://linux.die.net/man/1/aplay) for audio playback (excluding MP3 files)
* [amixer](https://linux.die.net/man/1/amixer) for setting the volume
* [arecord](https://linux.die.net/man/1/arecord) for recording audio

In addition to this, we need [mpg123](https://www.mpg123.de/) to be able to play MP3 files on Linux.

## Usage

The library can play and record audio. It will automatically detect which OS it's running on and will activate the components specific to this OS.

Below is the description of its public API:

### Audio Playback Functionality

For audio playback, the library exposes the `Player` class for audio playback, which detects the OS the library is running on and abstracts away any OS-specific audio playback implementations.

The class contains the following properties:

* `bool Playing`: Indicates that the audio is currently playing.
* `bool Paused`: Indicates that the audio playback is currently paused.

It also has the following methods:

* `Task Play(string fileName)`: Will stop any current playback and will start playing the specified audio file. The fileName parameter can be an absolute path or a path relative to the directory where the library is located. Sets the `Playing` flag to true. Sets the `Paused` flag to false.
* `Task Pause()`: Pauses any ongoing playback. Sets Paused flag to true. Doesn't modify the `Playing` flag.
* `Task Resume()`: Resumes any paused playback. Sets the `Paused` flag to false. Doesn't modify the `Playing` flag.
* `Task Stop()`: Stops any current playback and clears the buffer. Sets the `Playing` and `Paused` flags to false. Stopped playback cannot be resumed. If the same file needs to be played again, it can only be played from the beginning.
* `Task SetVolume()`: Sets the playing volume as percentage.

The class also has the following event handler:

* `EventHandler PlaybackFinished`: Internally, sets the `Playing` flag to false. Additional handlers can be attached to it to handle any custom logic.

### Audio Recording Functionality

For audio recording, we use the `Recorder` class. The class contains the following properties:

* `bool Recording`: Indicates that the audio is currently being recorded.

* `Task Record(string fileName)`: Begins the recording into the specified audio file.
* `Task Stop()`: Stops the recording ans saves the data into the file specified previously.