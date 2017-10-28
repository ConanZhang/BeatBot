using UnityEngine;

namespace uAudio
{
    public class dirLoop_UI : MonoBehaviour
    {
        public uAudioPlayer_UI my_uAudioPlayer_UI;
        #region vars
        public UnityEngine.UI.InputField song_dirFilePath;

        public UnityEngine.UI.Button bn_song_pre;
        public UnityEngine.UI.Button bn_song_next;
        public UnityEngine.UI.Button bn_dir_update;

        #endregion ---vars---

        System.Collections.Generic.List<System.IO.FileInfo> songs = new System.Collections.Generic.List<System.IO.FileInfo>();
        #region con
        void Start()
        {
            if (song_dirFilePath != null)
            {
                LoadSongs();
            }
            else
            {
                my_uAudioPlayer_UI.setSongPath_Change(getCurrentSong().ToString());
            }

            if (bn_song_pre != null)
                bn_song_pre.onClick.AddListener(MovePreSong);

            if (bn_dir_update != null)
                bn_dir_update.onClick.AddListener(Update_dir);

            if (bn_song_next != null)
                bn_song_next.onClick.AddListener(MoveNextSong);

            my_uAudioPlayer_UI.my_uAudioPlayer.sendPlaybackState += new System.Action<uAudio_backend.PlayBackState>(OnPlayBackState);
        }

        bool readyNextSong = false;
        void OnPlayBackState(uAudio_backend.PlayBackState valIN)
        {
            //if (!readyNextSong)
            {
                if (valIN == uAudio_backend.PlayBackState.Stopped)
                {
                    if (my_uAudioPlayer_UI.my_uAudioPlayer.TotalTime == my_uAudioPlayer_UI.my_uAudioPlayer.CurrentTime)
                    {
                        //readyNextSong = true;
                   //     MoveNextSong();
                        //g5eh565h56
                        //readyNextSong = false;
                            StartCoroutine(runNextSong());
                        //     MoveNextSong();
                    }
                }
            }
        }
        System.Collections.IEnumerator runNextSong()
        {
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            MoveNextSong();
            //readyNextSong = false;
            yield return new WaitForFixedUpdate();
        }
        #endregion ---con---

        void LoadSongList(System.IO.DirectoryInfo dirIN)
        {
            songs.Clear();

            try
            {
                foreach (System.IO.FileInfo file in dirIN.GetFiles())
                {
                    if (file.Extension.ToLower() == ".mp3")
                    {
                        songs.Add(file);
                    }
                }

                if (songs.Count == 0)
                    UnityEngine.Debug.Log(System.Environment.NewLine + "no songs found in dir: " + dirIN);

            }
            catch
            {
#if uAudio_debug
            UnityEngine.Debug.Log(System.Environment.NewLine + "uAudioPlayer_UI: LoadSongList #876sdf787sfdds");
#endif
            }
        }

        System.IO.DirectoryInfo getTargetDir()
        {
            string s = song_dirFilePath.text;
            if (System.IO.Directory.Exists(s))
            {
                System.IO.DirectoryInfo d = new System.IO.DirectoryInfo(s);
                return d;
            }
            else
            {
                s = "file://" + s;
                if (System.IO.Directory.Exists(s))
                {
                    System.IO.DirectoryInfo d = new System.IO.DirectoryInfo(s);
                    return d;
                }
                else
                {
                    UnityEngine.Debug.Log(System.Environment.NewLine + "uAudioPlayer_UI: getTargetDir dir not Exists #casi7facs768fcas");
                    return null;
                }
            }
        }

        void LoadSongs()
        {
            try
            {
                System.IO.DirectoryInfo targetDir = getTargetDir();
                LoadSongList(targetDir);

                var targetFile = getCurrentSong();
                my_uAudioPlayer_UI.setSongPath_Change(targetFile.ToString());
            }
            catch
            {
                UnityEngine.Debug.LogWarning("dirLoop_Load - LoadSongs #9v78gbdv");
            }
        }

        int currentSong_index = 0;

        System.IO.FileInfo getNextSong()
        {
            if (songs.Count == 0)
            {
                LoadSongs();
                currentSong_index = 0;
            }
            else
                currentSong_index++;

            if (currentSong_index >= songs.Count)
                currentSong_index = 0;
            //if (currentSong != null)
            //{
            //    songs.Add(currentSong);
            //}

            //currentSong = ;

            return songs[currentSong_index];
        }

        System.IO.FileInfo getPreSong()
        {
            if (songs.Count == 0)
            {
                LoadSongs();
                currentSong_index = 0;
            }
            else
                currentSong_index--;


            if (currentSong_index < 0)
                currentSong_index = songs.Count - 1;

            return songs[currentSong_index];
        }

        System.IO.FileInfo getCurrentSong()
        {
            return songs[currentSong_index]; ;
        }

        public void MoveNextSong()
        {

            var oldState = my_uAudioPlayer_UI.my_uAudioPlayer.PlaybackState;
            if (my_uAudioPlayer_UI.my_uAudioPlayer.IsPlaying)
                my_uAudioPlayer_UI.Stop();
            var targetFile = getNextSong();
            my_uAudioPlayer_UI.setSongPath_Change(targetFile.ToString());
            my_uAudioPlayer_UI.songTime.value = 0;

            if (oldState == uAudio_backend.PlayBackState.Playing)
                StartCoroutine(spinPlay());
        }

        System.Collections.IEnumerator spinPlay()
        {
            yield return new WaitForFixedUpdate();
            yield return new WaitForSeconds(.1f);
            yield return new WaitForFixedUpdate();
            my_uAudioPlayer_UI.Play();
            yield return null;
        }

        void MovePreSong()
        {
            my_uAudioPlayer_UI.Stop();
            var targetFile = getPreSong();
            my_uAudioPlayer_UI.setSongPath_Change(targetFile.ToString());
            my_uAudioPlayer_UI.songTime.value = 0;
            StartCoroutine(spinPlay());
        }
        
        void Update_dir()
        {
            LoadSongs();
        }
    }
}