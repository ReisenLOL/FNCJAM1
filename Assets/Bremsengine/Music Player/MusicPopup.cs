using Core.Extensions;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Bremsengine
{
    public class MusicPopup : MonoBehaviour
    {
        static MusicPopup runner;
        [Range(1f,500f)]
        [SerializeField] float lerpSpeed = 150f;
        [SerializeField] TMP_Text musicText;
        static Coroutine activeRoutine;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Reinitialize()
        {
            activeRoutine = null;
            Queued = false;
            QueuedSongText = "";
        }
        static bool Queued;
        static string QueuedSongText;
        public static void QueuePopup(string songText)
        {
            Queued = true;
            QueuedSongText = songText;
        }
        private void Awake()
        {
            runner = this;
            musicText.color = musicText.color.Opacity(0);
        }
        private void Start()
        {
            ShowText();
        }
        private byte ToByte(float f)
        {
            return (byte)f.Floor();
        }
        IEnumerator CO_DisplaySongText(float fadeSpeed)
        {
            float opacity = 0f;
            byte opacityByte = 0;
            musicText.text = string.IsNullOrEmpty(QueuedSongText) ? "" : "BGM: "+ QueuedSongText;
            while (opacityByte < 255)
            {
                opacity = opacity.MoveTowards(256f, fadeSpeed * Time.deltaTime);
                opacityByte = ToByte(opacity);
                musicText.color = musicText.color.Opacity(opacityByte);
                yield return null;
            }
            yield return new WaitForSecondsRealtime(2.5f);
            while (opacityByte > 0)
            {
                opacity = opacity.MoveTowards(0f, fadeSpeed * Time.deltaTime);
                opacityByte = ToByte(opacity);
                musicText.color = musicText.color.Opacity(opacityByte);
                yield return null;
            }
            musicText.text = "";
            activeRoutine = null;
        }
        private void Update()
        {
            if (runner != null && Queued && !string.IsNullOrEmpty(QueuedSongText))
            {
                ShowText();
            }
        }
        private void ShowText()
        {
            Queued = false;
            activeRoutine = StartCoroutine(CO_DisplaySongText(runner.lerpSpeed));
        }
    }
}
