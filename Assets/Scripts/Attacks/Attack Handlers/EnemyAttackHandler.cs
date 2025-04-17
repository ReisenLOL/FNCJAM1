using UnityEngine;

namespace Projectile
{
    #region Target Resolver
    public partial class EnemyAttackHandler
    {
        public override bool ResolveTarget(out BaseUnit target)
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
    public partial class EnemyAttackHandler : AttackHandler
    {
        protected override void WhenDestroy()
        {

        }

        protected override void WhenStart()
        {

        }
        private void Update()
        {
            if (ResolveTarget(out BaseUnit target))
            {
                TryAttack(target.CurrentPosition);
            }
        }
    }
}
