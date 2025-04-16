using Bremsengine;
using TMPro;
using UnityEngine;

#region XPBar
public partial class PlayerLevelManager
{
    public RectTransform XPBar;
    public void UpdateXPBar()
    {
        XPBar.localScale = new Vector3(currentPower / requiredPowerToNextLevel, XPBar.localScale.y);
    }
}
#endregion
public partial class PlayerLevelManager : MonoBehaviour
{
    public int level;
    public float requiredPowerToNextLevel;
    public float basePowerToNextLevel;
    public float currentPower;
    public float powerModifier = 1f;
    public float exponentIncrease;
    public WeaponSelect levelUpUI;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI powerText;
    private void Start()
    {
        powerText.text = "Power: " + currentPower;
        levelText.text = "Level: " + level;
        UpdateXPBar();
    }
    public void UpdatePower(float power)
    {
        currentPower += power * powerModifier;
        powerText.text = "Power: " + currentPower;
        if (currentPower >= requiredPowerToNextLevel)
        {
            LevelUp();
        }
        UpdateXPBar();
    }
    public void LevelUp()
    {
        level++;
        requiredPowerToNextLevel = Mathf.FloorToInt(basePowerToNextLevel * Mathf.Pow(level, exponentIncrease));
        levelUpUI.gameObject.SetActive(true);
        levelUpUI.ShowWeaponSelect();
        levelText.text = "Level: " + level;
        Debug.Log("Level up: " + level);
    }
}
