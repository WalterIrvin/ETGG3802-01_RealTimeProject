using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    public tmp_EnemyMover Enemy;
    public DrillerScript Driller;
    public BasicBaseScript Target;

    private System.Diagnostics.Stopwatch enemyTimer = new System.Diagnostics.Stopwatch();
    private System.Diagnostics.Stopwatch drillerTimer = new System.Diagnostics.Stopwatch();

    public float enemySpawnTimer;
    public float drillerSpawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        this.enemyTimer.Start();
        this.drillerTimer.Start();
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

        if (drillerTimer.Elapsed.Seconds > this.drillerSpawnTimer)
        {
            Vector3 pos = transform.position;
            pos.y -= 1;
            DrillerScript drillerClone = (DrillerScript)Instantiate(Driller, pos, transform.rotation);

            drillerTimer.Reset();
            drillerTimer.Start();

            enemyTimer.Reset();
            enemyTimer.Start();
        }

        if (enemyTimer.Elapsed.Seconds > this.enemySpawnTimer)
        {
            Vector3 pos = transform.position;
            pos.y -= 1;
            tmp_EnemyMover enemyClone = (tmp_EnemyMover)Instantiate(Enemy, pos, transform.rotation);
            enemyClone._destination = Target.transform;

            enemyTimer.Reset();
            enemyTimer.Start();
        }
    }
}
