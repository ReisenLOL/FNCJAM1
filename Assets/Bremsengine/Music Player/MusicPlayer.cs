using Core.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bremsengine
{
    public class MusicPlayer : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ReinitializeActiveTrack()
        {
            currentlyPlaying = new();
        }
        public struct activeTrack
        {
            public int track;
            public MusicWrapper music;
        }
        static activeTrack currentlyPlaying;
        public static bool IsPlayingOnTrack(int track, MusicWrapper music)
        {
            if (currentlyPlaying.music != music)
            {
                return false;
            }
            return currentlyPlaying.track == track;
        }
        public static float GlobalVolume { get; private set; }
        [SerializeField] MusicWrapper testStartingMusic;
        static Queue<MusicWrapper> Playlist = new();
        [SerializeField] List<MusicWrapper> testPlaylist = new();
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ClearPlaylist()
        {
            if (Playlist == null)
            {
                Playlist = new Queue<MusicWrapper>();
            }
            Playlist.Clear();
        }
        public static void AddToPlaylist(MusicWrapper w)
        {
            Playlist.Enqueue(w);
        }
        private void Start()
        {
            if (testStartingMusic != null)
            {
                PlayMusicWrapper(testStartingMusic);
            }
            foreach (var item in testPlaylist)
            {
                if (item == null)
                    continue;
                Playlist.Enqueue(item);
            }
        }
        private void FixedUpdate()
        {
            if (!Application.isFocused)
                return;
            if (IsPlaying)
                return;

            if (Playlist.Count <= 0)
            {
                return;
            }
            MusicWrapper wrapper = Playlist.Dequeue();
            wrapper.Play();
        }
        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            GlobalVolume = 0.75f;
            if (track1 == null) track1 = new GameObject("Music Track 1").transform.SetParentDecorator(transform).gameObject.AddComponent<AudioSource>();
            if (track2 == null) track2 = new GameObject("Music Track 2").transform.SetParentDecorator(transform).gameObject.AddComponent<AudioSource>();
            transform.SetParent(null);
            instance = this;
            DontDestroyOnLoad(transform.gameObject);
        }
        static MusicPlayer instance;
        [SerializeField] AudioSource track1;
        [SerializeField] AudioSource track2;
        MusicWrapper song1;
        MusicWrapper song2;
        [SerializeField] float crossFadeLength = 1f;
        int selectedTrack = 0;
        public static bool IsPlaying => instance.track1.isPlaying || instance.track2.isPlaying;
        public static void PlayMusicWrapper(MusicWrapper mw)
        {
            if (instance == null)
            {
                Debug.LogError("Music Player Not Instantiated Properly");
                return;
            }
            if (mw == null)
            {
                Debug.Log("Music Wrapper is null");
                return;
            }
            if (mw.dontReplaceSelf && IsPlayingOnTrack(instance.selectedTrack, mw))
            {
                return;
            }
            MusicPopup.QueuePopup(mw.TrackName);
            instance.PlayCrossfade(mw, !IsPlaying ? 0f : instance.crossFadeLength);
        }
        private void PlayCrossfade(MusicWrapper clip, float crossfade = 0.5f)
        {
            StartCoroutine(FadeTrack(clip, crossfade));
        }
        private IEnumerator FadeTrack(MusicWrapper clip, float crossfade)
        {
            activeTrack newTrack = new();
            crossfade = crossfade.Max(0.00f);
            float timeElapsed = 0f;
            if (clip.musicClip == null)
            {
                Debug.Log("Missing Audio Clip");
                yield break;
            }
            if (selectedTrack != 2)
            {
                track2.clip = clip;
                song2 = clip;
                selectedTrack = 2;
                if (crossfade == 0)
                {
                    track2.volume = clip.musicVolume;
                    track1.volume = 0f;
                }
                else
                {
                    while (timeElapsed < crossfade)
                    {
                        track1.volume = Mathf.Lerp(song1, 0f, timeElapsed / crossfade);
                        timeElapsed += Time.deltaTime;
                        yield return null;
                    }
                }
                track1.Stop();
                track2.volume = clip.musicVolume;
                track2.Play();
            }
            else
            {
                track1.clip = clip;
                song1 = clip;
                selectedTrack = 1;
                if (crossfade == 0)
                {
                    track1.volume = clip.musicVolume;
                    track2.volume = 0f;
                }
                else
                {
                    while (timeElapsed <= crossfade)
                    {
                        track2.volume = Mathf.Lerp(song2, 0f, timeElapsed / crossfade);
                        timeElapsed += Time.deltaTime;
                        yield return null;
                    }
                }
                track2.Stop();
                track1.volume = clip.musicVolume;
                track1.Play();
            }
            newTrack.track = selectedTrack;
            newTrack.music = clip;
            currentlyPlaying = newTrack;
        }
    }
}
