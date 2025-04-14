using Core.Extensions;
using UnityEngine;

namespace Bremsengine
{
    [CreateAssetMenu(menuName = "Bremsengine/MusicWrapper")]
    [System.Serializable]
    public class MusicWrapper : ScriptableObject
    {
        public static implicit operator AudioClip(MusicWrapper mw) => mw == null ? null : mw.musicClip;
        public static implicit operator float(MusicWrapper mw) => mw == null ? 0f : mw.musicVolume;
        public string TrackName = Helper.DefaultName;
        public AudioClip musicClip;
        public float musicVolume => clipVolume * MusicPlayer.GlobalVolume;
        [SerializeField] float clipVolume = 0.7f;
        [field: SerializeField] public bool dontReplaceSelf { get; private set; } = true;
        private void OnValidate()
        {
            this.FindStringError(nameof(TrackName), TrackName);
        }
        public void Play()
        {
            MusicPlayer.PlayMusicWrapper(this);
        }
    }
}
