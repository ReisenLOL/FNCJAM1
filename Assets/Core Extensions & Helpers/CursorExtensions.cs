using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using Core.Input;

namespace Core.Extensions
{
    public static partial class CursorHelper
    {
        public static Vector2 MouseWorldPosition2D => Camera.main == null ? Vector2.zero : (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        public static Vector2 GamepadLookDirection(Vector2 looker)
        {
            return (looker + PlayerInputController.LastRightStickDirectionInput - looker).normalized;
        }
        public static Vector2 GamepadLookDirectionAddPosition(Vector2 position)
        {
            return GamepadLookDirection(position) + position;
        }
        static Cursor cursor = new();
        public static CursorLockMode lockState
        {
            set
            {
                Cursor.lockState = value;

                if (OnLockStateChanged != null)
                {
                    OnLockStateChanged(value);
                }
            }

            get { return Cursor.lockState; }
        }
        public static bool visible
        {
            set
            {
                Cursor.visible = value;

                //Notify Event
                if (OnVisibleChanged != null)
                {
                    OnVisibleChanged(value);
                }
            }

            get { return Cursor.visible; }
        }

        public delegate void CursorLockStateAction(CursorLockMode lockMode);
        public static event CursorLockStateAction OnLockStateChanged;


        public delegate void CursorVisibleAction(bool visible);
        public static event CursorVisibleAction OnVisibleChanged;
    }
}
