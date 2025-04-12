using Core.Extensions;
using UnityEngine;

namespace Projectile
{
    public class ProjectileModRotate : ProjectileMod
    {
        public float CurrentRotationPerSecond = 0f;
        public float RotationTargetValue;
        public float Acceleration = 2f;

        public ProjectileModRotate(modSettings settings, float rotationTarget, float acceleration)
        {
            ApplySettings(settings);
            RotationTargetValue = rotationTarget;
            this.Acceleration = acceleration;
        }

        protected override void OnFirstRunPayload(Projectile eventProjectile)
        {

        }

        protected override void RunPayload(Projectile eventProjectile, float deltaTime)
        {
            CurrentRotationPerSecond = CurrentRotationPerSecond.MoveTowards(RotationTargetValue, Acceleration);
            eventProjectile.Action_AddRotation(CurrentRotationPerSecond * deltaTime);
        }
    }
}
