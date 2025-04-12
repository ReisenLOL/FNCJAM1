using UnityEditor;
using UnityEngine;

namespace Bremsengine
{
#if UNITY_EDITOR
    public class CreditsEditor : EditorWindow
    {
        static GameCreditsSO gameCredits;
        static string creditTitle;
        static string creditCreator;
        static string creditWebsite;
        static int priority;
        [MenuItem("Bremsengine/Credits Adder")]
        static void Init()
        {
            EditorWindow window = GetWindow(typeof(CreditsEditor));
        }
        private void OnGUI()
        {
            gameCredits = EditorGUILayout.ObjectField("Game Credits Selection", gameCredits, typeof(GameCreditsSO), false) as GameCreditsSO;
            creditTitle = EditorGUILayout.TextField("Credits Title: ", creditTitle);
            creditCreator = EditorGUILayout.TextField("Creator: ", creditCreator);
            creditWebsite = EditorGUILayout.TextField("Website: ", creditWebsite);
            priority = EditorGUILayout.IntSlider(priority, -500, 500);
            if (GUILayout.Button("Create"))
            {
                SendInfo();
            }
        }
        private static void SendInfo()
        {
            CreditsSO.Create(gameCredits, creditTitle, creditCreator, creditWebsite, priority);
        }
    }
#endif
}
