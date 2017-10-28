
#pragma warning disable 0414

using System;

namespace uAudio
{
    public class uAudioStreamer : UnityEngine.MonoBehaviour, uAudio_backend.IAudioPlayer
    {
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
        // bind off here
        void theAudioStream_sendPlaying()
        {
            //songVolume.value = Volume;

            //#if UNITY_STANDALONE_WIN
            //        TheAudioStream.playbackState = uAudio_Streaming.StreamingPlaybackState.Playing;
            //#endif
        }

        // bind off here
        public void ChangeCurrentTime(TimeSpan timeIN)
        {
            //throw new NotImplementedException();
        }

        #region vars

        /// <summary>
        /// This #if RemoveThread_uAudio
        /// is inplace for systems that need the threading calls removed from the uAudio plugin
        /// </summary>
#if !RemoveThread_uAudio
        public bool betaNativeThreadBuffering = false;
#endif
        public string targetFilePath;
        public string theUrl = string.Empty;

#if !RemoveThread_uAudio
        System.Threading.Thread myThreadPump;
        System.Threading.Thread myThreadPump2;
#endif
        //System.Collections.IEnumerator myUpdateLoop;

        bool callUpdateNeeded = false;
        //bool hot = false;
        public uAudioPlayer my_uAudioPlayer;

        public float minBufferTime = 2f;
        public float maxBufferTime = 20f;

        uAudio_backend.uAudio_Streaming _theAudioStream;
        uAudio_backend.uAudio_Streaming TheAudioStream
        {
            get
            {
                if (_theAudioStream == null)
                {
                    build_theAudioStream();
                }
                return _theAudioStream;
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
                return theUrl;
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
                if (_theAudioStream != null)
                {
                    if (TheAudioStream.playbackState == uAudio_backend.uAudio_Streaming.StreamingPlaybackState.Playing)
                        return uAudio_backend.PlayBackState.Playing;
                    else
                        if (TheAudioStream.playbackState == uAudio_backend.uAudio_Streaming.StreamingPlaybackState.Paused)
                        return uAudio_backend.PlayBackState.Paused;
                    else
                        return uAudio_backend.PlayBackState.Stopped;
                }
                else
                    return uAudio_backend.PlayBackState.Stopped;
            }
        }

        public float Volume
        {
            get
            {
                return my_uAudioPlayer.Volume;
            }
            set
            {
                my_uAudioPlayer.Volume = value;
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
                return my_uAudioPlayer.Pan;
            }
            set
            {
                my_uAudioPlayer.Pan = value;
            }
        }

        //public Action PlayBackStopped
        //{
        //    get
        //    {
        //        return my_uAudioPlayer.PlayBackStopped;
        //    }
        //    set
        //    {
        //        my_uAudioPlayer.PlayBackStopped = value;
        //    }
        //}
        public bool ForceBuffering
        {
            get
            {
                return TheAudioStream.forceBuffering;
            }

            set
            {
                TheAudioStream.forceBuffering = value;
            }
        }

#if !RemoveThread_uAudio
        public bool BetaNativeThreadBuffering
        {
            get
            {
                if (TheAudioStream != null)
                    TheAudioStream.BetaNativeThreadBuffering = betaNativeThreadBuffering;

                return betaNativeThreadBuffering;
            }

            set
            {
                betaNativeThreadBuffering = value;
            }
        }
#endif
#endregion ---vars---

#region con
        void onSongVolume_Change(float volumeIN)
        {
            Volume = volumeIN;
        }

