using Bremsengine;
using Core.Extensions;
using Core.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

namespace Projectile
{
    #region Target Resolver
    public partial class PlayerAttackHandler
    {
        Vector2 storedAttackDirection;
        public override bool ResolveTarget(out BaseUnit target)
        {
            target = null;
            if (EnemyUnit.AutoAimTowardEnemy(Owner.CurrentPosition, storedAttackDirection, 45f, out EnemyUnit enemy))
            {
                target = enemy;
            }
            return target != null;
        }
    }
    #endregion
    #region Click Input
    public partial class PlayerAttackHandler
    {
        float nextClickCheckTime;
        bool isClickPressed;
        private void ClickLoop()
        {
            Vector2 pos = Owner.CurrentPosition;
            if (PlayerInputController.RightStickDirection != Vector2.zero)
            {
                storedAttackDirection = PlayerInputController.RightStickDirection;
            }
            else
            {
                storedAttackDirection = RenderTextureCursorHandler.CursorWorldPosition - pos;
            }
            if (ResolveTarget(out BaseUnit target))
            {
                pos = target.CurrentPosition;
                ClickAttack(pos);
            }
            else
            {
                ClickAttack(pos + storedAttackDirection);
            }
        }
        private void ClickAttack(Vector2 worldPosition)
        {
            if (Time.time < nextClickCheckTime)
            {
                return;
            }
            if (isClickPressed || PlayerInputController.RightStickDirection != Vector2.zero)
            {
                nextClickCheckTime = Time.time + 0.1f;
                Debug.DrawLine(Owner.CurrentPosition, worldPosition, ColorHelper.DeepBlue, 1f);
                if (worldLight != null) worldLight.intensity = 1.5f;
                TryAttack(worldPosition);
            }
        }
        private void PressFire()
        {
            isClickPressed = true;
            ClickLoop();
        }
        private void ReleaseFire()
        {
            isClickPressed = false;
        }
        private void ReadInput(InputAction.CallbackContext ctx)
        {
            switch (ctx.phase)
            {
                case InputActionPhase.Disabled:
                    break;
                case InputActionPhase.Waiting:
                    break;
                case InputActionPhase.Started:
                    break;
                case InputActionPhase.Performed:
                    PressFire();
                    break;
                case InputActionPhase.Canceled:
                    ReleaseFire();
                    break;
                default:
                    break;
            }
        }
    }
    #endregion
    #region Light
    public partial class PlayerAttackHandler
    {
        [SerializeField] Light2D worldLight;
        private float _time = 1.5f;
        private bool changeLight => worldLight != null && worldLight.intensity > 0.05f;
        private void LightTick(int tick, float deltaTime)
        {
            if (changeLight)
            {
                worldLight.intensity -= _time * deltaTime;
                worldLight.intensity = worldLight.intensity.Max(0.05f);
            }
        }
    }
    #endregion
    public partial class PlayerAttackHandler : AttackHandler
    {
        [SerializeField] InputActionReference AttackInput;
        protected override void WhenDestroy()
        {
            TickManager.MainTick -= LightTick;
        }
        protected override void WhenStart()
        {
            TickManager.MainTick += LightTick;
        }
        private void Update()
        {
            ClickLoop();
        }
    }
}
