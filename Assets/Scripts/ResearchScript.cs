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
    public Image fillBar;
    public MoneyScript moneyHandler;
    public Text flavorText;
    public Text statsText;
    private TowerButtonResearchTab buttonObject;
    private float totalTime = 0;
    private bool researching = false;

    public void startResearch(SetActiveResearch researchObj)
    {
        if(researching)
        {
            return;
        }
        TowerButtonResearchTab buttonObj = researchObj.activeResearch;
        if (buttonObj == null)
        {
            return;
        } 
        buttonObject = buttonObj;
        TowerData tower = buttonObj.attachedTower;

        if (tower.researchCost <= moneyHandler.Money && !buttonObj.researching && !buttonObj.isResearched)
        {
            if(buttonObj.preReq != null)
            {
                if(buttonObj.preReq.isResearched)
                {
                    Debug.Log("Researching");
                    moneyHandler.Money -= tower.buildCost;
                    buttonObj.researching = true;
                    researching = true;
                }
            }
            else
            {
                Debug.Log("Researching base tech");
                moneyHandler.Money -= tower.buildCost;
                buttonObj.researching = true;
                researching = true;
            }
        }
    }

    void Update()
    {
        if (buttonObject != null)
        {
            totalTime += Time.deltaTime;
            float tmp = totalTime / buttonObject.attachedTower.researchTime;
            if (tmp >= 1)
            {
                tmp = 1;
                buttonObject.isResearched = true;
                buttonObject.researching = false;
                totalTime = 0;
                buttonObject = null;
                researching = false;
                fillBar.fillAmount = 0;
                return;
            }
            fillBar.fillAmount = tmp;
        }
        
    }
}
