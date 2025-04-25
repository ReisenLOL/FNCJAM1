using Bremsengine;
using TMPro;
using UnityEngine;

#region XPBar
public partial class PlayerLevelManager
{
    public RectTransform XPBar;
    public void UpdateXPBar()
    {
        XPBar.localScale = new Vector3((currentPower - requiredPowerToNextLevelMin) / (requiredPowerToNextLevel - requiredPowerToNextLevelMin), XPBar.localScale.y);
    }
}
#endregion
public partial class PlayerLevelManager : MonoBehaviour
{
    public int level;
    public float requiredPowerToNextLevel;
    public float requiredPowerToNextLevelMin;
    public float basePowerToNextLevel;
    public float currentPower;
    public float powerModifier = 1f;
    public float exponentIncrease;
    public WeaponSelect levelUpUI;
    public SpawnManager spawnManager;
    public TextMeshProUGUI levelText;
    private void Start()
    {
        if (PlayerItemData.instance.playerLevel > 0)
        {
            level = PlayerItemData.instance.playerLevel;
        }
        currentPower = PlayerItemData.instance.powerAmount;
        if (PlayerItemData.instance.requiredPowerToNextLevel > 0)
        {
            requiredPowerToNextLevel = PlayerItemData.instance.requiredPowerToNextLevel;
        }
        if (requiredPowerToNextLevelMin > 0)
        {
            requiredPowerToNextLevelMin = PlayerItemData.instance.requiredPowerToNextLevelMin;
        }
        levelText.text = "Level: " + level;
        UpdateXPBar();
    }

    public void UpdatePower(float power)
    {
        currentPower += power * powerModifier;
        if (currentPower >= requiredPowerToNextLevel)
        {
            LevelUp();
        }
        UpdateXPBar();
    }
    [ContextMenu("Force Level Up")]
    public void LevelUp()
    {
        level++;
        requiredPowerToNextLevelMin = requiredPowerToNextLevel;
        requiredPowerToNextLevel = Mathf.FloorToInt(basePowerToNextLevel * Mathf.Pow(level, exponentIncrease));
        levelUpUI.gameObject.SetActive(true);
        levelUpUI.ShowWeaponSelect();
        spawnManager.RecalculateHealthModifier(level);
        levelText.text = "Level: " + level;
        Debug.Log("Level up: " + level);
    }
}
