using Bremsengine;
using TMPro;
using UnityEngine;

public class PlayerLevelManager : MonoBehaviour
{
    public int level;
    public int requiredPowerToNextLevel;
    public int currentPower;
    public WeaponSelect levelUpUI;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI powerText;
    private void Start()
    {
        powerText.text = "Power: " + currentPower;
        levelText.text = "Level: " + level;
    }
    public void UpdatePower(int power)
    {
        currentPower += power;
        powerText.text = "Power: " + currentPower;
        if (currentPower >= requiredPowerToNextLevel)
        {
            LevelUp();
        }
    }
    public void LevelUp()
    {
        level++;
        requiredPowerToNextLevel += requiredPowerToNextLevel;
        levelUpUI.gameObject.SetActive(true);
        levelUpUI.ShowWeaponSelect();
        levelText.text = "Level: " + level;
        Debug.Log("Level up: " + level);
    }
}
