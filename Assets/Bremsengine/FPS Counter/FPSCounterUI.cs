using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Core.Extensions;
namespace Bremsengine
{
    public class FPSCounterUI : MonoBehaviour
    {
        public TMP_Text fpsText; // Assign a UI Text element in the inspector
        private Queue<float> frameTimes = new Queue<float>();
        private int bufferSize = 30;

        private void RefreshText()
        {
            float averageDeltaTime = 0.0f;
            foreach (float frameTime in frameTimes)
            {
                averageDeltaTime += frameTime;
            }
            if (frameTimes.Count == 0)
            {
                fpsText.text = "FPS : 0";
                return;
            }
            averageDeltaTime /= frameTimes.Count;

            float fps = 1.0f / averageDeltaTime.Max(0.0001f);
            fpsText.text = "FPS : "+Mathf.Ceil(fps).ToString("F0");
        }
        private void Start()
        {
            InvokeRepeating(nameof(RefreshText), 0.5f, 0.5f);
        }
        void Update()
        {
            float deltaTime = Time.unscaledDeltaTime;
            frameTimes.Enqueue(deltaTime);

            if (frameTimes.Count > bufferSize)
            {
                frameTimes.Dequeue();
            }
        }
    }
}
