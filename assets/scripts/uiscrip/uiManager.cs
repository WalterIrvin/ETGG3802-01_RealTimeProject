using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class uiManager : MonoBehaviour
{
    public GameMaster gameMaster;
    protected Vector3 pos;
    public GameObject CurrentlySelectedBlck;
    public TowerScript TowerPrefab;
    public GameObject MoneyHandler;
    GameObject[] pausedItems;
    protected MoneyScript M;

    public Text towerCostText;
    private string currentTowerType;
    public string baseTowerType;

    // Start is called before the first frame update
    void Start()
    {
        SetCurrentTowerType("BASE");

        Time.timeScale = 1;
        M = MoneyHandler.GetComponent<MoneyScript>();
        //pausedItems = GameObject.FindGameObjectsWithTag("ShowOnPause");
        hidePaused();
    }

    // Update is called once per frame
    void Update()
    {
        //showPaused();
        //if(Input.GetKeyDown(KeyCode.P))
        //{
        //showPaused();
        //if(Time.timeScale == 1)
        //{
        //Time.timeScale = 0;
        //showPaused();
        //} else if(Time.timeScale == 0)
        //{
        //Time.timeScale = 1;
        //hidePaused();
        //}
        //}    
    }

    public void pauseControl()
    {
        showPaused();
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            showPaused();
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            hidePaused();
        }
    }

    void showPaused()
    {
        //foreach (GameObject g in pausedItems)
        //{
        //    g.SetActive(true);
        //}
    }

    void hidePaused()
    {
        //foreach(GameObject g in pausedItems)
        //{
        //    g.SetActive(false);
        //}
    }

    public void loadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void SetCurrentTowerType(string newType)
    {
        currentTowerType = newType;

        TowerData towerData = TowerDictionary.GetTowerData(newType);
        if (towerData != null)
        {
            towerCostText.text = "Cost: " + towerData.buildCost.ToString();
            CurrentlySelectedBlck.GetComponent<ChangeSelectedBlock>().currentlySelectedBlock.GetComponent<ObstacleHighlighterScript>().towerInfo.UpdateUIText(newType);
        }
        else
            towerCostText.text = "";

    }

    //public void BuyTower(string whichType)
    public void BuyTower()
    {
        print("Buy Tower Button Pressed");

        //if (currentTowerType == "NONE")
        currentTowerType = baseTowerType;

        if (TowerDictionary.ContainsTowerType(currentTowerType))
        {
            ObstacleHighlighterScript ohs = CurrentlySelectedBlck.GetComponent<ChangeSelectedBlock>().currentlySelectedBlock.GetComponent<ObstacleHighlighterScript>();
            if (!ohs.hasTower)
            {
                int buyCost, valueThatHasNoUseHere;
                TowerDictionary.GetValueTotals(currentTowerType, out buyCost, out valueThatHasNoUseHere);

                if (M.Money >= buyCost)
                {
                    //CurrentlySelectedBlck.GetComponent<ChangeSelectedBlock>().currentlySelectedBlock.GetComponent<ObstacleHighlighterScript>().spawnTowerOnThisBlock(whichType);

                    // This code should go in ObstacleHighlighterScript.spawnTowerOnThisBlock(), but it doesn't work there... //
                    ohs.towerOnThisBlock = Instantiate(TowerPrefab, new Vector3(ohs.transform.position.x, ohs.transform.position.y + 0.325f, ohs.transform.position.z), Quaternion.identity);
                    ohs.towerOnThisBlock.SetTowerData(TowerDictionary.GetTowerData(currentTowerType));
                    ohs.hasTower = true;
                    // ====================================================================================================== //

                    M.Money -= buyCost;
                    ohs.UpdateUIText();
                }
                else
                    print("Not enough money!");
            }
            //else
            //    UpgradeTower();
        }
        else
        {
            print("Tried to buy a tower of a type that doesn't exist!");
        }

        SetCurrentTowerType("BASE");
    }

    //public void UpgradeTower(string whichType)
    public void UpgradeTower()
    {
        print("Upgrade Tower Button Pressed");

        ObstacleHighlighterScript ohs = CurrentlySelectedBlck.GetComponent<ChangeSelectedBlock>().currentlySelectedBlock.GetComponent<ObstacleHighlighterScript>();
        if (ohs.hasTower)
        {
            if (TowerDictionary.ContainsTowerType(currentTowerType))
            {
                string currentType = ohs.towerOnThisBlock.GetTowerType();
                if(TowerDictionary.IsValidUpgrade(currentType, currentTowerType, true))
                {
                    int currentCost, upgradeCost, uselessValue1, uselessValue2;
                    TowerDictionary.GetValueTotals(currentType, out currentCost, out uselessValue1);
                    TowerDictionary.GetValueTotals(currentTowerType, out upgradeCost, out uselessValue2);

                    if(M.Money >= upgradeCost - currentCost)
                    {
                        ohs.towerOnThisBlock.SetTowerData(TowerDictionary.GetTowerData(currentTowerType));
                        M.Money -= upgradeCost - currentCost;
                        ohs.UpdateUIText();
                    }
                    else
                        print("Not enough money!");

                }
                else
                    print(currentTowerType + " is not a valid upgrade for " + currentType + "!");
            }
            else
                print("Tried to upgrade a tower to a tower type that doesn't exist!");
        }
        else
            print("No tower to upgrade...");

        SetCurrentTowerType("BASE");
    }
}
