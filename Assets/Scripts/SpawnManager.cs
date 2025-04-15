using Core.Extensions;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyToSpawn;
    public Transform enemyFolder;
    public Transform[] spawnPoints;
    public int killQuota;
    public int currentKills;
    public float spawnRate;
    public float spawnTime;
    void Start()
    {
        spawnPoints = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();
    }
    void Update()
    {
        spawnTime += Time.deltaTime;
        if (spawnTime >= spawnRate)
        {
            Vector2 spawnPosition = Vector2.zero;
            for (int i = 0; i < 5; i++)
            {
                spawnTime = 0;
                int randomSpawnIndex = Random.Range(0, spawnPoints.Length); 
                spawnPosition = spawnPoints[randomSpawnIndex].position;
                if (spawnPosition.SquareDistanceToGreaterThan(BaseUnit.Player.CurrentPosition, 3f))
                {
                    break;
                }
            }
            int randomEnemyIndex = Random.Range(0, enemyToSpawn.Length);
            GameObject newEnemy = Instantiate(enemyToSpawn[randomEnemyIndex], spawnPosition, Quaternion.identity);
            newEnemy.transform.SetParent(enemyFolder);
        }
    } //qhar?
}
