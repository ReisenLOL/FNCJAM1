using Bremsengine;
using Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

namespace Projectile
{
    #region Projectile Mods
    public partial class Projectile
    {
        List<ProjectileMod> knownMods;
        public Projectile AddMod(ProjectileMod e)
        {
            knownMods.Add(e);
            return this;
        }
        public Projectile RemoveMod(ProjectileMod e)
        {
            knownMods.Remove(e);
            return this;
        }
        public Projectile ClearMods()
        {
            knownMods.Clear(); return this;
        }
        private void RunEvents(float deltaTime)
        {
            for (int i = 0; i < knownMods.Count; i++)
            {
                if (knownMods[i] == null)
                {
                    knownMods.RemoveAt(i);
                    i--;
                    continue;
                }
                knownMods[i].TickEvent(this, deltaTime);
            }
        }
    }
    #endregion
    #region Attack Types
    public partial class Projectile
    {
        public struct InputSettings
        {
            public InputSettings(Vector2 origin, Vector2 direction)
            {
                this.Origin = origin;
                this.Direction = direction;
                this.OnSpawn = null;
                this.OptionalTarget = null;
            }
            public InputSettings Copy()
            {
                return new()
                {
                    Origin = this.Origin,
                    Direction = this.Direction,
                    OnSpawn = this.OnSpawn,
                    OptionalTarget = this.OptionalTarget
                };
            }
            public InputSettings SetOrigin(Vector2 position)
            {
                Origin = position;
                return this;
            }
            public InputSettings SetDirection(Vector2 direction)
            {
                Direction = direction;
                return this;
            }
            public InputSettings AssignTarget(Transform target)
            {
                OptionalTarget = target;
                return this;
            }
            public Vector2 Origin { get; private set; }
            public Vector2 Direction { get; private set; }
            public ProjectileSpawnAction OnSpawn;
            public Transform OptionalTarget { get; private set; }
        }
        public struct SingleSettings
        {
            public SingleSettings(float addedAngle, float projectileSpeed)
            {
                this.AddedAngle = addedAngle;
                this.ProjectileSpeed = projectileSpeed;
            }
            public float AddedAngle;
            public float ProjectileSpeed;
            public bool Spawn(Projectile.InputSettings input, Projectile prefab, out List<Projectile> output)
            {
                return SpawnSingle(prefab, input, this, out output);
            }
        }
        public struct ArcSettings
        {
            public static ArcSettings operator *(ArcSettings settings, float multiplier)
            {
                return new ArcSettings(
                    settings.StartingAngle,
                    settings.EndingAngle,
                    settings.ArcInterval / multiplier,
                    settings.ProjectileSpeed
                );
            }
            public ArcSettings Widen(float multiplier)
            {
                return new ArcSettings(
                    this.StartingAngle * multiplier,
                    this.EndingAngle * multiplier,
                    this.ArcInterval * multiplier,
                    this.ProjectileSpeed
                    );
            }
            public ArcSettings Speed(float multiplier)
            {
                return new ArcSettings(
                   this.StartingAngle,
                   this.EndingAngle,
                   this.ArcInterval,
                   this.ProjectileSpeed * multiplier);
            }
            public ArcSettings(float startingAngle, float arcAngle, float arcInterval, float projectileSpeed)
            {
                this.StartingAngle = startingAngle;
                this.EndingAngle = arcAngle;
                this.ArcInterval = arcInterval;
                this.ProjectileSpeed = projectileSpeed;
            }
            public bool Spawn(Projectile.InputSettings input, Projectile prefab, out List<Projectile> output)
            {
                return SpawnArc(prefab, input, this, out output);
            }
            public float StartingAngle { get; private set; }
            public float EndingAngle { get; private set; }
            public float ArcInterval { get; private set; }
            public float ProjectileSpeed { get; private set; }
        }
        public static bool SpawnSingle(Projectile prefab, InputSettings input, SingleSettings settings, out List<Projectile> output)
        {
            output = new();
            bool spawnedBullet = CreateBullet(prefab, input.Origin, input.Direction.normalized.Rotate2D(settings.AddedAngle), input.OnSpawn, settings.ProjectileSpeed, out Projectile p);
            if (p != null)
            {
                p.Action_AddPosition(p.CurrentVelocity.ScaleToMagnitude(0.25f));
                output.Add(p);
            }
            return spawnedBullet;
        }
        public static bool SpawnArc(Projectile prefab, InputSettings input, ArcSettings settings, out List<Projectile> output)
        {
            output = new();
            foreach (var item in settings.ArcInterval.StepFromTo(settings.StartingAngle, settings.EndingAngle))
            {
                if (!CreateBullet(prefab, input.Origin, input.Direction.Rotate2D(item), input.OnSpawn, settings.ProjectileSpeed, out Projectile p))
                {
                    continue;
                }
                output.Add(p);
                p.Action_AddPosition(p.CurrentVelocity.ScaleToMagnitude(0.15f));
            }
            return output != null && output.Count > 0;
        }
    }
    #endregion
    #region Projectile Actions
    public partial class Projectile
    {
        public Projectile Action_MatchOther(Projectile other)
        {
            prefab = other;
            Action_SetSprite(other.projectileSprite.sprite);
            ContainedDamage = other.ContainedDamage;

            return this;
        }
        public Projectile Action_MultiplyVelocity(float multiplier)
        {
            CurrentVelocity *= multiplier;
            return this;
        }
        public Projectile Action_SetVelocity(Vector2 direction, float speed)
        {
            CurrentVelocity = direction.ScaleToMagnitude(speed);
            Action_FacePosition(CurrentPosition + CurrentVelocity);
            return this;
        }
        public Projectile Action_Retarget(Transform t)
        {
            Action_SetVelocity((Vector2)t.position - CurrentPosition, CurrentSpeed);
            return this;
        }
        public Projectile Action_FacePosition(Vector2 worldPosition)
        {
            projectileSprite.transform.Lookat2D(worldPosition);
            return this;
        }
        public Projectile Action_SetSprite(Sprite s)
        {
            projectileSprite.sprite = s; return this;
        }
        public Projectile Action_SpriteColorSLOW(Color32 c)
        {
            projectileSprite.color = c; return this;
        }
        public Projectile Action_SetSpriteLayerIndex(int index)
        {
            projectileSprite.sortingOrder = index; return this;
        }
        public Projectile Action_SetFaction(BremseFaction f)
        {
            this.Faction = f; return this;
        }
        public Projectile Action_SetDamage(float newDamage)
        {
            if (Owner != null)
            {
                newDamage = Owner.DamageScale(newDamage);
            }
            this.ContainedDamage = newDamage; return this;
        }
        public Projectile Action_SetOwnerReference(BaseUnit owner)
        {
            this.Owner = owner; return this;
        }
        public Projectile Action_AddPosition(Vector2 direction)
        {
            transform.position = CurrentPosition + direction;
            return this;
        }
        public Projectile Action_AddPositionForward(float distance)
        {
            transform.position = CurrentPosition + CurrentVelocity.ScaleToMagnitude(distance);
            return this;
        }
        public Projectile Action_NewPosition(Vector2 position)
        {
            transform.position = position;
            return this;
        }
        public Projectile Action_AddRotation(float value)
        {
            return Action_SetVelocity(CurrentVelocity.Rotate2D(value), CurrentVelocity.magnitude);
        }
        public Projectile Action_SplitBROKEN(float arcAngle, float arcRotation, int splitCount, bool destroy = true)
        {
            float iterationSplitRotation = 0f;
            for (int i = 0; i < splitCount; i++)
            {
                iterationSplitRotation = splitCount > 1 ? (-0.5f * arcAngle + ((arcAngle / splitCount) * i)) : 0f;
                if (CreateBullet(prefab, CurrentPosition, CurrentVelocity, null, CurrentSpeed, out Projectile p))
                {
                    p.Action_SetVelocity(CurrentVelocity, CurrentVelocity.magnitude).Action_AddRotation(iterationSplitRotation + arcRotation);
                }
            }
            if (destroy)
            {
                ClearProjectile();
                return null;
            }
            return this;
        }
        public struct crawlerPacket
        {
            public float delay;
            public float aimAngle;
            public float repeatAngle;
            public int repeatCount;
            public float repeatTimeInterval;
            public Action<Projectile> OnSpawn;
            public crawlerPacket(float delay, float aimAngle, float repeatAngle, int repeatCount, float repeatTimeInterval)
            {
                this.delay = delay;
                this.aimAngle = aimAngle;
                this.repeatAngle = repeatAngle;
                this.repeatCount = repeatCount;
                this.repeatTimeInterval = repeatTimeInterval;
                OnSpawn = null;
            }
            public crawlerPacket AttachOnSpawnEvent(Action<Projectile> a)
            {
                OnSpawn += a;
                return this;
            }
        }
        public Projectile Action_AttachCrawlerEvent(Projectile crawlerPrefab, ArcSettings arc, crawlerPacket crawlerData)
        {
            ProjectileModCrawler crawlerEvent = new ProjectileModCrawler(new(1f, crawlerData.delay), new(crawlerPrefab, arc, crawlerData.aimAngle));
            crawlerEvent.AttachOnSpawnEvent(crawlerData.OnSpawn);
            crawlerEvent.SetRepeats(crawlerData.repeatCount, crawlerData.repeatTimeInterval, crawlerData.repeatAngle);
            this.AddMod(crawlerEvent);
            return this;
        }
    }
    #endregion
    #region Spawning
    public partial class Projectile
    {
        ProjectileSpawnAction storedSpawnAction;
        static Transform bulletFolder;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeSpawnID()
        {
            NextSpawnID = 0;
        }
        public int SpawnID { get; private set; }
        static int NextSpawnID;
        public delegate void ProjectileSpawnAction(Projectile p);
        private static bool CreateBullet(Projectile prefab, Vector2 position, Vector2 direction, ProjectileSpawnAction spawnAction, float speed, out Projectile spawnedBullet)
        {
            spawnedBullet = null;
            if (prefab == null)
                return spawnedBullet != null;
            if (IsSweeping && prefab.sweepable)
            {
                return false;
            }
            Projectile p = null;
            if (TryGetFromPool(prefab.poolID, out p) && p != null)
            {
                p.Action_NewPosition(position);
                p.gameObject.SetActive(true);
            }
            else
            {
                if (p == null)
                {
                    //something bad destroy the queues lemao
                    DestroyPool(prefab.poolID);
                }
                if (bulletFolder == null)
                {
                    bulletFolder = new GameObject("Projectile Folder").transform;
                }
                p = Instantiate(prefab, position, Quaternion.identity);
                p.transform.SetParent(bulletFolder);
                p.hideFlags = HideFlags.HideInHierarchy;
            }
            p.SpawnID = NextSpawnID++;
            p.knownMods = new();
            p.Action_SetVelocity(direction, speed);
            p.Action_MatchOther(prefab);
            p.storedSpawnAction = spawnAction;
            p.projectileColliderComponent.SetProjectile(p);
            
            spawnAction?.Invoke(p);

            spawnedBullet = p;
            activeBullets.Add(p);
            p.OnScreenExit = null;
            return spawnedBullet != null;
        }
    }
    #endregion
    #region Sweeping
    public partial class Projectile
    {
        Projectile prefab;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void ReinitializeSweep()
        {
            SweepEndTime = 0;
            sweepList = new();
        }
        static float SweepEndTime;
        public static bool IsSweeping => Time.time < SweepEndTime;
        static byte sweepLootWeight;
        public static bool SweepLoot => sweepLootWeight > Helper.SeededRandomInt256;
        [field: SerializeField] public bool sweepable { get; private set; } = true;
        [QFSW.QC.Command("-sweep2")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void CommandSweep()
        {
            SweepBullets(0.5f, 15);
        }
        static List<Projectile> sweepList;
        public static void SweepBullets(float sweepDuration, byte lootWeight)
        {
            sweepLootWeight = lootWeight;
            SweepEndTime = Time.time + sweepDuration;
            sweepList.Clear();
            foreach (var item in activeBullets)
            {
                if (item != null && item.sweepable)
                {
                    sweepList.Add(item);
                    if (SweepLoot)
                    {
                        //WakaScoring.SpawnPickup(item.CurrentPosition);
                    }
                }
            }
            for (int i = 0; i < sweepList.Count; i++)
            {
                sweepList[i].ClearProjectile();
                sweepList.RemoveAt(i);
                i--;
                continue;
            }
        }
    }
    #endregion
    #region Pooling

