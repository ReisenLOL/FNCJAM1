using UnityEngine;

public struct HitPacket
{
    public HitPacket(Vector2 HitPosition, float Damage)
    {
        this.HitPosition = HitPosition;
        this.Damage = Damage;
    }
    public Vector2 HitPosition;
    public float Damage;
}
