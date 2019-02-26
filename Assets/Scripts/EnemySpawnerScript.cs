using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    public EnemyMover Enemy;
    public BasicBaseScript Target;

    private System.Diagnostics.Stopwatch enemyTimer = new System.Diagnostics.Stopwatch();

    public float enemySpawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        this.enemyTimer.Start();
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

        if (enemyTimer.Elapsed.Seconds > this.enemySpawnTimer)
        {
            Vector3 pos = transform.position;
            pos.y -= 1;
            EnemyMover enemyClone = (EnemyMover)Instantiate(Enemy, pos, transform.rotation);
            enemyClone._destination = Target.transform;

            enemyTimer.Reset();
            enemyTimer.Start();
        }
    }
}
