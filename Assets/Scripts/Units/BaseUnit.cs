using Bremsengine;
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
    public void PerformHit(HitPacket packet)
    {
        ChangeHealth(-packet.Damage);
    }
    private void ChangeHealth(float value)
    {
        CurrentHealth = CurrentHealth <= value ? 0f : CurrentHealth - value;
        if (IsAlive)
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
    public BremseFaction Faction => FactionInterface.Faction;
    protected IFaction FactionInterface => (IFaction)this;
    BremseFaction IFaction.Faction { get; set; }
}
#endregion
public abstract partial class BaseUnit : MonoBehaviour
{
    public static BaseUnit Player { get; private set; }
    public bool IsAlive => CurrentHealth > 0f && gameObject.activeInHierarchy;
    [SerializeField] float startingHealth = 100f;
    protected float CurrentHealth;
    [SerializeField] Transform centerPositionOverride;
    protected Vector2 Origin;
    public Vector2 CurrentPosition => centerPositionOverride == null ? transform.position : centerPositionOverride.position;
    protected abstract void WhenStart();
    private void Start()
    {
        WhenStart();
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
