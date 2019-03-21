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


    public WaveSpawnerScript spawnController;
    public BasicBaseScript Target;

    private System.Diagnostics.Stopwatch enemyTimer = new System.Diagnostics.Stopwatch();
    private System.Diagnostics.Stopwatch bossTimer = new System.Diagnostics.Stopwatch();

    private int enemyIdx = 0;
    private int bossIdx = 0;
    private int enemiesSpawned = 0;
    private int bossesSpawned = 0;
    private bool allowBoss = false;

    // Start is called before the first frame update
    void Start()
    {
        enemyTimer.Start();
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
        if(!allowBoss)
        {
            if (enemyIdx < spawnController.mookList.Count)
            {
                mookTier enemy = spawnController.mookList[enemyIdx];
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
                enemyIdx = 0;
                allowBoss = true;
                bossTimer.Reset();
                bossTimer.Start();
            }
        }
        else if(allowBoss)
        {
            if (bossIdx < spawnController.bossList.Count)
            {
                bossTier boss = spawnController.bossList[bossIdx];

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
                bossIdx = 0;
                allowBoss = false;
                spawnController.newWave();
            }
        }
    }
}
