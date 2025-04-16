using UnityEngine;

[CreateAssetMenu(fileName = "stageSO", menuName = "Scripts/StageData")]
public class Stage : ScriptableObject //alternatively we can have stages just be scenes. a scene for each stage
{
    public enum STAGETYPE
    {
        Other = 0,
        Combat = 1,
        Shop = 2,
        Boss = 3
    }
    public STAGETYPE type;

    //in case its combat
    public int stageEnemiesCount;
    public float stageEnemiesSpawnRate;
    public int stageDifficulty; //should determine the chances of stronger enemies spawning
}
