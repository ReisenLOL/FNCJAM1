using Bremsengine;
using Core.Extensions;
using System;
using UnityEngine;

#region Damage Scale
public partial class BaseUnit
{
    public abstract float DamageScale(float inputDamage);
}
#endregion
#region Hit & Damage
public partial class BaseUnit : IHitListener
{
    public delegate void HitEvent(HitPacket packet, BaseUnit unit);
    public event HitEvent WhenHit;
    public float defenseModifier = 1f; //i dunno where else to put this! - sylvia
    public void PerformHit(HitPacket packet)
    {
        if (!IsAlive)
            return;
        ChangeHealth(-packet.Damage * defenseModifier);
        WhenHit?.Invoke(packet, this);
    }
    public void BindHitEvent(HitEvent hitAction)
    {
        WhenHit += hitAction;
    }
    public void ReleaseHitEvent(HitEvent hitAction)
    {
        WhenHit -= hitAction;
    }
    public void ChangeHealth(float value) //i hope changing this to public wont destroy the game - sylvia
    {
        CurrentHealth += value;
        CurrentHealth = CurrentHealth.Max(0f);
        if (CurrentHealth <= 0)
        {
            Kill();
        }
    }
    protected abstract void OnKillEffects();
    private void Kill()
    {
        gameObject.SetActive(false);
        OnKillEffects();
        //drop loot or something lmao or put it in the onkilleffects lmao
    }
    public void ExternalKill()
    {
        CurrentHealth = 0f;
        gameObject.SetActive(false);
    }
}
#endregion
#region Faction Lmao
public partial class BaseUnit : IFaction
{
    public bool IsFriendlyWith(BremseFaction Faction) => FactionInterface.CompareFaction(Faction);
    public BremseFaction Faction => FactionInterface.Faction;
    protected IFaction FactionInterface => (IFaction)this;
    BremseFaction IFaction.Faction { get; set; }
}
#endregion
#region Stall
public partial class BaseUnit
{
    float stallEndTime;
    public void SetStallTime(float time)
    {
        stallEndTime = time + Time.time;
    }
    public bool IsStalled => Time.time < stallEndTime;
}
#endregion
public abstract partial class BaseUnit : MonoBehaviour
{
    public static BaseUnit Player { get; private set; }
    public bool IsAlive => CurrentHealth > 0f && gameObject.activeInHierarchy;
    [SerializeField] float startingHealth = 100f;
    public float CurrentHealth { get; protected set; }
    public float MaxHealth = 100f; //setting this to its own variable - sylvia

    [SerializeField] Transform centerPositionOverride;
    protected Vector2 Origin;
    public Vector2 CurrentPosition => centerPositionOverride == null ? transform.position : centerPositionOverride.position;
    protected abstract void WhenStart();
    private void Start()
    {
        WhenStart();
    }
    protected abstract void WhenDestroy();
    private void OnDestroy()
    {
        WhenDestroy();
    }
    private void Awake()
    {
        CurrentHealth = startingHealth;
        Origin = CurrentPosition;
        if (this is PlayerUnit p)
        {
            FactionInterface.SetFaction(BremseFaction.Player);
            Player = p;
        }
        else
        {
            FactionInterface.SetFaction(BremseFaction.Enemy);
        }
        WhenAwake();
    }
    protected abstract void WhenAwake();
}
