using Core.Extensions;
using Core.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bremsengine
{
    public class Rigidbody2DAnimator : MonoBehaviour
    {
        [SerializeField] float expectedSpeed = 5f;
        [SerializeField] Rigidbody2D rb;
        [SerializeField] Animator animator;
        [SerializeField] string animatorExpectedSpeedKey = "MOVESPEED";
        [SerializeField] string animatorVelocityKey = "VELOCITY";

        [SerializeField] List<Transform> horizontalFlipAnchors = new List<Transform>();
        float lastSignedDirection;
        [SerializeField] float minimumFlipSpeed = 2f;
        [SerializeField] bool animatorLockFlip;
        float lastFlipTime;
        private void LateUpdate()
        {
            if (rb == null)
                return;

            if (animator != null)
            {
                animator.SetFloat(animatorExpectedSpeedKey, expectedSpeed);
                animator.SetFloat(animatorVelocityKey, rb.linearVelocity.magnitude);
            }

            if (!animatorLockFlip && rb.linearVelocity.x != 0f && rb.linearVelocity.magnitude > minimumFlipSpeed && Time.time > lastFlipTime + 0.15f)
            {
                SetLookX(rb.linearVelocity.x);
            }
        }
        private void SetLookX(float x)
        {
            float signedDirection = x.Sign();

            if (lastSignedDirection != signedDirection)
            {
                if (signedDirection >= 0)
                {
                    FlipRight();
                }
                else
                {
                    FlipLeft();
                }
            }
            lastSignedDirection = signedDirection;
        }
        public void ForceLook(Vector2 position)
        {
            Vector2 direction = position - rb.position;
            SetLookX(direction.x);
        }
        public void LookTowardsCursor()
        {
            switch (PlayerInputController.ControlScheme)
            {
                case ControlScheme.None:
                    break;
                case ControlScheme.PC:
                    break;
                case ControlScheme.Gamepad:
                    ForceLook(CursorHelper.GamepadLookDirectionAddPosition(rb.position));
                    return;
                default:
                    break;
            }
            ForceLook(CursorHelper.MouseWorldPosition2D);
        }
        private void FlipRight()
        {
            foreach (var item in horizontalFlipAnchors)
            {
                item.localScale = new(1f, 1f, 1f);
            }
        }
        private void FlipLeft()
        {
            foreach (var item in horizontalFlipAnchors)
            {
                item.localScale = new(-1f, 1f, 1f);
            }
        }
    }
}
