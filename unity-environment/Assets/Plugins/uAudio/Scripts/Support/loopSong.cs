using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loopSong : MonoBehaviour {
    public uAudio.uAudioPlayer_UI my_uAudioPlayer_UI;
    public uAudio.dirLoop_UI my_dirLoop_UI;
    public uAudio.uAudioPlayer my_uAudioPlayer;
   
    void Start()
    {
        my_uAudioPlayer.sendPlaybackState += new System.Action<uAudio.uAudio_backend.PlayBackState>(songDone);
    }

	void songDone(uAudio.uAudio_backend.PlayBackState valIN)
    {
        if (valIN == uAudio.uAudio_backend.PlayBackState.Stopped)
        {
            var i = my_uAudioPlayer.CurrentTime;
            if (i == my_uAudioPlayer.TotalTime)
            {
                my_dirLoop_UI.MoveNextSong();
            }
        }
    }
}
