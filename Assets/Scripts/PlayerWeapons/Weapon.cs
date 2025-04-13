using Bremsengine;
using Core.Extensions;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected BaseUnit Owner;
    public float speed;
    public float damage;
    public float maxRange;
    public GameObject firedFrom;
    public Vector2 targetPosition;
    public void SetOwner(BaseUnit owner) => this.Owner = owner;
    public void RotateToTarget(Vector2 worldPosition)
    {
        targetPosition = worldPosition;
        transform.Lookat2D(worldPosition);
    }
    public bool TryHitOther(HitPacket packet, Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out IHitListener hit))
        {
            if ((BaseUnit)hit is BaseUnit unit)
            {
                if (unit.IsFriendlyWith(Owner.Faction))
                    return false;
            }
            hit.PerformHit(packet);
            return true;
        }
        return false;
    }
}
