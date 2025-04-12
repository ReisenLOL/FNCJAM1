using Core.Extensions;
using UnityEngine;

namespace Projectile
{
    public class DemoAttack : BaseAttack
    {
        [SerializeField] Projectile prefab;
        protected override void AttackPayload(Projectile.InputSettings input)
        {
            Arc(-45f, 45, 90f / 9f, 0.1f).Spawn(input, prefab, out iterationList);
            foreach (var iteration in iterationList)
            {
                iteration.AddMod(new ProjectileModAccelerate(new(1f, 0f), 15f, 15f));
                iteration.AddMod(new ProjectileModRotate(new(0.4f, 0.01f), 90f, 225f));
                iteration.AddMod(new ProjectileModRotate(new(0.8f, 0.4f), -90f, 225f));
                iteration.AddMod(new ProjectileModRotate(new(0.4f, 1.2f), 90f, 225f));
            }
        }
    }
}