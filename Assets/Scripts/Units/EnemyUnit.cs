using Bremsengine;
using System.Collections.Generic;
using UnityEngine;
using Core.Extensions;
using Unity.VisualScripting;

#region Alive Enemies & Auto Aim
public partial class EnemyUnit
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeEnemyList()
    {
        AliveEnemies = new();
        GeneralManager.SetStageLoadAction("Reset Alive Enemies", ResetAliveEnemies);
    }
    private static void ResetAliveEnemies()
    {
        AliveEnemies.Clear();
    }
    public static List<EnemyUnit> AliveEnemies;
    public void RecalculateAliveEnemy(HitPacket hit, BaseUnit unit)
    {
        if (CurrentHealth > 0f && !AliveEnemies.Contains(this))
        {
            AliveEnemies.Add(this);
            return;
        }
        if (CurrentHealth <= 0f)
        {
            AliveEnemies.Remove(this);
        }
    }
    public static bool AutoAimTowardEnemy(Vector2 origin, Vector2 direction, float maxAngle, out EnemyUnit selection)
    {
        Vector2 iterationDirection;
        float highestDot = -999f;
        selection = null;
        float iterationDot;
        foreach (var item in AliveEnemies)
        {
            iterationDirection = item.CurrentPosition - origin;
            if (iterationDirection.Angle(direction).Absolute() > maxAngle)
            {
                continue;
            }
            iterationDot = Vector2.Dot(iterationDirection.normalized, direction);
            if (iterationDot >= highestDot)
            {
                highestDot = iterationDot;
                selection = item;
            }
        }
        return selection != null;
    }
    public static bool TryGetRandomAliveEnemy(out EnemyUnit selection)
    {
        selection = null;
        if (AliveEnemies.Count == 0) { return selection = null; }

        selection = AliveEnemies[0.RandomBetween(0, AliveEnemies.Count)];
        return selection != null;
    }
}
#endregion
#region Enemy Circle Cast
public partial class EnemyUnit
{
    public static bool TryFindInCircleCast(Vector2 position, float radius, LayerMask mask, out HashSet<EnemyUnit> result)
    {
        if (Helper.TryFindInCircleCast<EnemyUnit>(position, radius, mask, out result))
        {
            return true;
        }
        return false;
    }
}
#endregion
public partial class EnemyUnit : BaseUnit
{
    private void ShowDamageText(HitPacket packet, BaseUnit unit)
    {
        TextPopupManager.PlayerDamageOutwards(packet.HitPosition, packet.Damage.ToInt());
    }
    [SerializeField] bool funnyexplosion = false;
    public override float DamageScale(float inputDamage)
    {
        return inputDamage;
    }
    protected override void OnKillEffects()
    {
        //i dunno where to put this line of code at so....
        GameObject.Find("SpawnManager").GetComponent<SpawnManager>().currentKills++;
        if (funnyexplosion) GeneralManager.FunnyExplosion(CurrentPosition, 1f);
    }

    protected override void WhenAwake()
    {

    }
    protected override void WhenStart()
    {
        WhenHit += RecalculateAliveEnemy;
        WhenHit += ShowDamageText;
    }
    protected override void WhenDestroy()
    {
        WhenHit -= RecalculateAliveEnemy;
        WhenHit -= ShowDamageText;
    }
}
