using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour
{
    private Transform main_target;
    public float fireDelay = 1f;
    public float range = 3f;
    public int towerDamage = 50;
    public GameObject projecticle_prefab;
    private float startTime;

    void Start()
    {
        startTime = Time.fixedTime;
        InvokeRepeating("SearchTarget", 0f, 0.1f);
    }

    void SearchTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");
        float shortest_dist = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach(GameObject enemy in targets)
        {
            float distToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distToEnemy < shortest_dist)
            {
                shortest_dist = distToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortest_dist <= range)
        {
            main_target = nearestEnemy.transform;
        } else
        {
            main_target = null;
        }
    }

    void Update()
    {
        if (main_target == null)
            return;

        if(main_target.gameObject.GetComponent<tmp_EnemyMover>().health <= 0)
        {
            Destroy(main_target.gameObject);
            main_target = null;
            return;
        }

        float curTime = Time.fixedTime;
        if (curTime - startTime >= fireDelay)
        {
            GameObject bullet = Instantiate(projecticle_prefab, transform.position, Quaternion.identity);
            BulletController bullet_script = bullet.GetComponent<BulletController>();
            bullet_script.mDamage = towerDamage;
            bullet_script.mDestination = main_target.position;
            startTime = Time.fixedTime;
        }
    }
}
