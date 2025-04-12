using UnityEngine;

namespace Projectile
{
    public class ProjectileEngineTester : MonoBehaviour
    {
        float nextTriggerTime = 0f;
        [SerializeField] AttackHandler handler;
        private void Update()
        {
            if (Time.time > nextTriggerTime)
            {
                nextTriggerTime = Time.time + 0.05f;
                TriggerNext();
            }
        }
        void TriggerNext()
        {
            if (handler.ResolveTarget(out BaseUnit target))
            {
                handler.TryAttack(target.CurrentPosition);
            }
        }
    }
}
