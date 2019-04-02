using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiRelay : MonoBehaviour
{
    public uiManager commandTarget;

    public void SetCurrentTowerType(string newType)
    {
        commandTarget.SetCurrentTowerType(newType);
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
