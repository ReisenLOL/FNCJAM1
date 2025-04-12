using Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile
{
    public class ProjectileModCrawler : ProjectileMod
    {
        [System.Serializable]
        public struct crawlerSettings
        {
            public Projectile crawlerPrefab;
            public Projectile.ArcSettings arc;
            public float addedAimAngle;
            public float interval;
            public int repeats;
            public float addedRepeatAngle;
            public Action<Projectile> OnSpawn;
            public crawlerSettings(Projectile crawler, Projectile.ArcSettings arc, float addedAimAngle)
            {
                this.crawlerPrefab = crawler;
                this.arc = arc;
                this.addedAimAngle = addedAimAngle;
                this.interval = 0f;
                this.repeats = 0;
                this.addedRepeatAngle = 0f;
                OnSpawn = null;
            }
            public crawlerSettings SetRepeats(int repeats, float repeatTime, float addedRepeatAngle)
            {
                this.repeats = repeats;
                this.interval = repeatTime;
                this.addedRepeatAngle = addedRepeatAngle;
                return this;
            }
            public crawlerSettings AttachOnSpawnEvent(Action<Projectile> action)
            {
                OnSpawn += action;
                return this;
            }
        }
        public ProjectileModCrawler SetRepeats(int repeats, float interval, float addedRepeatAngle)
        {
            crawler.SetRepeats(repeats, interval, addedRepeatAngle);
            return this;
        }
        crawlerSettings crawler;
        public
        bool hasSpawned = false;
        public ProjectileModCrawler(modSettings settings, crawlerSettings crawler)
        {
            ApplySettings(settings);
            this.crawler = crawler;
        }
        public crawlerSettings AttachOnSpawnEvent(Action<Projectile> action)
        {
            crawler.OnSpawn += action;
            return crawler;
        }
        protected override void OnFirstRunPayload(Projectile eventProjectile)
        {
            IEnumerator CO_Repeat(Projectile eventProjectile)
            {
                WaitForSeconds repeatDelay = new(crawler.interval);
                for (int i = 0; i < crawler.repeats; i++)
                {
                    yield return repeatDelay;
                    SpawnCrawler(eventProjectile, crawler.addedAimAngle + crawler.addedRepeatAngle * i);
                }
            }
            SpawnCrawler(eventProjectile, crawler.addedAimAngle);
            if (eventProjectile != null && eventProjectile.isActiveAndEnabled && crawler.repeats > 0)
            {
                eventProjectile.StartCoroutine(CO_Repeat(eventProjectile));
            }
        }
        private void SpawnCrawler(Projectile owner, float rotation)
        {
            if (owner == null)
            {
                return;
            }
            Projectile.InputSettings input = new(owner.CurrentPosition, owner.CurrentVelocity.Rotate2D(rotation));
            Projectile.SpawnArc(crawler.crawlerPrefab, input, crawler.arc, out List<Projectile> p);
            foreach (var item in p)
            {
                crawler.OnSpawn?.Invoke(item);
            }
        }
        protected override void RunPayload(Projectile eventProjectile, float deltaTime)
        {

        }
    }
}
