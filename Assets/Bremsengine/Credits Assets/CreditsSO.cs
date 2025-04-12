using Core.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Bremsengine
{
#if UNITY_EDITOR
    public partial class CreditsSO
    {
        public static CreditsSO Create(GameCreditsSO gameCredits, string title, string credit, string website, int priority)
        {
            CreditsSO asset = (CreditsSO)ScriptableObject.CreateInstance(typeof(CreditsSO));
            asset.Title = title;
            asset.Credit = credit;
            asset.Website = website;
            asset.Priority = priority;

            if (string.IsNullOrWhiteSpace(asset.ToString()))
                return null;

            asset.name = title + " : " + credit;
            AssetDatabase.AddObjectToAsset(asset, gameCredits);
            gameCredits.AddCredit(asset);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            asset.EditorPing();
            return asset;
        }
    }
#endif
    public partial class CreditsSO : ScriptableObject
    {
        public int Priority = 0;
        public string Title = "";
        public string Credit = "";
        public string Website = "";
        public override string ToString()
        {
            string creditText = "";
            creditText += Title.Capitalized() + "##".ReplaceLineBreaks("##");
            creditText += Credit.Capitalized() + "##".ReplaceLineBreaks("##");
            if (!string.IsNullOrWhiteSpace(Website))
            {
                creditText += Website.Capitalized() + "####".ReplaceLineBreaks("##");
            }
            return creditText;
        }
        public static int SortByPriority(CreditsSO a, CreditsSO b)
        {
            return b.Priority.CompareTo(a.Priority);
        }
    }
}
