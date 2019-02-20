using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    public tmp_EnemyMover Enemy;
    public BasicBaseScript Target;
    private System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
    public float spawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        this.timer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (Target.health <= 0)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
                GameObject.Destroy(enemy);

            return;
        }

        if (timer.Elapsed.Seconds > this.spawnTimer)
        {
            Vector3 pos = transform.position;
            pos.y -= 1;
            tmp_EnemyMover enemyClone = (tmp_EnemyMover)Instantiate(Enemy, pos, transform.rotation);
            enemyClone._destination = Target.transform;

            timer.Reset();
            timer.Start();
        }
    }
}
