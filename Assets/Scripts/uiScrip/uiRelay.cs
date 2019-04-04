using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiRelay : MonoBehaviour
{
    public string towerType;
    public uiManager commandTarget;

    public void SetCurrentTowerType()
    {
        commandTarget.SetCurrentTowerType(towerType);
    }

    public void BuyTower()
    {
        commandTarget.BuyTower();
    }

    public void UpgradeTower()
    {
        commandTarget.UpgradeTower();
    }
}
