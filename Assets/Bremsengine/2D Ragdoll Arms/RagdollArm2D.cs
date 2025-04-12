using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Extensions;

namespace Bremsengine
{
    public class RagdollArm2D : MonoBehaviour
    {
        float rotation = 0f; // this is nan
        float velocityY = 0f;
        [SerializeField] float lerpSpeed = 10f;
        [SerializeField] float idleRotation = -45f;
        [SerializeField] float fallRotation = 45f;
        float lerp = 0f; //this is also nan
        Vector2 lastPosition = Vector2.zero;
        [SerializeField] bool flip;
        private void Update()
        {
            velocityY = (transform.position.y - lastPosition.y) * (1f / Time.deltaTime);
            float fallDot = (15f - (15f - velocityY)) / 15f;

            lerp = (float.IsNaN(lerp) ? 0f : lerp) + (-fallDot * Time.deltaTime * lerpSpeed);
            if (lerp > 0f && velocityY > -2f)
            {
                lerp -= Time.deltaTime * lerpSpeed;
            }
            lerp = lerp.Clamp(0f, 1f);
            lastPosition = transform.position;

            rotation = idleRotation.Lerp(fallRotation, lerp);
            Vector2 look = (Vector2)transform.position + (!flip ? Vector2.right.Rotate2D(rotation) : Vector2.left.Rotate2D(rotation));
            transform.Lookat2D(look);
        }
    }
}
