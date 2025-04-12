using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

namespace Core.Extensions
{
    [CreateAssetMenu(menuName = "Splines/Bremse Spline SO", fileName = "Spline SO")]
    public class SplineSO : ScriptableObject
    {
        public static implicit operator Spline(SplineSO s) => s.containedSpline;
        [field: SerializeField] public Spline containedSpline { get; private set; }
        public void SetSpline(Spline spline)
        {
            containedSpline = spline;
            this.SetDirtyAndSave();
        }
        private void OnEnable()
        {
#if UNITY_EDITOR
            if (EditorUtility.IsDirty(this))
            {
                Debug.Log("Saved Dirty SplineSO");
                this.SetDirtyAndSave();
            }
#endif
        }
    }
}
