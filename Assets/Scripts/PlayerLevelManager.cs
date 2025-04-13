using TMPro;
using UnityEngine;

public class PlayerLevelManager : MonoBehaviour
{
    public int level;
    public int requiredPowerToNextLevel;
    public int currentPower;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI powerText;
    private void Start()
    {
        UpdatePower(0);
        LevelUp();
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
        levelText.text = "Level: " + level;
        Debug.Log("Level up: " + level);
    }
}
