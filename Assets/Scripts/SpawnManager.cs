using Core.Extensions;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyToSpawn;
    public Transform enemyFolder;
    public Transform[] spawnPoints;
    public float spawnRate;
    public float spawnTime;
    void Start()
    {
        spawnPoints = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        spawnTime += Time.deltaTime;
        if (spawnTime >= spawnRate)
        {
            Vector2 spawnPosition = Vector2.zero;
            for (int i = 0; i < 5; i++)
            {
                spawnTime = 0;
                int randomIndex = Random.Range(0, spawnPoints.Length); 
                spawnPosition = spawnPoints[randomIndex].position;
                if (spawnPosition.SquareDistanceToGreaterThan(BaseUnit.Player.CurrentPosition, 3f))
                {
                    break;
                }
            }
            GameObject newEnemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
            newEnemy.transform.SetParent(enemyFolder);
        }
    } //qhar?
}