    #region Editor
#if UNITY_EDITOR

    [CustomEditor(typeof(Projectile), true)]
    public class ChurroProjectileEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();  // Draw the default inspector for ChurroBaseAttack.

            // Add a button in the Inspector
            if (GUILayout.Button("Get New Pool ID"))
            {
                // Get the ChurroBaseAttack component
                Projectile pTarget = (Projectile)target;
                pTarget.poolID = System.Guid.NewGuid().ToString().GetHashCode();
                pTarget.Dirty();
                AssetDatabase.SaveAssets();

                // Log to confirm the ID was added
                Debug.Log($"Added PoolingID to {pTarget.gameObject.name} with ID: {pTarget.poolID}");
            }
        }
    }
#endif
    #endregion
    public partial class Projectile
    {
        [SerializeField] bool pooling = true;
        static Dictionary<int, Queue<Projectile>> pools;
        public int poolID;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ReinitializeProjectilePool()
        {
            pools = new();
            activeBullets = new();
            GeneralManager.SetStageLoadAction("Initialize Projectile Pool", ForceResetProjectileSystem);
        }
        private static bool TryGetQueueFor(int poolID, out Queue<Projectile> queue)
        {
            queue = null;
            if (!pools.ContainsKey(poolID))
            {
                pools[poolID] = new();
            }
            if (pools.ContainsKey(poolID) && pools[poolID] != null)
            {
                queue = pools[poolID];
            }
            return queue != null;
        }
        static void DestroyPool(int poolID)
        {
            if (TryGetQueueFor(poolID, out Queue<Projectile> queue))
            {
                foreach (var item in queue)
                {
                    if (item != null && item.gameObject != null)
                    {
                        Destroy(item.gameObject);
                    }
                }
                queue.Clear();
            }
        }
        static bool TryGetFromPool(int poolID, out Projectile p)
        {
            p = null;
            if (pools.ContainsKey(poolID) && pools[poolID] != null && pools[poolID].Count > 0)
            {
                p = pools[poolID].Dequeue();
                return true;
            }
            return false;
        }
        public bool ClearProjectile(int bounceCost = 1)
        {
            void Local_Destroy()
            {
                Destroy(gameObject);
                activeBullets.Remove(this);
            }
            void TryPool()
            {
                if (TryGetQueueFor(poolID, out Queue<Projectile> queue))
                {
                    queue.Enqueue(this);
                    gameObject.SetActive(false);
                }
                else
                {
                    Local_Destroy();
                }
                activeBullets.Remove(this);
            }
            ClearMods();
            if (!pooling)
            {
                Local_Destroy();
                return true;
            }
            TryPool();
            return true;
        }
    }
    #endregion
    #region Offscreen
    public partial class Projectile
    {
        static TagHandle stageboxTag => TagHandle.GetExistingTag("Projectile Stagebox");
        public delegate void ScreenExitAction(Projectile p, Vector2 edgeNormal);
        private ScreenExitAction OnScreenExit;
        public void AddOnScreenExitEvent(ScreenExitAction action)
        {
            OnScreenExit += action;
        }
        private void OnApplicationQuit()
        {
            foreach (var item in activeBullets)
            {
                item.ClearMods();
                item.OnScreenExit = null;
            }
        }
        public static void TryTriggerOnScreenExit(Projectile p, Collider2D collision)
        {
            if (collision.CompareTag(stageboxTag))
            {
                p.OnScreenExit?.Invoke(p, -(p.CurrentVelocity.QuantizeToStepSize(90f)));
                p.OnScreenExit = null;
            }
        }
    }
    #endregion
    #region Spawn Loot
    public partial class Projectile
    {
        static float StoredLootDamage;
        static float DamagePerLootItem = 10f;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ReinitializeLootDamage()
        {
            StoredLootDamage = 0;
        }
        public static void ContributeTowardsLoot(float damage, Vector2 position)
        {
            StoredLootDamage += damage;
            while (StoredLootDamage >= DamagePerLootItem)
            {
                StoredLootDamage -= DamagePerLootItem;
                //WakaScoring.SpawnPickup(position + UnityEngine.Random.insideUnitCircle);
            }
        }
    }
    #endregion
    #region Collide
    public partial class Projectile
    {
        //Moved collision onto projectile collider component so it can rotate as a child, since the rigidbody is jank and dogshit
        [SerializeField] ProjectileCollider projectileColliderComponent;
        public enum CollisionResult
        {
            Error,
            Friends,
            DefaultObject,
            Hit
        }
        public void PerformCollisionResult(CollisionResult result, Collider2D collision)
        {
            switch (result)
            {
                case CollisionResult.Error:
                    break;
                case CollisionResult.Friends:
                    break;
                case CollisionResult.DefaultObject:
                    if (!collision.CompareTag(stageboxTag))
                    {
                        ClearProjectile();
                    }
                    break;
                case CollisionResult.Hit:
                    if (collision.GetComponent<IHitListener>() is IHitListener listener)
                    {
                        HitPacket packet = new(CurrentPosition, ContainedDamage);
                        listener.PerformHit(packet);
                    }
                    ClearProjectile();
                    break;
                default:
                    break;
            }
        }
    }
    #endregion
    [RequireComponent(typeof(Rigidbody2D))]
    public partial class Projectile : MonoBehaviour
    {
        static HashSet<Projectile> activeBullets;
        public static int BulletCount => activeBullets == null ? 0 : activeBullets.Count;
        public static void ForceResetProjectileSystem()
        {
            ReinitializeProjectilePool();
            activeBullets = new();
        }
        float nextEventTickTime;
        float lastTickTime;
        float tickTimeLength = 0.05f;
        [field: SerializeField] public Collider2D ProjectileCollider { get; private set; }
        public Vector2 CurrentVelocity { get; private set; }
        public SpriteRenderer projectileSprite;
        [SerializeField] Rigidbody2D projectileRB;
        public BaseUnit Owner { get; private set; }
        public float ContainedDamage { get; private set; } = 1f;
        public Vector2 CurrentPosition => transform.position;
        public float CurrentSpeed => CurrentVelocity.magnitude;
        public BremseFaction Faction { get; private set; } = BremseFaction.Enemy;
        public Rigidbody2D RB => projectileRB;
        public static void RunActiveBullets()
        {
            foreach (var item in activeBullets)
            {
                item.RunProjectile();
            }
        }
        public static void LateRunActiveBullets(float velocityScale)
        {
            foreach (var item in activeBullets)
            {
                item.PerformVelocity(velocityScale);
            }
        }
        public void RunProjectile()
        {
            if (Time.time > nextEventTickTime)
            {
                float tickDuration = Time.time - lastTickTime;
                lastTickTime = Time.time;
                nextEventTickTime = Time.time + tickTimeLength;
                RunEvents(tickDuration);
            }
        }
        public void PerformVelocity(float velocityScale)
        {
            PerformVelocity(CurrentVelocity * velocityScale);
        }
        public void PerformVelocity(Vector2 velocity)
        {
            if (RB == null)
            {
                activeBullets.Remove(this);
                if (gameObject != null)
                {
                    Destroy(gameObject);
                }
            }
            RB.linearVelocity = velocity;
        }
    }
}
