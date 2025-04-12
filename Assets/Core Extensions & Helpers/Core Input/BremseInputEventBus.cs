using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Input
{
    public enum BremseInputPhase
    {
        JustPressed,
        Performed,
        Cancelled
    }
    [CreateAssetMenu(menuName = "Core/Input Event Bus")]
    public class BremseInputEventBus : ScriptableObject
    {
        private delegate void BremseInputEventHandler();
        BremseInputEventHandler OnPerformed;
        BremseInputEventHandler OnCancelled;
        BremseInputEventHandler OnJustPressed;
        private void Awake()
        {

        }
        public void Reinitialize()
        {
            //OnPerformed = null;
            //OnCancelled = null;
            //OnJustPressed = null;
        }
        public void BindAction(BremseInputPhase p, Action actionEvent)
        {
            switch (p)
            {
                case BremseInputPhase.JustPressed:
                    OnJustPressed += actionEvent.Invoke;
                    break;
                case BremseInputPhase.Performed:
                    OnPerformed += actionEvent.Invoke;
                    break;
                case BremseInputPhase.Cancelled:
                    OnCancelled += actionEvent.Invoke;
                    break;
                default:
                    break;
            }
        }
        public void ReleaseAction(BremseInputPhase p, Action actionEvent)
        {
            switch (p)
            {
                case BremseInputPhase.JustPressed:
                    OnJustPressed -= actionEvent.Invoke;
                    break;
                case BremseInputPhase.Performed:
                    OnPerformed -= actionEvent.Invoke;
                    break;
                case BremseInputPhase.Cancelled:
                    OnCancelled -= actionEvent.Invoke;
                    break;
                default:
                    break;
            }
        }
        public void TriggerPerformed(InputAction.CallbackContext _)
        {
            OnPerformed?.Invoke();
        }
        public void TriggerCancelled(InputAction.CallbackContext _)
        {
            OnCancelled?.Invoke();
        }
        public void TriggerJustPressed(InputAction.CallbackContext c)
        {
            if (c.phase == InputActionPhase.Canceled)
                return;
            OnJustPressed?.Invoke();
        }
    }
}
