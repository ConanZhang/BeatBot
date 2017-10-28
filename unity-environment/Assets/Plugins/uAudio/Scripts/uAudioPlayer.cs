#pragma warning disable 0649
using UnityEngine;
using System;

namespace uAudio
{
    public class uAudioPlayer : MonoBehaviour, uAudio_backend.IAudioPlayer
    {
        #region var
        //public Action SongFinish;
        uAudio_backend.uAudio _uAudio;
        public AudioSource myAudioSource;
        //public System.Action PlayBackStopped { get { return _PlaybackStopped; } set { _PlaybackStopped = value; } }
        public Action<float> Update_UI_songTime;
        public string targetFile;
        Action<uAudio_backend.PlayBackState> _sendPlaybackState;
        public Action<uAudio_backend.PlayBackState> sendPlaybackState
        {
            get
            {
                return _sendPlaybackState;
            }

            set
            {
                _sendPlaybackState = value;
            }
        }
        public int SongLength { get { return _uAudio.SongLength; } }

        //System.Action _PlaybackStopped;
        bool updateTime = false;
        uAudio_backend.PlayBackState State;
        bool SongDone = false;
        bool flare_SongEnd = false;
        float[] _getAudioData_sampler;
        public NLayer.MpegFile playbackDevice;

        uAudioDemo.Mp3StreamingDemo.ReadFullyStream readFullyStream;
        //double TotalTime
        //{ get
        //    {
        //        if(UAudio!=null)
        //        return     UAudio.TotalTime.TotalSeconds;
        //        else
        //        return 0;
        //    } }
        [HideInInspector]
      //  [Range(0f,1.0f)]
        public float start_volume_Offset = 1;

        public float Volume_Offset
        {
            get
            {
                return start_volume_Offset;
            }

            set
            {
                if (_uAudio != null)
                    _uAudio.Volume = value;
                start_volume_Offset = value;
            }
        }
        public uAudio_backend.uAudio UAudio
        {
            get
            {
                if (_uAudio == null)
                {
                    _uAudio = new uAudio_backend.uAudio();
                    _uAudio.SetAudioFile(targetFile);
                    _uAudio.Volume = start_volume_Offset;
                    _uAudio.sendPlaybackState = (uAudio_backend.PlayBackState c) => {
                        _sendPlaybackState(c);
                    };
                }
                return _uAudio;
            }
        }