        void build_theAudioStream()
        {
            try
            {
#if uAudio_debug
                UnityEngine.Debug.Log("load");
                UnityEngine.Debug.Log("1");
#endif
                _theAudioStream = new uAudio_backend.uAudio_Streaming();
#if UNITY_STANDALONE_WIN
                _theAudioStream.targetWindows = true;
#else
        _theAudioStream.targetWindows = false;
#endif
                _theAudioStream.my_uAudioPlayer2 = my_uAudioPlayer;
                _theAudioStream.startVolume = my_uAudioPlayer.Volume;
#if uAudio_debug
      UnityEngine.Debug.Log("2");
#endif
                _theAudioStream.Volume = my_uAudioPlayer.Volume_BackEnd;
                _theAudioStream.sendPlaying += theAudioStream_sendPlaying;
                _theAudioStream.Disposed += theAudioStream_Disposed;
                _theAudioStream.minBufferTime = minBufferTime;
#if uAudio_debug
        UnityEngine.Debug.Log("3");
#endif
                _theAudioStream.maxBufferTime = maxBufferTime;


#if !RemoveThread_uAudio
                _theAudioStream.BetaNativeThreadBuffering = betaNativeThreadBuffering;
#else
                _theAudioStream.BetaNativeThreadBuffering = false;
#endif
                 TheAudioStream.sendPlaybackState =(uAudio_backend.PlayBackState c)=>{
                    _sendPlaybackState(c); } ;

                //_theAudioStream.sendStartLoopPump += theAudioStream_sendStartLoopPump;
                //_theAudioStream.sendStopLoopPump += theAudioStream_sendStopLoopPump;

                //my_uAudioPlayer.myAudioSource.clip =
                //       UnityEngine.AudioClip.Create("uAudio_song", int.MaxValue,
                //       2,
                //       44100,
                //       true, new UnityEngine.AudioClip.PCMReaderCallback(_theAudioStream.ReadData));

                myLoopRead = LoopRead();
#if uAudio_debug
                UnityEngine.Debug.Log("4");
#endif
            }
            catch
            {
                UnityEngine.Debug.LogWarning("Build _theAudioStream crash #78vfg78dv");
            }
        }
        public double BufferedTime
        {
            get
            {
                return _theAudioStream.BufferedTime;
            }
        }


#if !RemoveThread_uAudio
        void loadAudio()
        {
#if uAudio_debug
    UnityEngine.Debug.Log("[-]");
#endif
            //"http://www.vorbis.com/music/Epoq-Lepidoptera.ogg";
            //  var url = "http://freedownloads.last.fm/download/569264057/Get%2BGot.mp3";
            //var url = "https://ec-media.soundcloud.com/zjQtTU5AG702.128.mp3?ff61182e3c2ecefa438cd02102d0e385713f0c1faf3b033959566bf30800eb12c470b699aa5404af4caebdb44e040797569796247388667238773f3bc5b1c3b3e0ef13b93f&AWSAccessKeyId=AKIAJ4IAZE5EOI7PA7VQ&Expires=1389750625&Signature=V%2BaRvxSfzKn%2FIgDlp58MgmxfDK0%3D";

            if (BetaNativeThreadBuffering)
            {
#if UNITY_EDITOR
                if (!UnityEngine.Application.isEditor || UnityEditor.EditorApplication.isPlaying)
#endif
                {
                    if (myThreadPump == null)
                    {
                        myThreadPump = new System.Threading.Thread(new System.Threading.ThreadStart(delegate
                        {
                            try
                            {
                                //hot = true;
                                RunPlay();
                            }
                            catch
                            {
                                UnityEngine.Debug.LogWarning("uAudioStreamer - loadAudio() Thread crash #65vh56h6");
                            }
                        }));
                        myThreadPump.IsBackground = true;
                        myThreadPump.Start();
                        //if (myThreadPump2 == null)
                        //{
                        //    myThreadPump2 = new System.Threading.Thread(new System.Threading.ThreadStart(delegate
                        //    {
                        //        try
                        //        {
                        //            while (true)
                        //            {
                        //                if (TheAudioStream.playbackState == uAudio_backend.uAudio_Streaming.StreamingPlaybackState.Stopped)
                        //                {
                        //                    System.Threading.Thread.Sleep(500);
                        //                }
                        //                else
                        //                {
                        //                    if (callUpdateNeeded)
                        //                        TheAudioStream.myUpdateLoop.MoveNext();
                        //                }
                        //                System.Threading.Thread.Sleep(100);
                        //            }
                        //        }
                        //        catch
                        //        {
                        //            UnityEngine.Debug.LogWarning("uAudioStreamer - loadAudio() Thread crash #89vg9vfdv");
                        //        }
                        //    }));
                        //    myThreadPump2.IsBackground = true;
                        //    myThreadPump2.Start();
                        //}
                    }
                }
            }
        }
#endif
        System.Collections.IEnumerator myLoopRead;

        System.Collections.IEnumerator LoopRead()
        {
            while (true)
            {
                if (TheAudioStream.playbackState == uAudio_backend.uAudio_Streaming.StreamingPlaybackState.Stopped)
                {
                    System.Threading.Thread.Sleep(300);
                    yield return null;
                }
                else
                {
                    try
                    {
                        if (callUpdateNeeded)
                        {
                            if (TheAudioStream.myUpdateLoop != null)
                                TheAudioStream.myUpdateLoop.MoveNext();
                        }
                    }
                    catch
                    {
                        UnityEngine.Debug.LogWarning("uAudioStreamer - loadAudio() Thread crash #89vg9vfdv");
                    }
                    yield return null;
                }
            }
        }
#endregion ---con---

#region events
        void theAudioStream_sendStartLoopPump()
        {
#if uAudio_debug
            UnityEngine.Debug.Log(System.Environment.NewLine + "STart_PUMP_LOOP: ^a");
#endif
            //hot = true;
            callUpdateNeeded = true;
        }

