using Bremsengine;
using TMPro;
using UnityEngine;

#region XPBar
public partial class PlayerLevelManager
{
    public RectTransform XPBar;
    public void UpdateXPBar()
    {
        XPBar.localScale = new Vector3((float)currentPower / (float)requiredPowerToNextLevel, XPBar.localScale.y);
    }
}
#endregion
public partial class PlayerLevelManager : MonoBehaviour
{
    public int level;
    public int requiredPowerToNextLevel;
    public int basePowerToNextLevel;
    public int currentPower;
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
    public void UpdatePower(int power)
    {
        currentPower += power;
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
