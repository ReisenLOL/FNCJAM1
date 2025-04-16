using Core.Extensions;
using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    public TextMeshProUGUI timerUI;
    public GameObject[] enemyToSpawn;
    public DialogueManager dialogueManager;
    public Transform enemyFolder;
    public Transform[] spawnPoints;
    public int killQuota;
    public int currentKills;
    public float spawnRate;
    public float spawnTime;
    public bool canSpawn = true;

    #region Enemy Spawning Methods
    public void SpawnRandomEnemy()
    {
        Vector2 spawnPosition = Vector2.zero;
        for (int i = 0; i < 5; i++)
        {
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

    public void SpawnRandomEnemyToPosition(Vector2 position)
    {
        int randomEnemyIndex = Random.Range(0, enemyToSpawn.Length);
        GameObject newEnemy = Instantiate(enemyToSpawn[randomEnemyIndex], position, Quaternion.identity);
        newEnemy.transform.SetParent(enemyFolder);
    }

    public void SpawnSpecificEnemyToPosition(GameObject enemy, Vector2 position)
    {
        GameObject newEnemy = Instantiate(enemy, position, Quaternion.identity);
        newEnemy.transform.SetParent(enemyFolder);
    }

    public void SpawnSpecificEnemy(GameObject enemy)
    {
        Vector2 spawnPosition = Vector2.zero;
        for (int i = 0; i < 5; i++)
        {
            int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
            spawnPosition = spawnPoints[randomSpawnIndex].position;
            if (spawnPosition.SquareDistanceToGreaterThan(BaseUnit.Player.CurrentPosition, 3f))
            {
                break;
            }
        }
        GameObject newEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity);
        newEnemy.transform.SetParent(enemyFolder);
    }
    #endregion

    void Start()
    {
        dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        spawnPoints = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();
    }
    void Update()
    {
        TimeSpan timer = TimeSpan.FromSeconds(Time.realtimeSinceStartup);
        if (timer.Seconds < 10)
        {
            timerUI.text = timer.Minutes.ToString() + ":0" + timer.Seconds.ToString();
        }
        else
        {
            timerUI.text = timer.Minutes.ToString() + ":" + timer.Seconds.ToString();
        }
        if (currentKills > killQuota)
        {
            Debug.Log("WOW!");
            dialogueManager.StartDialogue();
            canSpawn = false;
        }
        if (canSpawn)
        {
            spawnTime += Time.deltaTime;
            if (spawnTime >= spawnRate)
            {
                spawnTime = 0;
                SpawnRandomEnemy();
            }
        }
    } //qhar?
}
