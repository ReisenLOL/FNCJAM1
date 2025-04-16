using UnityEngine;

public class StageManager : MonoBehaviour
{
    #region Variables
    public static StageManager instance;
    public Stage currentStage;
    public Stage nextStage;
    public SpawnManager spawnManager;
    public PlayerUnit playerUnit;
    #endregion
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
        } else
        {
            instance = this;
        }
    }

    private void Start()
    {
        InitializeStage();
    }

    // Update is called once per frame
    void SwitchStage(Stage stageToSwitchTo)
    {
        currentStage = stageToSwitchTo;
        nextStage = null;
    }

    void InitializeStage()
    {
        spawnManager.killQuota = currentStage.stageEnemiesCount;
        spawnManager.spawnRate = currentStage.stageEnemiesSpawnRate;
        spawnManager.currentKills = 0;
        spawnManager.canSpawn = true;
        playerUnit.ChangeHealth(playerUnit.MaxHealth);
    }
}
