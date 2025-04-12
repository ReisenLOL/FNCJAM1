using Core.Extensions;
using UnityEngine;

namespace Projectile
{
    public class DemoSimpleLineAttack : BaseAttack
    {
        [SerializeField] Projectile prefab;
        [SerializeField] Projectile prefab2;
        protected override void AttackPayload(Projectile.InputSettings input)
        {
            Single(0f, 25f).Spawn(input, prefab, out iterationList);
            Arc(-35f, 35f, 70f / 9f, 8f).Spawn(input, prefab2, out iterationList);
            foreach (var iteration in iterationList)
            {
                iteration.AddOnScreenExitEvent(BounceProjectile);
            }
        }
        private void BounceProjectile(Projectile p, Vector2 normal)
        {
            p.Action_SetVelocity(p.CurrentVelocity.Bounce(normal, 1f), p.CurrentSpeed);
        }
    }
}
