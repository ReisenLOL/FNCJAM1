using Bremsengine;
using Core.Extensions;
using UnityEngine;

public class SceneMusicPlayer : MonoBehaviour
{
    [SerializeField] MusicWrapper toPlay;
    private void Start()
    {
        if (toPlay == null)
            return;
        toPlay.Play();
    }
}
