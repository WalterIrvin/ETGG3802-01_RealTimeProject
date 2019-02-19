using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    public tmp_EnemyMover Enemy;
    private System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
    float spawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        this.timer.Start();
        this.spawnTimer = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.Elapsed.Seconds > this.spawnTimer)
        {
            Vector3 pos = transform.position;
            pos.z += 2;
            tmp_EnemyMover enemyClone = (tmp_EnemyMover)Instantiate(Enemy, pos, transform.rotation);
            enemyClone._destination = GameObject.FindWithTag("Base").transform;

            timer.Reset();
            timer.Start();
        }
    }
}
