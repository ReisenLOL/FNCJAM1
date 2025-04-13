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
            spawnTime = 0;
            GameObject newEnemy = Instantiate(enemyToSpawn, enemyFolder);
            int randomIndex = Random.Range(0, spawnPoints.Length);
            newEnemy.transform.position = spawnPoints[randomIndex].position;
        }
    } //qhar?
}
