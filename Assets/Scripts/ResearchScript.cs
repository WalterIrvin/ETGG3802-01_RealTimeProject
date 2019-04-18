using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class prependStatsBar
{
   // public string[];
}
public class ResearchScript : MonoBehaviour
{
    public TowerDictionary towerDict;
    public Text flavorText;
    public Text statsText;
    void Start()
    {

    }

    public void startResearch(TowerButtonResearchTab buttonObj)
    {
        TowerData tower = buttonObj.attachedTower;
    }

    void Update()
    {
        
    }
}
