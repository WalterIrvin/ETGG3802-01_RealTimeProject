using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiRelay : MonoBehaviour
{
    public string towerType;
    public uiManager commandTarget;
    public TowerButtonResearchTab ResearchChecker;
    private AudioSource source;
    private bool isResearch = false;

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
        //if (!isResearch)
          //  return;
        commandTarget.BuyTower();
        source.Play(0);
    }

    public void UpgradeTower()
    {
        commandTarget.UpgradeTower();
        source.Play(0);
    }
    private void Update()
    {

    }
}
