using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour
{
    public float turnSpeed = 10f;
    private TowerData towerData = null;
    private float startTime;

    public Transform TurretHead;
    private Transform main_target;

    private GameObject laserTarget;
    private LineRenderer laserBeam;

    private AudioSource source;

    void Start()
    {
        OnSpawn();
    }

    public void OnSpawn()
    {
        // Moving the start code here fixes the issues with spawning laser towers //

        startTime = Time.fixedTime;
        InvokeRepeating("SearchTarget", 0f, 0.1f);
        source = GetComponent<AudioSource>();

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

    public Material GetMaterial()
    {
        return towerData.rangeMaterial;
    }

    public Material GetHighlightMaterial()
    {
        return towerData.highlightMaterial;
    }

    public Material GetSelectMaterial()
    {
        return towerData.selectMaterial;
    }

    public float GetRange()
    {
        return towerData.detectRange;
    }

    public void SetTowerData(TowerData newData)
    {
        if(newData == null)
            return;

        OnSpawn();

        towerData = newData;
        TurretHead.GetChild(0).GetComponent<MeshRenderer>().material = towerData.towerMaterial;

        if(newData.whatDoesThisShoot == PROJECTILE_TYPE.PROJ_LASER)
            laserBeam.material = towerData.projectileMaterial;
        
    }

    void Update()
    {
        if(towerData == null || main_target == null)
            return;

        if(towerData.whatDoesThisShoot == PROJECTILE_TYPE.PROJ_LASER)
        {
            TurretHead.transform.LookAt(new Vector3(main_target.position.x, TurretHead.transform.position.y, main_target.position.z));
        }
        else
        {
            Vector3 direction = main_target.position - transform.position;
            Quaternion lookRot = Quaternion.LookRotation(direction);
            Vector3 eulerRot = Quaternion.Lerp(TurretHead.rotation, lookRot, Time.deltaTime * turnSpeed).eulerAngles;
            TurretHead.rotation = Quaternion.Euler(new Vector3(0f, eulerRot.y, 0f));
        }

        float curTime = Time.fixedTime;
        if (curTime - startTime >= towerData.fireDelay)
        {
            switch (towerData.whatDoesThisShoot)
            {
                case PROJECTILE_TYPE.PROJ_BULLET:
                    source.Play(0);
                    GameObject bullet = Instantiate(towerData.bulletPrefab, transform.position, Quaternion.identity);
                    BulletController bullet_script = bullet.GetComponent<BulletController>();
                    bullet_script.GetComponent<MeshRenderer>().material = towerData.selectMaterial;// projectileMaterial;

                    bullet_script.mDamage = towerData.bulletDamage;
                    bullet_script.mDestination = main_target.position;

                    bullet_script.bulletEffect = towerData.bulletEffect;
                    bullet_script.effectTimer = towerData.effectTimer;

                    startTime = Time.fixedTime;
                    break;

                case PROJECTILE_TYPE.PROJ_LASER:
                    laserBeam.SetPositions(new Vector3[] { TurretHead.position, laserTarget.transform.position });
                    laserBeam.startWidth = 0.125f;
                    laserBeam.endWidth = 0.125f;

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
    }
}
