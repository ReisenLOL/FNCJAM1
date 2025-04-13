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

    public static void HealthPopupText(Vector2 position, int damage, Color color)
    {
        if (damage == 0) return;
        instance.popupTextPrefab.Spawn(position, damage);
        instance.popupTextPrefab.SetColor(color);
    }
}
