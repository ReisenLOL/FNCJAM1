using Bremsengine;
using Core.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile
{
    #region Shortcuts
    public partial class BaseAttack
    {
        protected static bool IsDifficulty(GeneralManager.Difficulty d) => Difficulty == d;
        protected Projectile.ArcSettings Arc(float startAngle, float endAngle, float angleInterval, float projectileSpeed) => new(startAngle, endAngle, angleInterval, projectileSpeed);
        protected Projectile.crawlerPacket CrawlerPacket(float delay, float aimAngle, float repeatAngle, int repeatCount, float repeatTimeInterval) => new(delay, aimAngle, repeatAngle, repeatCount, repeatTimeInterval);
        protected Projectile.SingleSettings Single(float addedAngle, float ProjectileSpeed) => new(addedAngle, ProjectileSpeed);
        protected static GeneralManager.Difficulty Difficulty => GeneralManager.CurrentDifficulty;
        protected List<Projectile> iterationList;
        protected Vector2 Down => (owner.CurrentPosition + Vector2.down) - owner.CurrentPosition;
        protected Vector2 Up => (owner.CurrentPosition + Vector2.up) - owner.CurrentPosition;
    }
    #endregion
    public abstract partial class BaseAttack : MonoBehaviour
    {
        [field: SerializeField] protected BaseUnit owner { get; private set; }
        [SerializeField] protected AttackHandler handler;
        [SerializeField] protected AttackHandler.AttackTimeSettings swingTimeSettings = new(1f, 0f);
        [SerializeField] BremseFaction noUnitFallbackFaction = BremseFaction.None;
        [SerializeField] protected float fallbackDamage = 5f;
        [SerializeField] int projectileLayerIndex = 100;
        [Header("Optional")]
        [SerializeField] protected ACWrapper attackSound;
        public void TriggerAttackLoad()
        {
            OnAttackLoad();
        }
        protected virtual void OnAttackLoad()
        {

        }
        private void PerformContainedAttack(Vector2 target)
        {
            if (gameObject.activeInHierarchy)
            {
                PerformContainedAttack(target, false);
            }
        }
        private void PerformContainedAttack(Vector2 target, bool bypassAliveCheck = false)
        {
            if (gameObject == null || (!gameObject.activeInHierarchy && !bypassAliveCheck))
            {
                return;
            }
            if (handler != null)
            {
                if (!handler.IsAttackTimeAllowed())
                {
                    return;
                }
                handler.ApplyTimeSettings(swingTimeSettings);
            }
            Projectile.InputSettings s = new(owner == null ? transform.position : owner.CurrentPosition, target - (owner == null ? transform.position : (Vector2)owner.CurrentPosition));
            if (handler.ResolveTarget(out BaseUnit unit))
            {
                s.AssignTarget(unit.transform);
            }
            s.OnSpawn += ProjectileSpawnCallback;
            attackSound.Play(transform.position);
            AttackPayload(s);
        }
        public void ExternalForcedAttack()
        {
            ExternalForcedAttack(Vector2.down);
        }
        public void ExternalForcedAttack(Vector2 direction)
        {
            PerformContainedAttack(direction, true);
        }
        protected abstract void AttackPayload(Projectile.InputSettings input);
        private void ProjectileSpawnCallback(Projectile p)
        {
            if (owner != null)
            {
                p.Action_SetFaction(owner.Faction);
                p.Action_SetDamage(owner.DamageScale(fallbackDamage));
                p.Action_SetOwnerReference(owner);
            }
            else
            {
                p.Action_SetFaction(noUnitFallbackFaction);
                p.Action_SetDamage(fallbackDamage);
                p.Action_SetOwnerReference(null);
            }
            p.Action_SetSpriteLayerIndex(projectileLayerIndex);
        }
        public void SetHandler(AttackHandler handler)
        {
            this.handler = handler;
            handler.OnAttack = null;
            handler.OnAttack += PerformContainedAttack;
            TriggerAttackLoad();
        }
        public void ClearHandler()
        {
            if (handler != null)
            {
                handler.OnAttack -= PerformContainedAttack;
            }
            handler = null;
        }
        public void SetOwner(BaseUnit unit)
        {
            owner = unit;
        }
        private void Start()
        {
            TriggerAttackLoad();
            WhenStart();
            if (handler != null) handler.OnAttack = PerformContainedAttack;
        }
        protected virtual void WhenStart()
        {

        }
        private void OnDestroy()
        {
            if (handler != null) handler.OnAttack -= PerformContainedAttack;
        }
    }
}
