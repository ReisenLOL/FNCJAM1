using Core.Extensions;
using UnityEngine;

namespace Bremsengine
{
    public class UnitDamageScaler : MonoBehaviour
    {
        #region Boring stuff
        private void ResetOnAwake()
        {
            storedDamageScale = -1f;
        }
        private float buildPower => storedDamageScale <= 0f ? storedDamageScale = (power.Max(100f) * 0.01f).Max(1f) : storedDamageScale;
        float storedDamageScale = -1f;
        #endregion
        [SerializeField] private float power = 100f;
        public float ExternalDamageScale = 1f;
        public float DamageScale => buildPower * ExternalDamageScale;
        public void AddPower(float power) => this.power += power;
        private void Awake()
        {
            ResetOnAwake();
        }
        public float Rebuild()
        {
            storedDamageScale = -1f;
            return buildPower;
        }
    }
}
