using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour
{
    private TowerData towerData = null;
    private float startTime;

    public Transform TurretHead;
    private Transform main_target;

    private GameObject laserTarget;
    private LineRenderer laserBeam;

    //public float fireDelay = 1f;
    //public float range = 3f;
    //public int towerDamage = 50;
    //public GameObject projecticle_prefab;
    //public GameObject projecticle_slower_prefab;
    
    //public string type = "Base";
    //public Material MAT_RapidFire;
    //public Material MAT_Slow;

    private AudioSource source;

    void Start()
    {
        startTime = Time.fixedTime;
        InvokeRepeating("SearchTarget", 0f, 0.1f);
        source = GetComponent<AudioSource>();
        source.volume = 0.1f;


        laserBeam = GetComponent<LineRenderer>();
        laserBeam.transform.position = transform.position;
        laserBeam.startWidth = 0;
        laserBeam.endWidth = 0;
    }

    void SearchTarget()
    {
        if(towerData == null)
            return;

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

        if (nearestEnemy != null && shortest_dist <= towerData.detectRange)
        {
            main_target = nearestEnemy.transform;
            laserTarget = nearestEnemy;
        } else
        {
            main_target = null;
            laserTarget = null;

            laserBeam.startWidth = 0;
            laserBeam.endWidth = 0;
        }
    }

    public string GetTowerType()
    {
        return towerData.towerType;
    }

    public void SetTowerData(TowerData newData)
    {
        if(newData == null)
            return;

        towerData = newData;
        TurretHead.GetChild(0).GetComponent<MeshRenderer>().material = towerData.towerMaterial;

        if(newData.whatDoesThisShoot == PROJECTILE_TYPE.PROJ_LASER)
        {
            laserBeam.materials[0] = towerData.bulletPrefab.GetComponent<MeshRenderer>().material;
            laserBeam.startColor = laserBeam.endColor = towerData.bulletPrefab.GetComponent<MeshRenderer>().material.color;
        }
    }

    public void Upgrade_RapidFire()
    {
        /*
        GameObject MoneyHandle = GameObject.FindWithTag("Money");
        if (type == "Base" && MoneyHandle.GetComponent<MoneyScript>().Money >= 100)
        {
            MoneyHandle.BroadcastMessage("ChangeMoney", -100);
            fireDelay *= .25f;
            TurretHead.GetChild(0).GetComponent<MeshRenderer>().material = MAT_RapidFire;
            this.GetComponent<MeshRenderer>().material = MAT_RapidFire;
            type = "Rapid";
        }
        else
        {
            Debug.Log("Something went wrong upgrading tower...");
        */
    }

    void Upgrade_Slow()
    {
        /*
        GameObject MoneyHandle = GameObject.FindWithTag("Money");
        if(type == "Base" && MoneyHandle.GetComponent<MoneyScript>().Money >= 100)
        {
            MoneyHandle.BroadcastMessage("ChangeMoney", -100);
            TurretHead.GetChild(0).GetComponent<MeshRenderer>().material = MAT_Slow;
            this.GetComponent<MeshRenderer>().material = MAT_Slow;
            towerDamage /= 2;
            type = "Slow";
        }
        else
        {
            Debug.Log("Something went wrong upgrading tower...");
        }
        */
    }

    void Update()
    {
        if(towerData == null || main_target == null)
            return;

        Vector3 direction = main_target.position - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(direction);
        Vector3 eulerRot = lookRot.eulerAngles;
        TurretHead.rotation = Quaternion.Euler(new Vector3(0f, eulerRot.y, 0f));

        float curTime = Time.fixedTime;
        if (curTime - startTime >= towerData.fireDelay)
        {
            switch (towerData.whatDoesThisShoot)
            {
                case PROJECTILE_TYPE.PROJ_BULLET:
                    GameObject bullet = Instantiate(towerData.bulletPrefab, transform.position, Quaternion.identity);
                    BulletController bullet_script = bullet.GetComponent<BulletController>();
                    bullet_script.mDamage = towerData.bulletDamage;
                    bullet_script.mDestination = main_target.position;

                    bullet_script.bulletEffect = towerData.bulletEffect;
                    bullet_script.effectTimer = towerData.effectTimer;

                    startTime = Time.fixedTime;
                    break;

                case PROJECTILE_TYPE.PROJ_LASER:
                    laserBeam.SetPositions(new Vector3[] { TurretHead.position, laserTarget.transform.position });
                    laserBeam.startWidth = 0.2f;
                    laserBeam.endWidth = 0.2f;

                    if(Equals(laserTarget.tag, "Enemy") || Equals(laserTarget.tag, "Driller"))
                    {
                        if(towerData.bulletEffect != MODIFIER_EFFECT.MOD_NONE)
                        {
                            laserTarget.BroadcastMessage("SetStatus", towerData.bulletEffect);
                            laserTarget.BroadcastMessage("SetStatusTimer", towerData.effectTimer);
                        }

                        laserTarget.BroadcastMessage("dmgHealth", towerData.bulletDamage);
                    }

                    //if (Equals(tmp.tag, "Enemy") || Equals(tmp.tag, "Driller"))

                    break;

                default:
                    break;
            }
        }
        

        /*
        if (main_target == null)
            return;
        Vector3 direction = main_target.position - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(direction);
        Vector3 eulerRot = lookRot.eulerAngles;
        TurretHead.rotation = Quaternion.Euler(new Vector3(0f, eulerRot.y, 0f));

        float curTime = Time.fixedTime;
        if (curTime - startTime >= fireDelay)
        {
            if (type != "Slow")
            {
                GameObject bullet = Instantiate(projecticle_prefab, transform.position, Quaternion.identity);
                BulletController bullet_script = bullet.GetComponent<BulletController>();
                bullet_script.mDamage = towerDamage;
                bullet_script.mDestination = main_target.position;
                startTime = Time.fixedTime;
            }
            else
            {
                GameObject bullet = Instantiate(projecticle_slower_prefab, transform.position, Quaternion.identity);
                SlowController bullet_script = bullet.GetComponent<SlowController>();
                bullet_script.mDamage = towerDamage;
                bullet_script.mDestination = main_target.position;
                startTime = Time.fixedTime;
            }
            source.Play(0);
        }
        */
    }
}
