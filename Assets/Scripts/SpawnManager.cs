using Bremsengine;
using Core.Extensions;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    public TextMeshProUGUI timerUI;
    public GameObject[] enemyToSpawn;
    public GameObject miniBossToSpawn;
    public Transform enemyFolder;
    public Transform[] spawnPoints;
    int killQuota;
    public int KillQuota => killQuota;
    public void AddKillQuota(int quota) => killQuota += quota;
    public int currentKills;
    private float gameTimer;
    public float spawnRate;
    public float spawnTime;
    public static bool CanSpawn => !Dialogue.IsDialogueRunning && !GeneralManager.IsPaused; // leaving in some dummy values.
    public List<SpawnPhase> enemyPhases;
    private SpawnPhase currentPhase;
    public Dialogue KillQuotaDummyDialogue;

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
    #region Enemy Phase Spawning
    [System.Serializable]
    public class SpawnPhase
    {
        public float startTime;
        public float spawnRate;
        public GameObject[] enemyUnits;
    }
    void SpawnEnemyFromPhase(SpawnPhase phase)
    {
        int index = Random.Range(0, phase.enemyUnits.Length);
        GameObject enemy = phase.enemyUnits[index];
        SpawnSpecificEnemy(enemy);
    }
    #endregion


    void Start()
    {
        InvokeRepeating("SpawnMiniBoss", 60f, 60f);
        spawnPoints = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();
    }
    void Update()
    {
        gameTimer += Time.deltaTime;
        for (int i = 0; i < enemyPhases.Count; i++)
        {
            if (gameTimer >= enemyPhases[i].startTime)
            {
                currentPhase = enemyPhases[i];
            }
        }
        TimeSpan timer = TimeSpan.FromSeconds(Time.time);
        if (timer.Seconds < 10)
        {
            timerUI.text = timer.Minutes.ToString() + ":0" + timer.Seconds.ToString();
        }
        else
        {
            timerUI.text = timer.Minutes.ToString() + ":" + timer.Seconds.ToString();
        }
        bool killQuotaCondition = currentKills > KillQuota;
        if (killQuotaCondition)
        {
            KillQuotaDummyDialogue.StartDialogue();
            Debug.Log("WOW! cool dialogue stuff kill quota lmao");
        }
        if (CanSpawn)
        {
            spawnTime += Time.deltaTime;
            if (spawnTime >= spawnRate)
            {
                spawnTime = 0;
                SpawnEnemyFromPhase(currentPhase);
            }
        }
    } //qhar?
    private void SpawnMiniBoss()
    {
        if (CanSpawn)
        {
            SpawnSpecificEnemy(miniBossToSpawn);
        }
    }
}
