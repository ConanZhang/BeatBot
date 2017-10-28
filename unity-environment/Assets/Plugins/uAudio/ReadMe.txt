Scenes:
Assets\Plugins\uAudio\uAudio.unity - is the Main Scene with each audio types.
Assets\Plugins\uAudio\Basic.unity - is a basic usage of uAudio with just one object.
Assets\Plugins\uAudio\MP3 Player.unity - is just a MP3 Player alone just one object and GUI.

Setup MP3 to play:
Copy the demo song from "Assets\Plugins\uAudio\Demo Music\" to a location on your local drive. (recomendation "C:\Music\")
Then in your scene put the path into uAudioEasy.musicFile. (recomendation "C:\Music\Nameless Warning - Things in Life.mp3")

Simple Setup:
Add the script Assets\Plugins\uAudio\Scripts\uAudioPlayer.cs to an object.
Add AudioSource to same object
	-Then Link AudioSource to -> My Audio Source in uAudioPlayer
	-Then set the value "Target File", this can be done in the editor or from code.
	-Then call Play();
	
Setup:
Add the script Assets\Plugins\uAudio\Scripts\uAudioPlayer_UI.cs to an object.
	-Link my_uAudioPlayer to -> uAudioPlayer
	-Then link uGUI items on uAudioPlayer
		UnityEngine.UI.Slider songTime; - slider get's and set's the song time index
		UnityEngine.UI.Slider songVolume; - slider get's and set's the song volume index
		UnityEngine.UI.Text songCurrentTime; - this text updated with the current song time
		UnityEngine.UI.Text songMaxTime; - this is text updated to the songs max time
		UnityEngine.UI.InputField songFilePath; - this is read in as the path to the song file
		UnityEngine.UI.Button bn_play - this plays the song
		UnityEngine.UI.Button bn_pause - this pauses the song
		UnityEngine.UI.Button bn_stop - this stops the song

No need to have an Audio Listener in the scene, but this will not conflict(in anyway) with an Audio Listener if you do have one.
uAudio calls directly into the windows audio library to play the audio.

If you have audio cutting out, and you have a number of audio sources, try to change the priority of the song audio source.

This Asset is from Wave3D
www.Wave3D.com/uAudio/uAudio.html
email us at uAudio_support@wave3d.com
Thank you for using uAudio!