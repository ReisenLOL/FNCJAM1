using UnityEngine;

namespace Projectile
{
    public class ProjectileRunner : MonoBehaviour
    {
        static ProjectileRunner instance;
        float nextSecond;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void StartInstance()
        {
            if (instance == null)
            {
                instance = new GameObject("Projectile Runner").AddComponent<ProjectileRunner>();
            }
        }
        private void Update()
        {
            if (instance != this)
                return;
            if (Time.unscaledTime > nextSecond && TimeHandler.TimeScale > 0f)
            {
                nextSecond = Time.unscaledTime + 1f;
                Projectile.RunEverySecond();
            }
            Projectile.RunActiveBullets();
        }
        private void LateUpdate()
        {
            if (instance != this)
                return;
            Projectile.LateRunActiveBullets(1f);
        }
    }
}
