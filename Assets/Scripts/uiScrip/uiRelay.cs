using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiRelay : MonoBehaviour
{
    public string towerType;
    public uiManager commandTarget;
    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void SetCurrentTowerType()
    {
        commandTarget.SetCurrentTowerType(towerType);
    }

    public void BuyTower()
    {
        commandTarget.BuyTower();
        source.Play(0);
    }

    public void UpgradeTower()
    {
        commandTarget.UpgradeTower();
        source.Play(0);
    }
}
