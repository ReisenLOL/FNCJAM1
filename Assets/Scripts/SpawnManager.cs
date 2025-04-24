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
    public bool hasStartedDialogue = false;
    public int KillQuota => killQuota;
    public void AddKillQuota(int quota) => killQuota += quota;
    public int currentKills;
    public float gameTimer;
    public float spawnRate;
    public float spawnTime;

    private bool bossSpawned = false;
    private GameObject currentBoss;
    public static bool CanSpawn => !Dialogue.IsDialogueRunning && !GeneralManager.IsPaused; // leaving in some dummy values.
    public List<SpawnPhase> enemyPhases;
    private SpawnPhase currentPhase;
    public Dialogue bossDialogue;
    public Dialogue postFightDialogue;
    public float healthModifier = 1f;
    public float healthScalingValue;

    #region Enemy Spawning Methods

    public void RecalculateHealthModifier(int level)
    {
        healthModifier = 1f + (level * healthScalingValue);
    }
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
        BaseUnit enemyStats = newEnemy.GetComponent<BaseUnit>();
        float pastMaxHealth = enemyStats.MaxHealth;
        enemyStats.MaxHealth *= healthModifier;
        enemyStats.ChangeHealth(enemyStats.MaxHealth - pastMaxHealth);
        newEnemy.transform.SetParent(enemyFolder);
    }

    public void SpawnRandomEnemyToPosition(Vector2 position)
    {
        int randomEnemyIndex = Random.Range(0, enemyToSpawn.Length);
        GameObject newEnemy = Instantiate(enemyToSpawn[randomEnemyIndex], position, Quaternion.identity);
        BaseUnit enemyStats = newEnemy.GetComponent<BaseUnit>();
        float pastMaxHealth = enemyStats.MaxHealth;
        enemyStats.MaxHealth *= healthModifier;
        enemyStats.ChangeHealth(enemyStats.MaxHealth - pastMaxHealth);
        newEnemy.transform.SetParent(enemyFolder);
    }

    public void SpawnSpecificEnemyToPosition(GameObject enemy, Vector2 position)
    {
        GameObject newEnemy = Instantiate(enemy, position, Quaternion.identity);
        BaseUnit enemyStats = newEnemy.GetComponent<BaseUnit>();
        float pastMaxHealth = enemyStats.MaxHealth;
        enemyStats.MaxHealth *= healthModifier;
        enemyStats.ChangeHealth(enemyStats.MaxHealth - pastMaxHealth);
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
        BaseUnit enemyStats = newEnemy.GetComponent<BaseUnit>();
        float pastMaxHealth = enemyStats.MaxHealth;
        enemyStats.MaxHealth *= healthModifier;
        enemyStats.ChangeHealth(enemyStats.MaxHealth - pastMaxHealth);
        newEnemy.transform.SetParent(enemyFolder);
    }
    #endregion
    #region Enemy Phase Spawning
    [System.Serializable]
    public class SpawnPhase
    {
        public float startTime;
        public float spawnRate;
        public bool isBossPhase;
        public GameObject[] enemyUnits;
    }
    void SpawnEnemyFromPhase(SpawnPhase phase)
    {
        int index = Random.Range(0, phase.enemyUnits.Length);
        GameObject enemy = phase.enemyUnits[index];
        SpawnSpecificEnemy(enemy);
    }
    void SpawnBoss(SpawnPhase phase)
    {
        GameObject boss = phase.enemyUnits[0];
        Vector2 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        currentBoss = Instantiate(boss, spawnPosition, Quaternion.identity);
        currentBoss.transform.SetParent(enemyFolder);
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
        float elapsed = Time.time - Timer.instance.gameStartTime;
        TimeSpan timer = TimeSpan.FromSeconds(elapsed);
        if (timer.Seconds < 10)
        {
            timerUI.text = timer.Minutes.ToString() + ":0" + timer.Seconds.ToString();
        }
        else
        {
            timerUI.text = timer.Minutes.ToString() + ":" + timer.Seconds.ToString();
        }
        if (CanSpawn && !currentPhase.isBossPhase)
        {
            spawnTime += Time.deltaTime;
            if (spawnTime >= spawnRate)
            {
                spawnTime = 0;
                SpawnEnemyFromPhase(currentPhase);
            }
        }
        if (currentPhase.isBossPhase)
        {
            if (!bossSpawned)
            {
                bossDialogue.StartDialogue();
                SpawnBoss(currentPhase);
                bossSpawned = true;
            }
        }
        if (bossSpawned && currentBoss.activeSelf == false)
        {
            if (!hasStartedDialogue)
            {
                postFightDialogue.StartDialogue();
                hasStartedDialogue = true;
            }
        }
    } //qhar?
    private void SpawnMiniBoss()
    {
        if (CanSpawn && !currentPhase.isBossPhase)
        {
            SpawnSpecificEnemy(miniBossToSpawn);
        }
    }
}
