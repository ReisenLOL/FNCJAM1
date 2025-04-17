using Bremsengine;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    #region Variables
    public static StageManager instance;
    public Stage currentStage;
    public Stage nextStage;
    [SerializeField] SpawnManager spawnManager;
    [SerializeField] PlayerUnit playerUnit;
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
        playerUnit.ChangeHealth(playerUnit.MaxHealth);
        DialogueManager dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();

        //dialogueManager.StartDialogue();
    }
}
