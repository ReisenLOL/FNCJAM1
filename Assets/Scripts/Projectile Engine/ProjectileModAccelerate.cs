using Core.Extensions;
using UnityEngine;

namespace Projectile
{
    public class ProjectileModAccelerate : ProjectileMod
    {
        float acceleration;
        float targetSpeed;
        public ProjectileModAccelerate(modSettings settings, float speed, float acceleration)
        {
            ApplySettings(settings);
            this.targetSpeed = speed;
            this.acceleration = acceleration;
        }
        protected override void OnFirstRunPayload(Projectile eventProjectile)
        {

        }
        protected override void RunPayload(Projectile eventProjectile, float deltaTime)
        {
            float currentSpeed = eventProjectile.CurrentVelocity.magnitude;
            currentSpeed = currentSpeed.MoveTowards(targetSpeed, deltaTime * acceleration);

            eventProjectile.Action_SetVelocity(eventProjectile.CurrentVelocity, currentSpeed);
        }
    }
}
