using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    public int mapX, mapZ;

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
    private BasicBaseScript Target;
    //public WaveHandler waveController;

    private System.Diagnostics.Stopwatch enemyTimer = new System.Diagnostics.Stopwatch();
    private System.Diagnostics.Stopwatch bossTimer = new System.Diagnostics.Stopwatch();

    private int enemyIdx = 0;
    private int bossIdx = 0;
    private int enemiesSpawned = 0;
    private int bossesSpawned = 0;
    private int currentWave = 0;

    public bool waveOver = false;
    private bool allowBoss = false;

    // Start is called before the first frame update
    void Start()
    {
        enemyTimer.Start();
        bossTimer.Start();

        List<GameObject> tmp = new List<GameObject>(GameObject.FindGameObjectsWithTag("Base"));

        Target = tmp[0].GetComponent<BasicBaseScript>();
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
                    {
                        allowBoss = true;
                        bossTimer.Reset();
                        bossTimer.Start();
                        enemyIdx = 0;
                    }
                    else if (WaveOverCheck())
                    {
                        enemyIdx = 0;
                    }
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
                    if (WaveOverCheck())
                    {
                        allowBoss = false;
                        bossIdx = 0;
                        enemyTimer.Reset();
                        enemyTimer.Start();
                    }
                }
            }
        }
    }


    public void SpawnWave(int wavNum)
    {
        currentWave = wavNum;
        if (WaveList.Count > currentWave)
        {
            if (WaveList[currentWave].GenericEnemyList.Count == 0 && WaveList[currentWave].bossList.Count == 0)
            {
                waveOver = true;
                return;
            }

            if (WaveList[currentWave].GenericEnemyList.Count == 0)
            {
                allowBoss = true;
            }

            Debug.Log("Wave Start");
            waveOver = false;
        }
        else if (WaveList.Count <= currentWave)
        {
            waveOver = true;
            return;
        }
    }

    private bool WaveOverCheck()
    {
        List<GameObject> enemies = new List<GameObject>();

        foreach (GameObject tmpObj in GameObject.FindGameObjectsWithTag("Enemy"))
            enemies.Add(tmpObj);

        foreach (GameObject tmpObj in GameObject.FindGameObjectsWithTag("Driller"))
            enemies.Add(tmpObj);

        if (enemies.Count == 0) {
            Debug.Log("Wave Over");
            waveOver = true; }

        else
            waveOver = false;

        return waveOver;
    }

    public void pause()
    {
        if (!waveOver)
        {
            enemyTimer.Stop();
            bossTimer.Stop();
        }
    }

    public void unpause()
    {
        if (!waveOver)
        {
            enemyTimer.Start();
            bossTimer.Start();
        }
    }
}
