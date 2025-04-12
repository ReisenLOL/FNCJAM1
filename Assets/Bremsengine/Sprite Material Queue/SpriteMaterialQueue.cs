using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Bremsengine
{
    public class SpriteMaterialQueue : MonoBehaviour
    {
        [field: SerializeField] public SpriteRenderer SR { get; private set; }
        [SerializeField] List<SpriteRenderer> renderers = new();
        public Material StandardMaterial { get; private set; }
        static Coroutine activeroutine;
        [SerializeField] Material flashMaterial;
        [SerializeField] float flashInterval;
        private void Awake()
        {
            StandardMaterial = SR.sharedMaterial;
        }
        public void RunMaterialQueue(float duration)
        {
            SR.sharedMaterial = StandardMaterial;
            if (activeroutine != null)
            {
                SetRendererMaterial(StandardMaterial);
                StopCoroutine(activeroutine);
            }
            activeroutine = StartCoroutine(CO_FlashMaterial(flashMaterial, duration, flashInterval));
        }
        void SetRendererMaterial(Material m)
        {
            if (SR != null)
            {
                SR.sharedMaterial = m;
            }
            foreach (var r in renderers)
            {
                if (r == null)
                    continue;
                r.sharedMaterial = m;
            }
        }
        public IEnumerator CO_FlashMaterial(Material flashMaterial, float duration, float flashInterval)
        {
            SetRendererMaterial(StandardMaterial);
            float endTime = Time.time + duration;
            while (Time.time < endTime)
            {
                Material determinedMaterial = SR.sharedMaterial != flashMaterial ? flashMaterial : StandardMaterial;
                SetRendererMaterial(determinedMaterial);
                yield return new WaitForSeconds(flashInterval);
            }
            SetRendererMaterial(StandardMaterial);
        }
    }
}