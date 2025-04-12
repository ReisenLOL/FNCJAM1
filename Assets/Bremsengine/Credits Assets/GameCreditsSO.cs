using UnityEngine;
using System.Collections.Generic;
using Core.Extensions;
using UnityEditor;
namespace Bremsengine
{
#if UNITY_EDITOR
    public partial class GameCreditsSO
    {
        public void AddCredit(CreditsSO c)
        {
            credits.Add(c);
            AssetDatabase.SaveAssets();
        }
    }
#endif
    [CreateAssetMenu(menuName = "Bremsengine/Credits", fileName = "New Game Credits")]
    public partial class GameCreditsSO : ScriptableObject
    {
        [SerializeField] string gameName;
        [SerializeField] string gameCreator;
        [SerializeField] List<CreditsSO> credits = new();
        public string CompileCredits()
        {
            credits.Sort(CreditsSO.SortByPriority);
            string spam = "";
            spam += $"{gameName}##by {gameCreator}########".ReplaceLineBreaks("##");
            foreach (var c in credits)
            {
                spam += c.ToString();
                spam += "##".ReplaceLineBreaks("##");
            }
            return spam;
        }
    }
}