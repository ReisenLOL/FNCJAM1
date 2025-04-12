using Core.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Input;

namespace Bremsengine
{
    public class RotateTowardsCursor2D : MonoBehaviour
    {
        [SerializeField] public bool LockRotation;
        [SerializeField] Transform rotationAnchor;
        [Header("Sprite Sorting")]
        [SerializeField] Transform SpriteListSocket;
        [SerializeField] float frontLayerValue = 2000f;
        [SerializeField] float backLayerValue = -2000f;
        [SerializeField] Vector2 backFacingRange = new(30f, 150f);

        public void CalculateBackface()
        {
            if (SpriteListSocket.GetComponentsInChildren<SpriteRenderer>() is SpriteRenderer[] list && list.Length > 0)
            {
                int currentLayer = (int)frontLayerValue;
                int iterationLayer;

                if (rotationAnchor.localEulerAngles.z.IsBetween(backFacingRange))
                {
                    currentLayer = (int)backLayerValue;
                }

                for (int i = 0; i < list.Length; i++)
                {
                    iterationLayer = i + currentLayer;
                    list[i].sortingOrder = iterationLayer;
                }
            }
        }
        Vector2 lastLookPosition;
        public Vector2 LookPosition => CalculateLookPosition();
        private Vector2 CalculateLookPosition()
        {
            switch (PlayerInputController.ControlScheme)
            {
                case ControlScheme.None:
                    break;
                case ControlScheme.PC:
                    break;
                case ControlScheme.Gamepad:
                    return CursorHelper.GamepadLookDirectionAddPosition(rotationAnchor.position);
                default:
                    break;
            }
            Vector2 mouse = CursorHelper.MouseWorldPosition2D;
            return mouse;
        }
        private void Update()
        {
            if (rotationAnchor == null)
            {
                return;
            }
            lastLookPosition = LookPosition;
            if (LockRotation)
            {
                return;
            }
            rotationAnchor.Lookat2D(LookPosition);
        }
        private void FixedUpdate()
        {
            CalculateBackface();
        }
    }
}
