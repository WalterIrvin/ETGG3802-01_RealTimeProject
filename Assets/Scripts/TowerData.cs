using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerData : MonoBehaviour
{
    public string towerName;
    public string towerDesc;
    public string towerType;
    public string prevTowerType;
    public int buildCost;
    public int refundAmount;
    public Material towerMaterial;
    public PROJECTILE_TYPE whatDoesThisShoot;
    public GameObject bulletPrefab;
    public float fireDelay;
    public float detectRange;
    public int bulletDamage;
    public MODIFIER_EFFECT bulletEffect;
    public float effectTimer;
    //public List<EntityModifier> bulletEffects;
}
