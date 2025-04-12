using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

namespace Core.Extensions
{
#if UNITY_EDITOR
    using UnityEditor.Splines;
    [CustomEditor(typeof(SplineSO))]
    public class SplineScriptableObjectEditor : Editor
    {
        private GameObject temporarySplineObject;
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SplineSO s = null;
            if (target is SplineSO splineSO)
            {
                s = splineSO;
            }
            if (s == null)
            {
                return;
            }
            if (GUILayout.Button("Edit Spline in Scene"))
            {
                s.Dirty();
                if (temporarySplineObject == null)
                {
                    temporarySplineObject = new GameObject("Temporary Spline");
                    temporarySplineObject.hideFlags = HideFlags.HideAndDontSave;
                    temporarySplineObject.AddComponent<SplineContainer>();
                }

                var splineContainer = temporarySplineObject.GetComponent<SplineContainer>();
                splineContainer.Spline = s.containedSpline;

                Selection.activeGameObject = temporarySplineObject;
                return;
            }
            if (EditorUtility.IsDirty(s) && (GUILayout.Button("Save Asset")))
            {
                s.SetDirtyAndSave();
            }
            EditorGUILayout.TextArea($"Yo, im dumb and i cant get it to mark it\nas dirty from the spline editor,\n so it only marks it dirty when u press edit spline\nPlease wait with saving or go back\nand press edit after youre done anyway\nto mark it as dirty again.\nctrl + s or Project -> Save Assets\nshould also save it and clear dirty.");
        }
    }
#endif
}