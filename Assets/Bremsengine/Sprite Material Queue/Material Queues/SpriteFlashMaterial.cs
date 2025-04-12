using System.Collections;
using UnityEngine;

namespace Bremsengine
{
    public class SpriteFlashMaterial : MonoBehaviour
    {
        [SerializeField] SpriteMaterialQueue materialQueue;

        [ContextMenu("Activate Flash")]
        public void TriggerFlashMaterial(float duration)
        {
            materialQueue.RunMaterialQueue(duration);
        }
    }
}
