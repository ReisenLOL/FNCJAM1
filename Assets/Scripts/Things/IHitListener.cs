using Bremsengine;
using UnityEngine;

public interface IHitListener
{
    public void PerformHit(HitPacket packet);
}
