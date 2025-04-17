using Core.Extensions;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Projectile
{
    #region Target Resolver
    public partial class AttackHandler
    {
        public virtual bool ResolveTarget(out BaseUnit target)
        {
            target = null;
            if (BaseUnit.Player != null && BaseUnit.Player != Owner)
            {
                target = BaseUnit.Player;
            }
            return target != null;
        }
    }
    #endregion
    [DefaultExecutionOrder(-1)]
    public abstract partial class AttackHandler : MonoBehaviour
    {
        #region Attack Time Settings
        [System.Serializable]
        public struct AttackTimeSettings
        {
            public float SwingDuration;
            public float UnitStallDuration;
            public AttackTimeSettings(float swingDuration, float unitStallDuration)
            {
                this.SwingDuration = swingDuration;
                this.UnitStallDuration = unitStallDuration;
            }
        }
        #endregion
        public void ApplyTimeSettings(AttackTimeSettings settings)
        {
            stallTime = settings.UnitStallDuration + Time.time;
            NextAttackTime = Time.time + settings.SwingDuration;
            Owner.SetStallTime(settings.UnitStallDuration);
        }
        float NextAttackTime;
        float stallTime;
        public bool IsStalled() => Time.time < stallTime;
        public bool IsAttackTimeAllowed() => Time.time >= NextAttackTime;
        [field: SerializeField] public BaseUnit Owner { get; private set; }
        public delegate void AttackAction(Vector2 target);
        public AttackAction OnAttack;
        public void SetOwner(BaseUnit owner)
        {
            this.Owner = owner;
        }
        private void WhenScreenEnter()
        {
            if (IsAttackTimeAllowed())
            {
                NextAttackTime = Time.time + 0.5f;
            }
        }
        private void Start()
        {
            WhenStart();
        }
        protected abstract void WhenStart();
        private void OnDestroy()
        {
            WhenDestroy();
        }
        protected abstract void WhenDestroy();
        public bool TryAttack(Vector2 worldPosition)
        {
            if (!IsAttackTimeAllowed())
            {
                return false;
            }
            OnAttack?.Invoke(worldPosition);
            return true;
        }
    }
}
