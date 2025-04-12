using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bremsengine.Assets.Bremsengine.Sprite
{
    public class SpriteColorSetter : MonoBehaviour
    {
        [SerializeField] List<SpriteRenderer> renderers = new();
        [SerializeField] Color32 color;
        private void OnValidate()
        {
            foreach (var renderer in renderers)
            {
                if (renderer == null)
                {
                    continue;
                }
                renderer.color = color;
            }
        }
    }
}