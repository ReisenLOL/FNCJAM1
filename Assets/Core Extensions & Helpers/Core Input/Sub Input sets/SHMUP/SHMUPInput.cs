using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;

namespace Core.Input
{
    public class SHMUPInput : MonoBehaviour
    {
        [SerializeField] BremseInputEventBus Focus;
        [SerializeField] BremseInputEventBus Shoot;
        [SerializeField] BremseInputEventBus AutoFire;
        [SerializeField] BremseInputEventBus Berserk;
        [SerializeField] BremseInputEventBus Bomb;
        [SerializeField] BremseInputEventBus WeaponNext;
        [SerializeField] BremseInputEventBus WeaponPrevious;
        private void BindAction(InputAction action, BremseInputEventBus bus)
        {
            action.performed += bus.TriggerPerformed;
            action.started += bus.TriggerJustPressed;
            action.canceled += bus.TriggerCancelled;
        }
        private void ReleaseAction(InputAction action, BremseInputEventBus bus)
        {
            action.performed -= bus.TriggerPerformed;
            action.started -= bus.TriggerJustPressed;
            action.canceled -= bus.TriggerCancelled;
        }
        private void Awake()
        {
            GameActions.ShmupActions s = PlayerInputController.actions.Shmup;
            BindAction(s.Autofire, AutoFire);
            BindAction(s.Fire, Shoot);
            BindAction(s.Focus, Focus);
            BindAction(s.Ability1, Berserk);
            BindAction(s.Bomb, Bomb);
            BindAction(s.WeaponNext, WeaponPrevious);
            BindAction(s.WeaponPrevious, WeaponNext);

            Focus.Reinitialize();
            Shoot.Reinitialize();
            AutoFire.Reinitialize();
            Berserk.Reinitialize();
            WeaponPrevious.Reinitialize();
            WeaponNext.Reinitialize();
            Bomb.Reinitialize();
        }
        private void OnDestroy()
        {
            GameActions.ShmupActions s = PlayerInputController.actions.Shmup;
            ReleaseAction(s.Autofire, AutoFire);
            ReleaseAction(s.Fire, Shoot);
            ReleaseAction(s.Focus, Focus);
            ReleaseAction(s.Ability1, Berserk);
            ReleaseAction(s.Bomb, Bomb);
            ReleaseAction(s.WeaponNext, WeaponPrevious);
            ReleaseAction(s.WeaponPrevious, WeaponNext);
        }
    }
}
