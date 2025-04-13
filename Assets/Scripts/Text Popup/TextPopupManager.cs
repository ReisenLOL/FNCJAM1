using UnityEngine;
using DamageNumbersPro;
using Bremsengine;
using Unity.VisualScripting;
public class TextPopupManager : MonoBehaviour
{
    static TextPopupManager instance;
    [SerializeField] DamageNumber popupTextPrefab;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void ResetState()
    {
        instance = null;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public static void PopupText(Vector2 position, string text)
    {
        instance.popupTextPrefab.Spawn(position, text);
    }
}
