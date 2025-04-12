using Bremsengine;
using TMPro;
using UnityEngine;

namespace Projectile
{
    public class ProjectileCountDisplay : MonoBehaviour
    {
        [SerializeField] TMP_Text bulletCountText;
        private void OnEnable()
        {
            TickManager.MainTickLightweight += RefreshUI;
        }
        private void OnDisable()
        {
            TickManager.MainTickLightweight -= RefreshUI;
        }
        public void RefreshUI()
        {
            if (bulletCountText == null)
                return;
            bulletCountText.text = string.Intern("Bullet Count: ") + Projectile.BulletCount.ToString();
        }
    }
}