        public bool IsPlaying
        {
            get { return State == uAudio_backend.PlayBackState.Playing; }
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

        void uAudio_stopped()
        {
            updateTime = false;
        }

        public string AudioTitle
        {
            get
            {
                if (_uAudio != null)
                    return _uAudio.AudioTitle;
                else
                    return string.Empty;
            }
        }

        public TimeSpan CurrentTime
        {
            get
            {
                if (_uAudio != null && State != uAudio_backend.PlayBackState.Stopped)
                    //Mathf.Max(0, Mathf.FloorToInt((float)UAudio.CurrentTime.TotalSeconds - 1))
                    return _uAudio.CurrentTime;// - new TimeSpan(0, 0, 1);
                else
                    return endSongTime;
            }
            set
            {
                if (_uAudio != null)
                    _uAudio.CurrentTime = value;
            }
        }


        public float Pan
        {
            get
            {
                if (myAudioSource != null)
                    return myAudioSource.panStereo;
                else
                    return 0;
            }
            set
            {
                if (myAudioSource != null)
                    myAudioSource.panStereo = value;
            }
        }

        public TimeSpan TotalTime
        {
            get
            {
                if (_uAudio != null)
                    return _uAudio.TotalTime;
                else
                    return TimeSpan.Zero;
            }
        }

        public float Volume
        {
            get
            {

                return myAudioSource.volume;
            }
            set
            {
                myAudioSource.volume = value;
            }
        }

        public float Volume_BackEnd
        {
            get
            {
                if (_uAudio != null)
                    return _uAudio.Volume;
                else
                    return Volume_Offset;
            }
            set
            {
                if (_uAudio != null)
                    _uAudio.Volume = value;
                Volume_Offset = value;
            }
        }

        public uAudio_backend.PlayBackState PlaybackState
        {
            get
            {
                return State;
            }
        }

        public void ChangeCurrentVolume(float volumeIN)
        {
            Volume = volumeIN;
        }

        string uAudio_backend.IAudioPlayer.current_TargetFile_Loaded
        {
            get
            {
                return targetFile;
            }
        }

        public void ChangeCurrentTime(TimeSpan timeIN)
        {
            CurrentTime = timeIN;
        }
        #endregion ---vars---

        #region funcs
        void Update()
        {
            if (updateTime)
            {
                if (State == uAudio_backend.PlayBackState.Playing)
                {
                    float newValue = (float)CurrentTime.TotalSeconds;

                    if (Update_UI_songTime != null)
                        Update_UI_songTime(newValue);
                }
            }

            if (flare_SongEnd)
            {
                flare_SongEnd = false;
                SongEnd();
                flare_SongEnd = false;
            }
        }

        bool _loadedTarget = false;
        public void LoadFile(string targetFileIN)
        {
            targetFile = targetFileIN;
            if (!_loadedTarget || UAudio.targetFile != targetFile)
            {
                _loadedTarget = true;
                UAudio.LoadFile(targetFileIN);
            }
        }

        public void SetFile(string targetFileIN)
        {
            targetFile = targetFileIN;
        }
        TimeSpan endSongTime = TimeSpan.Zero;
        void SongEnd()
        {
            try
            {
                endSongTime = CurrentTime;
                if (readFullyStream != null)
                {
                    readFullyStream.Close();
                }
 
                myAudioSource.Stop();
                _uAudio.Stop();

                myAudioSource.clip = null;
                //    myAudioSource.time = 0;
                updateTime = false;
                _loadedTarget = false;
                State = uAudio_backend.PlayBackState.Stopped;
                  try
                {
                    if (sendPlaybackState != null)
                        sendPlaybackState(uAudio_backend.PlayBackState.Stopped);
                }
                catch
                {
                    UnityEngine.Debug.LogWarning("sendPlaybackState #897j8h2432a1q");
                }
            }
            catch
            {
                throw new Exception("Song end #7cgf87dcf7sd8csd");
            }
        }

        public void Play(TimeSpan? startOff= null)
        {
#if uAudio_debug
            UnityEngine.Debug.LogWarning("&c&");
#endif
            if (State != uAudio_backend.PlayBackState.Playing)
            {
#if uAudio_debug
                UnityEngine.Debug.LogWarning("&d&");
#endif
                if (State == uAudio_backend.PlayBackState.Paused)
                    Pause();
                else
                {
                    State = uAudio_backend.PlayBackState.Playing;

                    try
                    {
                        //LoadFile(targetFile);
#if uAudio_debug
                        UnityEngine.Debug.LogWarning("B");
#endif
                        SongDone = false;
                        flare_SongEnd = false;
                        UAudio.targetFile = targetFile;

                        if (myAudioSource.clip == null)
                        {
#if uAudio_debug
                            UnityEngine.Debug.LogWarning("%B1%");
#endif
                            if (UAudio.LoadMainOutputStream())
                            {
#if uAudio_debug
                                UnityEngine.Debug.LogWarning("%B2%");
#endif

#if !UNITY_STANDALONE_WIN
                            
                                //WWW w = new WWW(targetFile);

                                                                //int song_sampleSize;
                                                                //SongReadLoop = new AudioClip.PCMReaderCallback(Song_Stream_Loop);
#if uAudio_debug
                                                                UnityEngine.Debug.LogWarning("%B4%");
#endif
                                                                //song_sampleSize = (int)UAudio.uwa.audioPlayback.TotalSamples;
#if uAudio_debug
                                                                UnityEngine.Debug.LogWarning("%B5%");
#endif
                                                                if (!targetFile.StartsWith("file://"))
                                                                    targetFile = "file://" + targetFile;

                                                                var www = new UnityEngine.WWW(targetFile);

                                                                UnityEngine.AudioClip myAudioClip = www.GetAudioClip(true, false, UnityEngine.AudioType.MPEG);// UnityEngine.AudioType.MPEG);

                                                                //  yield return www;
                                                                // Next line hangs
                                                                var clip = myAudioClip;
                                                                int i = 100;

                                                                // todo : check this -=---> clip.loadState != UnityEngine.AudioDataLoadState.Loaded
#if !UNITY_2_6 && !UNITY_2_6_1 && !UNITY_3_0 && !UNITY_3_0_0 && !UNITY_3_1 && !UNITY_3_2 && !UNITY_3_3 && !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_0_1 && !UNITY_4_1 && !UNITY_4_2 && !UNITY_4_3 && !UNITY_4_5 && !UNITY_4_6 && !UNITY_4_7
                                                                while (clip.loadState != UnityEngine.AudioDataLoadState.Loaded && i > 0)
#endif
                                                                {
                                                                    i--;
                                                                    System.Threading.Thread.Sleep(10);
                                                                }
                                                                myAudioSource.clip = clip;
#else

#if !UNITY_STANDALONE_WIN && !UNITY_EDITOR
                                if (!targetFile.StartsWith("file://"))
                                    targetFile = "file://" + targetFile;
#endif


                                long song_sampleSize;
#if uAudio_debug
                                UnityEngine.Debug.LogWarning("%B3%");
#endif
                                AudioClip.PCMReaderCallback SongReadLoop;
                                SongReadLoop = new AudioClip.PCMReaderCallback(Song_Stream_Loop);

                                System.IO.Stream s = System.IO.File.OpenRead(targetFile);

                                if (readFullyStream != null)
                                    readFullyStream.Dispose();

                                readFullyStream = new uAudioDemo.Mp3StreamingDemo.ReadFullyStream(s);
                                readFullyStream.stream_CanSeek = true;

                                byte[] buff = new byte[1024];
                                NLayer.MpegFile c = new NLayer.MpegFile(readFullyStream, true);
                                //playbackDevice = m;

                                if(startOff == TimeSpan.Zero)
                                c.ReadSamples(buff, 0, buff.Length);

                                playbackDevice = c;
                             //   UAudio.TotalTime
                                //     song_sampleSize = playbackDevice.Length;// * playbackDevice.SampleRate;
                                song_sampleSize = UAudio.SongLength;
                             // hack
                             // song_sampleSize = int.MaxValue;// need to better alocate this

                                int setSongSize;
                                if (song_sampleSize > int.MaxValue)
                                {
                                    UnityEngine.Debug.LogWarning("uAudioPlayer - Song size over size on int #4sgh54h45h45");
                                    setSongSize = int.MaxValue;
                                }
                                else
                                    setSongSize = (int)song_sampleSize;

                                myAudioSource.clip =
                              UnityEngine.AudioClip.Create("uAudio_song", setSongSize,
                                        c.WaveFormat.Channels,
                                        c.WaveFormat.SampleRate,
                                        true, SongReadLoop);

                                if (!startOff.HasValue)
                                    CurrentTime = TimeSpan.Zero;
                                else
                                    CurrentTime = startOff.Value;
#endif


                                try
                                {
                                    if (sendPlaybackState != null)
                                        sendPlaybackState(uAudio_backend.PlayBackState.Playing);
                                }
                                catch
                                {
                                    UnityEngine.Debug.LogWarning("theAudioStream_sendStartLoopPump #32fw46hw465h45h");
                                }
#if uAudio_debug
                                UnityEngine.Debug.LogWarning("%B6%");
#endif
                            }
                            else
                                myAudioSource.clip = null;
                        }
                        else
                        {
#if uAudio_debug
                            UnityEngine.Debug.LogWarning("%B!%");
#endif
                        }

                        if (myAudioSource.clip != null)
                        {
#if uAudio_debug
                            UnityEngine.Debug.LogWarning("%B7%");
#endif
                            if (!myAudioSource.isPlaying)
                                myAudioSource.Play();

#if uAudio_debug
                            UnityEngine.Debug.LogWarning("%B8%");
#endif
                            updateTime = true;
                        }
                        else
                            State = uAudio_backend.PlayBackState.Stopped;
                    }
                    catch (System.Exception ex)
                    {
#if uAudio_debug
                        UnityEngine.Debug.LogWarning("%B9%"+System.Environment.NewLine+ex.Message);
#endif
                        State = uAudio_backend.PlayBackState.Stopped;
                        UnityEngine.Debug.LogWarning("uAudioPlayer - Play #j356j536j356j56j");
                        UnityEngine.Debug.LogWarning(ex);
                    }
                }
            }
        }

        void Song_Stream_Loop(float[] data)
        {
            try
            {
                if (!SongDone)
                {
                    //if (_getAudioData_sampler == null)
                    //    _getAudioData_sampler = new float[data.Length];

                    //int got = playbackDevice.Read(data, 0, data.Length);
                    int got = _uAudio.uwa.audioPlayback.inputStream.Read(data, 0, data.Length);
                    //int got = _uAudio.uwa.audioPlayback.inputStream.Read(_getAudioData_sampler, 0, _getAudioData_sampler.Length);

                    if (got <= 0)
                    {
                        SongDone = true;
                    }
                    //    for (int i = got - 1; i < _getAudioData_sampler.Length; i++)
                    //        _getAudioData_sampler[i] = 0;
                    //}

                    //Array.Copy(_getAudioData_sampler, data, data.Length);
                }
                else
                {
                    flare_SongEnd = true;
                }
            }
            catch
            {
                UnityEngine.Debug.LogError("Decode Error #8f76s8dsvsd");
            }
        }

        public void Pause()
        {
            if (State == uAudio_backend.PlayBackState.Playing)
            {
                myAudioSource.Pause();
                State = uAudio_backend.PlayBackState.Paused;
                try
                {
                    if (sendPlaybackState != null)
                        sendPlaybackState(uAudio_backend.PlayBackState.Paused);
                }
                catch
                {
                    UnityEngine.Debug.LogWarning("sendPlaybackState #whrth546h56h56");
                }
            }
            else
            {
                if (State == uAudio_backend.PlayBackState.Paused)
                {
                    myAudioSource.UnPause();
                    State = uAudio_backend.PlayBackState.Playing; try
                    {
                        if (sendPlaybackState != null)
                            sendPlaybackState(uAudio_backend.PlayBackState.Playing);
                    }
                    catch
                    {
                        UnityEngine.Debug.LogWarning("sendPlaybackState #56y53y5tge56");
                    }
                }
            }
        }

        public void Stop()
        {
            if (State != uAudio_backend.PlayBackState.Stopped)
            {
                SongEnd();
            }
        }
        #endregion ---funcs---

        #region Dispose
        void OnApplicationQuit()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (readFullyStream != null)
            {
                readFullyStream.Close();
            }
            if (_uAudio != null)
            {
                _uAudio.Dispose();
                _uAudio = null;
            }
            _loadedTarget = false;
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        //public void Init(IWaveProvider waveProvider)
        //{
        //    myWaveProvider = waveProvider;
        //}
        #endregion ---Dispose---
    }
}