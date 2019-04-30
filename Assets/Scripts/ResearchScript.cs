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
    public float totalTime = 0;
    private bool researching = false;
    private TowerData towerToResearch;
    public bool otherOption;

    public void startResearch(SetActiveResearch researchObj)
    {
        if(researching)
            return;        

        TowerButtonResearchTab buttonObj = researchObj.activeResearch;
        if(buttonObj == null)
            return;
         
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

    public void SetValuesWithSave(string whichType, float currentTime)
    {
        if(whichType == "NONE")
            researching = false;
        else
        {
            SetTowerToResearch(whichType);
            if(towerToResearch == null || TowerDictionary.GetResearchStatus(towerToResearch.towerType) || !TowerDictionary.ResearchPreReqComplete(towerToResearch.towerType))
                return;

            researching = true;
            totalTime = currentTime;
        }
    }

    public void GetValuesForSave(out string whichType, out float currentTime)
    {
        if(!researching)
        {
            whichType = "NONE";
            currentTime = 0;
        }
        else
        {
            whichType = towerToResearch.towerType;
            currentTime = totalTime;
        }
    }

    public void SetTowerToResearch(string whichType)
    {
        if(researching)
            return;

        towerToResearch = TowerDictionary.GetTowerData(whichType);
    }

    public void StartResearch_()
    {
        if(researching || towerToResearch == null || TowerDictionary.GetResearchStatus(towerToResearch.towerType) || !TowerDictionary.ResearchPreReqComplete(towerToResearch.towerType) || moneyHandler.Money < towerToResearch.researchCost)
            return;

        moneyHandler.Money -= towerToResearch.researchCost;
        researching = true;
    }

    public void SpeedUpResearch()
    {

    }

    void Update()
    {
        if(otherOption)
        {
            Update_();
            return;
        }

        if(buttonObject != null)
        {
            totalTime += Time.deltaTime;
            float tmp = totalTime / buttonObject.attachedTower.researchTime;
            if (tmp >= 1)
            {
                tmp = 1;
                buttonObject.isResearched = true;
                buttonObject.researching = false;
                totalTime = 0;
                TowerDictionary.SetResearch(buttonObject.attachedTower.towerType, true);
                buttonObject = null;
                researching = false;
                fillBar.fillAmount = 0;
            }
            else
                fillBar.fillAmount = tmp;
        }
    }

    private void Update_()
    {
        if(researching)
        {
            totalTime += Time.deltaTime;
            float tmp = totalTime / towerToResearch.researchTime;
            if(tmp >= 1)
            {
                tmp = 1;
                totalTime = 0;
                fillBar.fillAmount = 0;

                TowerDictionary.SetResearch(towerToResearch.towerType, true);
                towerToResearch = null;
                researching = false;
            }
            else
                fillBar.fillAmount = tmp;
        }
    }
}
