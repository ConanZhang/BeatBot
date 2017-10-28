using UnityEngine;

namespace uAudio
{
    public class uAudioPlayer_UI : MonoBehaviour
    {
        #region vars
        public uAudio.uAudioPlayer my_uAudioPlayer;
        public UnityEngine.UI.InputField songFilePath;
        public UnityEngine.UI.Slider songTime;
        public UnityEngine.UI.Text songCurrentTime;
        public UnityEngine.UI.Text songMaxTime;
        public UnityEngine.UI.Button bn_play;
        public UnityEngine.UI.Button bn_pause;
        public UnityEngine.UI.Button bn_stop;
        public UnityEngine.UI.Slider songVolume;
        public UnityEngine.UI.Toggle DontResetTime;
        public string targetFile;
        float myLastSetTime;
        #endregion ---vars---

        #region con
        void Start()
        {
            my_uAudioPlayer.Update_UI_songTime = Update_UI_songTime;

            if (songFilePath != null)
            {
                targetFile = songFilePath.text;
                onSongPath_Change(targetFile);
                //my_uAudioPlayer2.LoadFile(songFilePath.text);
                //#if !UNITY_2_6 && !UNITY_2_6_1 && !UNITY_3_0 && !UNITY_3_0_0 && !UNITY_3_1 && !UNITY_3_2 && !UNITY_3_3 && !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_0_1 && !UNITY_4_1 && !UNITY_4_2 && !UNITY_4_3 && !UNITY_4_5 && !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2
                //                songFilePath.onValueChanged.AddListener(onSongPath_Change);
                //#else
                //            songFilePath.onValueChange.AddListener(onSongPath_Change);
                //#endif
            }
            else
            {
                if (targetFile != string.Empty)
                    my_uAudioPlayer.SetFile(targetFile);
                else
                    UnityEngine.Debug.LogException(new System.Exception("uAudio_MP3_UI - targetFile #87gfs876vsvdvs"));
            }

            if (songTime != null)
                songTime.maxValue = Mathf.FloorToInt((float)my_uAudioPlayer.UAudio.TotalTime.TotalSeconds);

            if (songMaxTime != null)
                songMaxTime.text = System.TimeSpan.FromSeconds(Mathf.FloorToInt((float)my_uAudioPlayer.UAudio.TotalTime.TotalSeconds)).ToString();

            if (songVolume != null)
                songVolume.onValueChanged.AddListener(onSongVolume_Change);

            if (songTime != null)
                songTime.onValueChanged.AddListener(onSongTime_Change);

            if (bn_play != null)
                bn_play.onClick.AddListener(Play);

            if (bn_pause != null)
                bn_pause.onClick.AddListener(my_uAudioPlayer.Pause);

            if (bn_stop != null)
                bn_stop.onClick.AddListener(my_uAudioPlayer.Stop);

            my_uAudioPlayer.sendPlaybackState += new System.Action<uAudio_backend.PlayBackState>(send_data);
        }
        public void send_data(uAudio_backend.PlayBackState songState)
        {
            // connect here to get when the song is changes state
            //songState == uAudio_backend.PlayBackState.Playing
            //songState == uAudio_backend.PlayBackState.Stopped
            //songState == uAudio_backend.PlayBackState.Paused
            // UnityEngine.Debug.LogWarning("sendPlaybackState: " + v.ToString() + "---- 7ts87dt");
        }

        #endregion ---con---
        public void Play()
        {
            if (my_uAudioPlayer.PlaybackState != uAudio_backend.PlayBackState.Playing)
            {
                if (songFilePath != null && targetFile != songFilePath.text)
                {
                    targetFile = songFilePath.text;
                    my_uAudioPlayer.LoadFile(targetFile);
                }
                if (DontResetTime != null && DontResetTime.isOn)
                {
                    my_uAudioPlayer.Play(System.TimeSpan.FromSeconds(songTime.value));
                }
                else
                    my_uAudioPlayer.Play();

                if (songTime != null)
                    songTime.maxValue = Mathf.FloorToInt((float)my_uAudioPlayer.TotalTime.TotalSeconds);
                if (songMaxTime != null)
                    songMaxTime.text = my_uAudioPlayer.TotalTime.ToString().Substring(0, 8);

            }
        }

        public void Stop()
        {
            my_uAudioPlayer.Stop();
        }

        #region UI
        public void onSongVolume_Change(float volumeIN)
        {
            my_uAudioPlayer.Volume = volumeIN;
        }

        public void onSongTime_Change(float timeIN)
        {
            if (myLastSetTime != timeIN)
            {
                my_uAudioPlayer.ChangeCurrentTime(System.TimeSpan.FromSeconds(timeIN));
            }
        }

        void onSongPath_Change(string PathIN)
        {
            my_uAudioPlayer.SetFile(PathIN);

        }

        public void setSongPath_Change(string PathIN)
        {
            songFilePath.text = PathIN;
        }

        public void Update_UI_songTime(float newTimeIN)
        {
            try
            {
                myLastSetTime = newTimeIN;

                if (songTime != null)
                    songTime.value = newTimeIN;
                if (songCurrentTime != null)
                    songCurrentTime.text = System.TimeSpan.FromSeconds(newTimeIN).ToString();
            }
            catch
            { Debug.LogError("Update_UI_songTime - fail #gvc878svdsvd"); }
        }
        #endregion ---UI---
    }
}