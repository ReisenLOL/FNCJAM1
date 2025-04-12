using UnityEngine;
using Core.Extensions;
namespace Projectile
{
    public abstract class ProjectileMod
    {
        bool wasActive;
        float tickStartTime;
        float tickEndTime;
        public void ApplySettings(modSettings settings)
        {
            wasActive = false;
            tickStartTime = Time.time + settings.delay;
            tickEndTime = Time.time + settings.delay + settings.duration;
        }
        public struct modSettings
        {
            public float delay;
            public float duration;
            public modSettings(float duration, float delay)
            {
                this.delay = delay.Max(0.02f);
                this.duration = duration.Max(0.02f);
            }
        }
        public void TickEvent(Projectile eventProjectile, float deltaTime)
        {
            if (Time.time.IsBetween(tickStartTime, tickEndTime))
            {
                if (!wasActive)
                {
                    OnFirstRunPayload(eventProjectile);
                    wasActive = true;
                }
                RunPayload(eventProjectile, deltaTime);
            }
        }
        protected abstract void OnFirstRunPayload(Projectile eventProjectile);
        protected abstract void RunPayload(Projectile eventProjectile, float deltaTime);
    }
}