        void theAudioStream_sendStopLoopPump()
        {
#if uAudio_debug
            UnityEngine.Debug.Log(System.Environment.NewLine + "STOP_PUMP_LOOP: ^x");
#endif
            //hot = false;
        }
#endregion ---events---

#region loop
        void Update()
        {
            //if (hot)
            //{
            //    //  if (!BetaNativeThreadBuffering)
            //    {
            //        if(myLoopRead!=null)
            //        myLoopRead.MoveNext();
            //    }
            //}
            if (_theAudioStream != null)
                if (TheAudioStream.runPlay)
                {
                    TheAudioStream.runPlay = false;
                    StartCoroutine(firePlay(CurrentTime));
                    //if (TheAudioStream.reader.Channels>0)
                    {

                    }
                }
        }
        System.Collections.IEnumerator firePlay(System.TimeSpan? OffsetStart)
        {

#if uAudio_debug
            UnityEngine.Debug.Log(System.Environment.NewLine + "1");
#endif
            yield return new UnityEngine.WaitForSeconds(.1f);

#if uAudio_debug
            UnityEngine.Debug.Log(System.Environment.NewLine + "build clip");
#endif
            my_uAudioPlayer.myAudioSource.clip = UnityEngine.AudioClip.Create("uAudio_song", int.MaxValue,
           TheAudioStream.reader.WaveFormat.Channels,
          TheAudioStream.reader.WaveFormat.SampleRate,
           true, new UnityEngine.AudioClip.PCMReaderCallback(TheAudioStream.ReadData));

#if uAudio_debug
            UnityEngine.Debug.Log(System.Environment.NewLine + "done build clip");
#endif
            yield return new UnityEngine.WaitForSeconds(.1f);

            TheAudioStream.callPlay(OffsetStart);
        }

#endregion ---loop---

#region GUI
        public void PlayAudioFile(UnityEngine.UI.InputField FileNameIN,System.TimeSpan? OffsetStart)
        {
            PlayAudioStream(FileNameIN.text,OffsetStart);
        }

        public void PlayAudioStream(string FileNameIN,System.TimeSpan? OffsetStart)
        {
            if (!TheAudioStream.IsPlaying)
            {
#if uAudio_debug
   UnityEngine.Debug.Log("5");
#endif
                LoadFile(FileNameIN);
                Play(OffsetStart);
                theAudioStream_sendStartLoopPump();
#if uAudio_debug
               UnityEngine.Debug.Log("6");
#endif
            }

        }

        public void SetStreamURL(string streamURL)
        {
#if uAudio_debug
       UnityEngine.Debug.Log(System.Environment.NewLine + "Boot Load song: " + streamURL + System.Environment.NewLine);
#endif
            theUrl = streamURL;
        }

        public void PlayPauseAudio()
        {
            if (theUrl == string.Empty)
                theUrl = loadLinkFile(targetFilePath);

            TheAudioStream.LoadFile(theUrl);
            TheAudioStream.Play();
#if uAudio_debug
         UnityEngine.Debug.Log(System.Environment.NewLine + "Play Song song: " + System.Environment.NewLine);
#endif
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

            return fileIN;
        }
#endregion ---GUI---

#region actions
        public void Play(System.TimeSpan? OffsetStart=null)
        {

#if !RemoveThread_uAudio
            if (!betaNativeThreadBuffering)
            {
#endif
                RunPlay();
                //hot = true;
                callUpdateNeeded = true;

#if !RemoveThread_uAudio
        }
            else
            {
                loadAudio();
            }
#endif
        }
        void RunPlay()
        {
#if uAudio_debug
      UnityEngine.Debug.Log("7");
#endif
            PlayPauseAudio();
            try
            {
                if (sendPlaybackState != null)
                    sendPlaybackState(uAudio_backend.PlayBackState.Playing);
            }
            catch
            {
                UnityEngine.Debug.LogWarning("sendPlaybackState #y8h7y87t87t");
            }
#if uAudio_debug
          UnityEngine.Debug.Log("8");
#endif
        }
        public void Pause()
        {
            if (_theAudioStream != null)
                TheAudioStream.Pause();
        }

        public void Stop()
        {
            if (_theAudioStream != null)
            {
                my_uAudioPlayer.Stop();
                TheAudioStream.Stop();
                Halt();
            }
        }

        public void Halt()
        {
#if uAudio_debug
       UnityEngine.Debug.Log("end");
#endif

#if !RemoveThread_uAudio
            //hot = false;
            if (myThreadPump != null)
            {
                myThreadPump.Abort();
                myThreadPump = null;
            }
#endif
            if (_theAudioStream != null)
            {
                _theAudioStream.Dispose();
                _theAudioStream = null;
            }
        }


        public void LoadFile(string targetFile)
        {
            targetFilePath = targetFile;
            theUrl = loadLinkFile(targetFilePath);
        }

        public void SetFile(string targetFile)
        {
            SetURL(targetFile);
        }

        public void SetURL(string targetFile)
        {
            theUrl = targetFile;
        }
#endregion ---actions---

#region Dispose  
        void theAudioStream_Disposed(object sender, System.EventArgs e)
        {

#if !RemoveThread_uAudio
            if (myThreadPump != null)
            {
                myThreadPump.Abort();
                myThreadPump = null;
            }
#endif
            _theAudioStream = null;
        }

        void OnApplicationQuit()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_theAudioStream != null)
            {
                try
                {
                    _theAudioStream.Stop();
                }
                catch { }

                _theAudioStream.Dispose();
                _theAudioStream = null;
            }

#if !RemoveThread_uAudio
            if (myThreadPump != null)
            {
                myThreadPump.Abort();
                myThreadPump = null;
            }
#endif
        }
#endregion ---Dispose---
    }
}