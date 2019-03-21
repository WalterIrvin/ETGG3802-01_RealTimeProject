using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{

    [System.Serializable]
    public class mookTier
    {
        public GameObject gobj;
        public float timer;
        public int amount;
    }

    [System.Serializable]
    public class bossTier
    {
        public GameObject gobj;
        public float timer;
        public int amount;
    }

    [System.Serializable]
    public class Wave
    {
        public List<mookTier> GenericEnemyList;
        public List<bossTier> bossList;
    }

    public List<Wave> WaveList;
    public BasicBaseScript Target;

    private System.Diagnostics.Stopwatch enemyTimer = new System.Diagnostics.Stopwatch();
    private System.Diagnostics.Stopwatch bossTimer = new System.Diagnostics.Stopwatch();

    private int enemyIdx = 0;
    private int bossIdx = 0;
    private int enemiesSpawned = 0;
    private int bossesSpawned = 0;
    private int currentWave = 0;

    public bool waveOver = true;
    private bool allowBoss = false;

    // Start is called before the first frame update
    void Start()
    {
        enemyTimer.Start();
        bossTimer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (Target.health <= 0)
        {
            List<GameObject> enemies = new List<GameObject>();

            foreach (GameObject tmpObj in GameObject.FindGameObjectsWithTag("Enemy"))
                enemies.Add(tmpObj);

            foreach (GameObject tmpObj in GameObject.FindGameObjectsWithTag("Driller"))
                enemies.Add(tmpObj);

            foreach (GameObject enemy in enemies)
                GameObject.Destroy(enemy);

            return;
        }

        if (!waveOver)
        {
            if (!allowBoss)
            {
                if (enemyIdx < WaveList[currentWave].GenericEnemyList.Count)
                {
                    mookTier enemy = WaveList[currentWave].GenericEnemyList[enemyIdx];
                    if (enemyTimer.Elapsed.Seconds > enemy.timer)
                    {
                        Vector3 pos = transform.position;
                        pos.y -= 1;
                        GameObject enemyClone = Instantiate(enemy.gobj, pos, transform.rotation);
                        enemyClone.GetComponent<EnemyMover>()._destination = Target.transform;
                        enemyTimer.Reset();
                        enemyTimer.Start();
                        enemiesSpawned++;

                        if (enemiesSpawned >= enemy.amount)
                        {
                            enemiesSpawned = 0;
                            enemyIdx++;
                        }
                    }
                }
                else
                {
                    if (WaveList[currentWave].bossList.Count > 0)
                        allowBoss = true;
                    else
                        waveOver = true;

                    enemyIdx = 0;
                }
            }
            else if (allowBoss)
            {
                if (bossIdx < WaveList[currentWave].bossList.Count)
                {
                    bossTier boss = WaveList[currentWave].bossList[bossIdx];

                    if (bossTimer.Elapsed.Seconds > boss.timer)
                    {
                        Vector3 pos = transform.position;
                        pos.y -= 1;
                        GameObject bossClone = Instantiate(boss.gobj, pos, transform.rotation);
                        bossTimer.Reset();
                        bossTimer.Start();
                        enemyTimer.Reset();
                        enemyTimer.Start();
                        bossesSpawned++;

                        if (bossesSpawned >= boss.amount)
                        {
                            bossesSpawned = 0;
                            bossIdx++;
                        }
                    }
                }
                else
                {
                    waveOver = true;
                    allowBoss = false;
                    bossIdx = 0;
                }
            }
        }
    }

    public void SpawnWave(int wavNum)
    {
        if (wavNum >= WaveList.Count || (WaveList[currentWave].GenericEnemyList.Count == 0 && WaveList[currentWave].bossList.Count == 0))
        {
            waveOver = true;
        }
        else
        {
            Debug.Log("Wave Start");
            waveOver = false;
            currentWave = wavNum;
        }

        if (WaveList[currentWave].GenericEnemyList.Count == 0)
            allowBoss = true;
    }
}
