using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerData : MonoBehaviour
{
    public TOWER_TYPE towerType;
    public Material towerMaterial;
    public PROJECTILE_TYPE whatDoesThisShoot;
    public GameObject bulletPrefab;
    public float fireDelay;
    public float detectRange;
    public int bulletDamage;
    public TOWER_TYPE prevTowerLevel;
    public int buildCost;
    public int refundAmount;
    public List<EntityModifier> bulletEffects;
}
