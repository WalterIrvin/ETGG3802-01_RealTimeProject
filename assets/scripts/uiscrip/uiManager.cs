using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class uiManager : MonoBehaviour
{

    protected Vector3 pos;
    public GameObject CurrentlySelectedBlck;
    public GameObject TowerPrefab;
    public GameObject MoneyHandler;
    GameObject[] pausedItems;
    protected MoneyScript M;


    // Start is called before the first frame update
    void Start()
    {
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

    public void BuyTower()
    {
        print("Buy Tower Button Pressed");
        bool ifHasATower = CurrentlySelectedBlck.GetComponent<ChangeSelectedBlock>().currentlySelectedBlock.GetComponent<ObstacleHighlighterScript>().hasTower;
        if (!ifHasATower)
        {
            //pos = CurrentlySelectedBlck.GetComponent<ChangeSelectedBlock>().currentlySelectedBlock.transform.position;
            //pos.y += 0.325f;
            //Instantiate(TowerPrefab, pos, Quaternion.identity);
            M.Money -= 100;
            CurrentlySelectedBlck.GetComponent<ChangeSelectedBlock>().currentlySelectedBlock.GetComponent<ObstacleHighlighterScript>().spawnTowerOnThisBlock();
            CurrentlySelectedBlck.GetComponent<ChangeSelectedBlock>().currentlySelectedBlock.GetComponent<ObstacleHighlighterScript>().hasTower = true;
        }
    }

    public void UpgradeTower()
    {
        Debug.Log("Upgrade Tower Button Pressed");
        if (CurrentlySelectedBlck.GetComponent<ChangeSelectedBlock>().currentlySelectedBlock.GetComponent<ObstacleHighlighterScript>().hasTower == true)
        {
            if(CurrentlySelectedBlck.GetComponent<ChangeSelectedBlock>().currentlySelectedBlock.GetComponent<ObstacleHighlighterScript>().isTowerUpgraded == false)
            {
                //if there is a tower, and it isn't upgraded
                //then upgrade it
                Debug.Log("Upgrading tower");
                M.Money -= 150;
                CurrentlySelectedBlck.GetComponent<ChangeSelectedBlock>().currentlySelectedBlock.GetComponent<ObstacleHighlighterScript>().isTowerUpgraded = true;
                CurrentlySelectedBlck.GetComponent<ChangeSelectedBlock>().currentlySelectedBlock.GetComponent<ObstacleHighlighterScript>().upgradeTowerOnThisBlock();
                
            }
            else
            {
                //if there is a tower, and it is upgraded
                //then do nothing
                Debug.Log("I'm already upgraded");
            }
        }
    }
}
