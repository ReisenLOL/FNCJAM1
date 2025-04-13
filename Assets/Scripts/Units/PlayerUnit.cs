using Core.Extensions;
using UnityEngine;

public class PlayerUnit : BaseUnit
{
    [SerializeField] float Power = 100;
    public static PlayerUnit PlayerAsPlayerUnit => (PlayerUnit)BaseUnit.Player;
    const float PowerScaler = 0.01f;
    private void ShowDamageText(HitPacket packet, BaseUnit unit)
    {
        TextPopupManager.PlayerDamageIncoming(packet.HitPosition, packet.Damage.ToInt());
    }
    public override float DamageScale(float inputDamage)
    {
        return inputDamage.Multiply((Power * PowerScaler).Max(1f));
    }

    protected override void OnKillEffects()
    {

    }
    protected override void WhenDestroy()
    {
        WhenHit -= ShowDamageText;
    }

    protected override void WhenAwake()
    {

    }

    protected override void WhenStart()
    {
        WhenHit += ShowDamageText;
    }
}
