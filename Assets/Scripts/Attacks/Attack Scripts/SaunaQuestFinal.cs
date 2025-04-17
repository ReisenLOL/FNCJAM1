using Core.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile
{
    public class SaunaQuestFinal : BaseAttack
    {
        [SerializeField] ProjectilePrefabSO spiralProjectile;
        [SerializeField] ProjectilePrefabSO ringProjectile;
        [SerializeField] float attackLength = 35f;
        [SerializeField] int iterations = 300;
        protected override void AttackPayload(Projectile.InputSettings input)
        {
            List<Projectile> Spiral(float speed, int arms, float rotation = -90f)
            {
                Arc(0f, 360f, 360f / (arms - 1), speed).Spawn(input.SetDirection(input.Direction.Rotate2D(rotation)), spiralProjectile, out iterationList);
                foreach (Projectile item in iterationList)
                {
                    item.AddOnScreenExitEvent((Projectile p, Vector2 normal) => p.ClearProjectile());
                }
                return iterationList;
            }
            List<Projectile> Ring(float speed, int arms, float rotation = -90f)
            {
                Arc(0f, 360f, 360f / (arms - 1), speed).Spawn(input.SetDirection(input.Direction.Rotate2D(rotation)), ringProjectile, out iterationList);
                foreach (Projectile item in iterationList)
                {
                    item.AddOnScreenExitEvent((Projectile p, Vector2 normal) => p.ClearProjectile());
                }
                return iterationList;
            }
            StartCoroutine(CO_Attack());
            IEnumerator CO_Attack()
            {
                float repeatDelay = attackLength / iterations;
                WaitForSeconds stall = new WaitForSeconds(repeatDelay);
                for (int i = 0; i < iterations; i++)
                {
                    input.SetOrigin(owner.CurrentPosition);
                    input.SetDirection(Down);
                    #region spirals
                    attackSound.Play(input.Origin);
                    foreach (var item in Spiral(4f, 6, -22.5f + (i * 5.7f)))
                    {
                        item.AddMod(new ProjectileModAccelerate(new(1f, 1f), 1.5f, 6f));
                    }
                    foreach (var item in Spiral(4f, 6, -22.5f + (i * -1.9f)))
                    {
                        item.AddMod(new ProjectileModAccelerate(new(1f, 1f), 1.5f, 6f));
                    }
                    foreach (var item in Spiral(3f, 6, 45f + (i * -1.9f)))
                    {
                        item.AddMod(new ProjectileModAccelerate(new(1f, 1f), 1.5f, 6f));
                    }
                    foreach (var item in Spiral(3f, 6, 45f + (i * 5.2f)))
                    {
                        item.AddMod(new ProjectileModAccelerate(new(1f, 1f), 1.5f, 6f));
                    }
                    #endregion
                    #region Rings
                    if (i % 40 == 39)
                    {
                        foreach (var item in Ring(4f, 44, Random.Range(0f, 10f)))
                        {

                        }
                    }
                    if (i % 80 == 79)
                        foreach (var item in Ring(6f, 52, Random.Range(0f, 10f)))
                        {

                        }
                    if (i % 120 == 119)
                        foreach (var item in Ring(8f, 60, Random.Range(0f, 10f)))
                        {

                        }
                    if (i % 160 == 159)
                        foreach (var item in Ring(10f, 68, Random.Range(0f, 10f)))
                        {

                        }
                    if (i % 200 == 199)
                        foreach (var item in Ring(12f, 76, Random.Range(0f, 10f)))
                        {

                        }
                    #endregion
                    yield return stall;
                }
            }
        }
    }
}
