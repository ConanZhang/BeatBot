using System;

namespace uAudio
{
    public class uAudioStreamer_UI : UnityEngine.MonoBehaviour, uAudio_backend.IAudioPlayer
    {
        #region vars
        public UnityEngine.UI.InputField urlInput;
        public UnityEngine.UI.Slider Buffer;
        public UnityEngine.UI.Button bn_play;
        public UnityEngine.UI.Button bn_pause;
        public UnityEngine.UI.Button bn_stop;
        public UnityEngine.UI.Slider SongVolume;
        public string targetFilePath;

        public uAudioStreamer my_uAudioStreamer;
        public bool force_buffer;

        UnityEngine.Coroutine bufferLoop;
        bool halt = false;
        System.Collections.IEnumerator updateBufferLoop;
        System.Collections.IEnumerator updateBuffer()
        {
            while (!halt)
            {
                try
                {
                    float v = (float)my_uAudioStreamer.BufferedTime;
                    Buffer.value = v;
                }
                catch
                {
#if uAudio_debug  
                UnityEngine.Debug.Log("Buffer #error");
#endif
                }
                yield return null;
            }
        }
        public Action<uAudio_backend.PlayBackState> sendPlaybackState
        {
            get
            {
                return my_uAudioStreamer.sendPlaybackState;
            }

            set
            {
                my_uAudioStreamer.sendPlaybackState = value;
            }
        }
        Action uAudio_backend.IAudioPlayer.SLEEP
        {
            get
            {
                UnityEngine.Debug.LogWarning("SLEEP #h9dsvd");
                throw new NotImplementedException();
            }

            set
            {
                UnityEngine.Debug.LogWarning("SLEEP #h9dsvd");
                throw new NotImplementedException();
            }
        }

        public string AudioTitle
        {
            get
            {
                return targetFilePath;
            }
        }

        string uAudio_backend.IAudioPlayer.current_TargetFile_Loaded
        {
            get
            { return targetFilePath; }
        }

        public uAudio_backend.PlayBackState PlaybackState
        {
            get
            {
                return my_uAudioStreamer.PlaybackState;
            }
        }

        public void ChangeCurrentVolume(UnityEngine.UI.Slider volumeSlider)
        {
            ChangeCurrentVolume(volumeSlider.value);
        }

        public void ChangeCurrentVolume(float volumeIN)
        {
            my_uAudioStreamer.Volume = volumeIN;
        }

        public float Volume
        {
            get
            {
                return my_uAudioStreamer.Volume;
            }
            set
            {
                my_uAudioStreamer.Volume = value;
            }
        }

        public TimeSpan TotalTime
        {
            get
            {
                return TimeSpan.Zero;
            }
        }
        public int SongLength { get { return 0; } }

        public TimeSpan CurrentTime
        {
            get
            {
                return TimeSpan.Zero;
            }

            set
            {
                //throw new NotImplementedException();
            }
        }

        public float Pan
        {
            get
            {
                return my_uAudioStreamer.Pan;
            }

            set
            {
                my_uAudioStreamer.Pan = value;
            }
        }

        //public Action PlayBackStopped
        //{
        //    get
        //    {
        //        return my_uAudioStreamer.PlayBackStopped;
        //        //throw new NotImplementedException();
        //    }

        //    set
        //    {
        //        my_uAudioStreamer.PlayBackStopped = value;
        //    }
        //}
        public bool ForceBuffering
        {
            get
            {
                return my_uAudioStreamer.ForceBuffering;
            }

            set
            {
                my_uAudioStreamer.ForceBuffering = value;
            }
        }
        #endregion ---vars---

