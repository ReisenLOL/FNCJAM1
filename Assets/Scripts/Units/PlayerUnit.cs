using Core.Extensions;
using UnityEngine;

public class PlayerUnit : BaseUnit
{
    [SerializeField] float Power = 100;
    const float PowerScaler = 0.01f;
    public override float DamageScale(float inputDamage)
    {
        return inputDamage.Multiply((Power * PowerScaler).Max(1f));
    }

    protected override void OnKillEffects()
    {

    }

    protected override void WhenAwake()
    {

    }

    protected override void WhenStart()
    {

    }
}
