using UnityEngine;
using System.Collections.Generic;

namespace Core.Extensions
{
    public static partial class Helper
    {
        static RaycastHit2D[] cache = new RaycastHit2D[30];
        public static bool TryFindInCircleCast<T>(Vector2 origin, float radius, LayerMask layerMask, out HashSet<T> result) where T : MonoBehaviour
        {
            cache = Physics2D.CircleCastAll(origin, radius, Vector2.zero, 0f, layerMask);
            if (cache.Length <= 0)
            {
                result = null;
                return false;
            }
            result = new();
            foreach (var hit in cache)
            {
                if (hit.transform != null && hit.transform.TryGetComponent(out T component))
                {
                    result.Add(component);
                }
            }
            return result != null && result.Count > 0;
        }
        public static bool RaycastTryGetComponent<T>(Vector2 origin, Vector2 target, LayerMask layerMask, out T result) where T : MonoBehaviour
        {
            int hits = Physics2D.RaycastNonAlloc(origin, origin + (target - origin), cache, (target - origin).magnitude, layerMask);
            result = null;
            if (hits <= 0)
            {
                return result != null;
            }
            if (hits > 0)
            {
                foreach (var hit in cache)
                {
                    if (hit.transform != null)
                    {
                        result = hit.transform.GetComponent<T>();
                        break;
                    }
                }
            }
            return result != null;
        }
    }
}
