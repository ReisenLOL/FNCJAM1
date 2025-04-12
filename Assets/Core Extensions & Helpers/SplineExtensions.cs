using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Splines;
namespace Core.Extensions
{
    [System.Serializable]
    public class SplinePathSettings
    {
        static Vector2 CalculatedWorldCenter;
        [SerializeField] public bool PreventOverride;
        [SerializeField] public bool UseOwnerTransformAsWorldCenter;
        public Vector2 WorldCenter => PreventOverride ? WorldCenterOverride : WorldCenterOverride;
        public Vector2 WorldCenterOverride;
        public float RB_MaxSpeed;
        public float RB_Acceleration;
        public bool Looping;
        public float PathUpdateDistance;
        public float PathForceRecalculateDistance;
        [HideInInspector] public Vector2 CalculatedTargetPosition;
        public float CurrentLerpValue;
        public static void SetWorldCenter(Vector2 v)
        {
            CalculatedWorldCenter = v;
        }
        public void UseTransformAsWorldCenter(Vector2 v, bool forced)
        {
            if (UseOwnerTransformAsWorldCenter || forced)
            {
                UseOwnerTransformAsWorldCenter = true;
                this.WorldCenterOverride = v;
                PreventOverride = false;
                Helper.Repaint();
            }
        }
        public void ClearWorldCenterOverride()
        {
            WorldCenterOverride = Vector2.zero;
            UseOwnerTransformAsWorldCenter = false;
            PreventOverride = true;
            Helper.Repaint();
        }
    }
    public static class SplineExtensions
    {
        private static Vector2 BremseEvaluateLerp(this Spline path, Vector2 worldCenter, float lerp, bool looping = false)
        {
            Vector2 evaluatePosition = new();
            if (path.EvaluatePosition(looping ? lerp % 1f : lerp) is float3 f)
            {
                evaluatePosition.x = f.x; evaluatePosition.y = f.y;
                evaluatePosition += worldCenter;
            }
            return evaluatePosition;
        }
        public static float BremseGetLength(this Spline path)
        {
            float length = 0f;
            for (int i = 0; i < path.Count; i++)
            {
                length += path.GetCurveLength(i);
            }
            return length;
        }
        public static Vector2 LerpPosition(this Spline path, Vector2 worldCenter, float lerp, bool looping = false)
        {
            return BremseEvaluateLerp(path, worldCenter, lerp, looping);
        }
        public static Vector2 LerpPositionWithTime(this Spline path, Vector2 worldCenter, float time, bool looping = false)
        {
            return BremseEvaluateLerp(path, worldCenter, time / path.BremseGetLength(), looping);
        }
        public static Rigidbody2D RunAlongSpline(this Rigidbody2D rb, Spline path, SplinePathSettings settings)
        {
            if (settings.CalculatedTargetPosition.SquareDistanceToGreaterThan(rb.position, settings.PathForceRecalculateDistance))
            {
                settings.CalculatedTargetPosition = path.LerpPosition(settings.WorldCenter, settings.CurrentLerpValue, settings.Looping);
            }
            else if (settings.CalculatedTargetPosition.SquareDistanceToLessThan(rb.position, settings.PathUpdateDistance.Max(0.1f)))
            {
                settings.CurrentLerpValue += Time.deltaTime;
                settings.CalculatedTargetPosition = path.LerpPosition(settings.WorldCenter, settings.CurrentLerpValue, settings.Looping);
            }
            rb.VelocityTowards((settings.CalculatedTargetPosition - rb.position).ScaleToMagnitude(settings.RB_MaxSpeed), settings.RB_Acceleration.Max(0.5f));
            return rb;
        }
    }
}
