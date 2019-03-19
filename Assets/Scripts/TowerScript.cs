using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour
{
    public Transform TurretHead;
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
        List<GameObject> targets = new List<GameObject>(); 

        foreach (GameObject tmpObj in GameObject.FindGameObjectsWithTag("Enemy"))
            targets.Add(tmpObj);

        foreach (GameObject tmpObj in GameObject.FindGameObjectsWithTag("Driller"))
            targets.Add(tmpObj);

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
        Vector3 direction = main_target.position - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(direction);
        Vector3 eulerRot = lookRot.eulerAngles;
        TurretHead.rotation = Quaternion.Euler(new Vector3(0f, eulerRot.y, 0f));

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
