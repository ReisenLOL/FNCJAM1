using UnityEngine;
using DamageNumbersPro;
using Bremsengine;
using Unity.VisualScripting;
public class TextPopupManager : MonoBehaviour
{
    static TextPopupManager instance;
    [SerializeField] DamageNumber popupTextPrefab;
    [SerializeField] DamageNumber playerDamageIncoming;
    [SerializeField] DamageNumber playerDamageOutgoing;
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
    public static void PlayerDamageOutwards(Vector2 position, int damage)
    {
        if (damage <= 0f)
        {
            return;
        }
        instance.playerDamageOutgoing.Spawn(position, damage);
    }
    public static void PlayerDamageIncoming(Vector2 position, int damage)
    {
        if (damage == 0) return;
        instance.playerDamageIncoming.Spawn(position, damage);
    }
}
