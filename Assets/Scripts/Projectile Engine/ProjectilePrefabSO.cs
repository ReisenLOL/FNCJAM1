using UnityEngine;

namespace Projectile
{
    [CreateAssetMenu(menuName ="Projectile Engine/Prefab SO")]
    public class ProjectilePrefabSO : ScriptableObject
    {
        public static implicit operator Projectile(ProjectilePrefabSO prefab) => prefab.Prefab;
        [field: SerializeField] public Projectile Prefab { get; private set; }
    }
}
