using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Extensions;
using UnityEditor;

namespace Bremsengine
{
    public class GooglyEye2D : MonoBehaviour
    {
        Vector2 Velocity;
        Vector3 LastPosition;

        [SerializeField] float maxSpeedEffect = 4f;
        [SerializeField] float size = 0.2f;
        [SerializeField] Transform eyeball;
        [SerializeField] Transform iris;
        float gravityLerp;
        [SerializeField] float gravity = 10f;
        private void ApplySize(float size)
        {
            if (eyeball.GetComponent<SpriteRenderer>() is SpriteRenderer e)
            {
                e.size = new(size, size);
            }
            if (iris.GetComponent<SpriteRenderer>() is SpriteRenderer i)
            {
                i.size = new Vector2(size, size) * 0.5f;
            }
        }
        private void Awake()
        {
            ApplySize(size);
        }
        public Vector2 DetermineDesiredPosition(Vector2 direction)
        {
            if (eyeball == null ||
                iris == null)
            {
                if (eyeball == null)
                {
                    return eyeball.transform.position;
                }
                return Vector2.zero;
            }

            float lerp = Velocity.magnitude / maxSpeedEffect.Max(0.1f);
            lerp = lerp.Clamp(0f, 1f);

            Vector2 position1 = eyeball.position;
            Vector2 position2 = (Vector2)eyeball.position - direction.normalized * (size / 4f);

            return Vector2.Lerp(position1, position2, lerp);
        }
        private void Update()
        {
            Velocity = (transform.position - LastPosition) * (1f / Time.deltaTime);
            LastPosition = transform.position;
        }
        private void LateUpdate()
        {
            Vector2 desiredPosition = DetermineDesiredPosition(Velocity);
            if (Velocity.magnitude < maxSpeedEffect)
            {
                gravityLerp = gravityLerp.LerpTime(1f, gravity);
            }
            else
            {
                gravityLerp = gravityLerp.LerpTime(0f, gravity);
            }
            Vector2 gravityPosition = (Vector2)eyeball.position + (Vector2.down) * size * 0.25f;
            desiredPosition = Vector2.Lerp(desiredPosition, gravityPosition, gravityLerp);

            iris.transform.position = desiredPosition;
        }
    }
}
