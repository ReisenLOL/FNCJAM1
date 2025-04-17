using Core.Extensions;
using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Projectile
{
    public class ReisenAttack : BaseAttack
    {
        [SerializeField] ProjectilePrefabSO burstShot;
        [SerializeField] ProjectilePrefabSO frontal;
        [SerializeField] ACWrapper burstSound;
        [SerializeField] int burstCount = 6;
        protected override void AttackPayload(Projectile.InputSettings input)
        {
            WaitForSeconds burstStall = new(swingTimeSettings.SwingDuration / burstCount.MultiplyAndFloorAsFloat(2f));
            void FrontalShot()
            {
                Projectile.InputSettings frontalInput = input.Copy();
                frontalInput.SetOrigin(input.Origin + input.Direction.Rotate2D(45f).ScaleToMagnitude(0.55f));
                Single(0f, 35f).Spawn(frontalInput, frontal, out iterationList);
                foreach (var iteration in iterationList)
                {
                    iteration.Action_SetSpriteLayerIndex(50);
                }
                frontalInput.SetOrigin(input.Origin + input.Direction.Rotate2D(-45f).ScaleToMagnitude(0.55f));
                Single(0f, 35f).Spawn(frontalInput, frontal, out iterationList);
                foreach (var iteration in iterationList)
                {
                    iteration.Action_SetSpriteLayerIndex(50);
                }
            }
            IEnumerator CO_Burst()
            {
                for (int i = 0; i < burstCount; i++)
                {
                    input.SetOrigin(owner.CurrentPosition);
                    Single(3f.RandomPositiveNegativeRange(), 28f).Spawn(input, burstShot, out iterationList);
                    foreach(var iteration in iterationList)
                    {
                        iteration.Action_AddPositionForward(0.75f);
                    }
                    burstSound.Play(input.Origin);
                    yield return burstStall;
                }
            }
            StartCoroutine(CO_Burst());
            FrontalShot();
        }
    }
}
