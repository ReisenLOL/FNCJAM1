using UnityEngine;

public class LevelUpCheat : MonoBehaviour
{
    public bool debug;
    public PlayerLevelManager levelManager;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G) && debug)
        {
            levelManager.UpdatePower(levelManager.requiredPowerToNextLevel - levelManager.currentPower);
        }
    }
}