        #region con
        void Start()
        {
            my_uAudioStreamer.ForceBuffering = force_buffer;

            if (Buffer != null)
            {
                Buffer.maxValue = my_uAudioStreamer.maxBufferTime;
            }

            if (SongVolume != null)
            {
                my_uAudioStreamer.Volume = SongVolume.value;
                SongVolume.onValueChanged.AddListener(onSongVolume_Change);
            }
            else
            {
                my_uAudioStreamer.Volume = 1;
            }
            if (urlInput != null)
            {
                targetFilePath = urlInput.text;
                //my_uAudioStreamer2.LoadFile(urlInput.text);
#if !UNITY_2_6 && !UNITY_2_6_1 && !UNITY_3_0 && !UNITY_3_0_0 && !UNITY_3_1 && !UNITY_3_2 && !UNITY_3_3 && !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_0_1 && !UNITY_4_1 && !UNITY_4_2 && !UNITY_4_3 && !UNITY_4_5 && !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2
                urlInput.onValueChanged.AddListener(onSongPath_Change);
#else
                urlInput.onValueChange.AddListener(onSongPath_Change);
#endif
            }

            if (bn_play != null)
                bn_play.onClick.AddListener(startPlay);

            if (bn_pause != null)
                bn_pause.onClick.AddListener(Pause);

            if (bn_stop != null)
                bn_stop.onClick.AddListener(Stop);

            my_uAudioStreamer.sendPlaybackState += new System.Action<uAudio_backend.PlayBackState>(Play);
        }
        public void Play(uAudio_backend.PlayBackState v)
        {
            UnityEngine.Debug.LogWarning("sendPlaybackState: " + v.ToString() + "---- 7ts87dt");
        }

        void onSongVolume_Change(float volumeIN)
        {
            Volume = volumeIN;
        }

        void onSongPath_Change(string PathIN)
        {
            targetFilePath = PathIN;

        }
        #endregion ---con---

        #region GUI
        public void SetStreamURL(string streamURL)
        {
            my_uAudioStreamer.SetStreamURL(streamURL);
        }

        string loadLinkFile(string fileIN)
        {
            if (System.IO.File.Exists(fileIN))
            {
                var s = System.IO.File.ReadAllLines(fileIN);
                string ss;
                if (s.Length > 0)
                {
                    ss = s[0];
                    System.Uri u = new System.Uri(ss);
                    fileIN = u.ToString();
                }
                else
                    fileIN = string.Empty;
            }

            LoadFile(fileIN);

            return fileIN;
        }
        #endregion ---GUI---

        #region actions
         void startPlay()
        {
            if (CurrentTime == System.TimeSpan.Zero)
                Play(null);
            else
                Play(CurrentTime);
        }
        public void Play(System.TimeSpan? OffsetStart = null)
        {
            if (!(my_uAudioStreamer.PlaybackState == uAudio_backend.PlayBackState.Playing))
            {
                halt = false;
                loadLinkFile(targetFilePath);
                my_uAudioStreamer.Play();


                if (Buffer != null)
                {
                    updateBufferLoop = updateBuffer();
                }

            }
        }

        public void Pause()
        {
            my_uAudioStreamer.Pause();
        }

        public void Stop()
        {
            my_uAudioStreamer.Stop();
            halt = true;
            updateBufferLoop = null;
            if (Buffer != null)
                Buffer.value = 0;
        }

        public void ChangeCurrentTime(TimeSpan timeIN)
        {
            //throw new NotImplementedException();
        }

        public void LoadFile(string targetFile)
        {
            //            LoadFile - Does Nothing on a stream
            my_uAudioStreamer.SetFile(targetFile);
        }

        public void SetFile(string targetFile)
        {
            my_uAudioStreamer.SetFile(targetFile);
        }

        public void SetURL(string targetFile)
        {
            my_uAudioStreamer.SetURL(targetFile);
        }

        void OnGUI()
        {
            if (updateBufferLoop != null)
                updateBufferLoop.MoveNext();
        }

        public void Dispose()
        {
            // alls good
            my_uAudioStreamer.Dispose();
        }
        #endregion ---actions---
    }
}