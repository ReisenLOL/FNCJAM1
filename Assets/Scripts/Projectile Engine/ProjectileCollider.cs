using Bremsengine;
using Core.Extensions;
using UnityEngine;

namespace Projectile
{
    public class ProjectileCollider : MonoBehaviour
    {
        Projectile assignedProjectile;
        public void SetProjectile(Projectile p)
        {
            assignedProjectile = p;
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            Projectile.TryTriggerOnScreenExit(assignedProjectile, collision);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Projectile.CollisionResult result = GetCollisionResult(collision);
            Debug.Log(result);
            assignedProjectile.PerformCollisionResult(result, collision);
        }
        public Projectile.CollisionResult GetCollisionResult(Collider2D other)
        {
            if (other == null)
            {
                Debug.LogError("Bad");
                return Projectile.CollisionResult.Error;
            }
            if (other.GetComponent<IFaction>() is IFaction hitListener and not null)
            {
                if (hitListener.Faction != BremseFaction.None && hitListener.CompareFaction(assignedProjectile.Faction))
                {
                    Debug.Log("T");
                    return Projectile.CollisionResult.Friends;
                }
                else
                {
                    return Projectile.CollisionResult.Hit;
                }
            }
            return Projectile.CollisionResult.DefaultObject;
        }
    }
}
