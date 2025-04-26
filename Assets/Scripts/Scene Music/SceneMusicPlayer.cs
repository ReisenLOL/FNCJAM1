using Bremsengine;
using Core.Extensions;
using UnityEngine;

public class SceneMusicPlayer : MonoBehaviour
{
    public MusicWrapper toPlay;
    private void Start()
    {
        Play();
    }
    private void Play()
    {
        if (toPlay == null)
            return;
        toPlay.Play();
    }
}
