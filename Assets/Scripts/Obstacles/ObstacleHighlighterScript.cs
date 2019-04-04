using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHighlighterScript : MonoBehaviour
{
    public uiTowerInfo towerInfo;
    public GameObject selectedBlock;
    public Material oldMaterial;
    public Material highlightMaterial;
    //***public GameObject TowerPrefab;
    //***public GameObject theTowerOnThisBlock;

    public TowerScript towerPrefab;
    public TowerScript towerOnThisBlock;
    public bool hasTower = false;
    
    //public bool isTowerUpgraded = false;
    private Vector3 pos;

    private bool mouseIsOverBlock = false;
    private ChangeSelectedBlock blockSelectorScript;
    
    void Start()
    {
        blockSelectorScript = selectedBlock.GetComponent<ChangeSelectedBlock>();
        InvokeRepeating("CheckHighlight", 0f, 0.1f);
    }
    void CheckHighlight()
    {
        if (blockSelectorScript.currentlySelectedBlock != this.gameObject && !mouseIsOverBlock)
        {
            GetComponent<Renderer>().material.color = oldMaterial.color;
        }
    }
    private void OnMouseOver()
    {
        mouseIsOverBlock = true;
        GetComponent<Renderer>().material.color = highlightMaterial.color;
    }

    private void OnMouseExit()
    {
        mouseIsOverBlock = false;
        GetComponent<Renderer>().material.color = oldMaterial.color;
    }

    private void OnMouseDown()
    {
        selectedBlock.GetComponent<ChangeSelectedBlock>().currentlySelectedBlock = gameObject;
        UpdateUIText();
    }

    public void UpdateUIText()
    {
        if (hasTower)
            towerInfo.UpdateUIText(towerOnThisBlock.GetTowerType());
        else
            towerInfo.UpdateUIText("NONE");
    }

    public void spawnTowerOnThisBlock(string towerType)
    {
        /*
        Debug.Log("In the SpawnTowerOnThisBlock func");
        pos = transform.position;
        pos.y += 0.325f;
        Debug.Log(pos);
        */

        //***theTowerOnThisBlock = Instantiate(TowerPrefab, pos, Quaternion.identity) as GameObject;
        //towerOnThisBlock = Instantiate(towerPrefab, pos, Quaternion.identity);
        //towerOnThisBlock.SetTowerData(TowerDictionary.GetTowerData(towerType)); // The uiManager has a check to make sure this won't be null.
        //hasTower = true;

        //Instantiate(TowerPrefab, pos, Quaternion.identity);
    }

    public void upgradeTowerOnThisBlock()
    {
        //***theTowerOnThisBlock.GetComponent<TowerScript>().Upgrade_RapidFire();
    }
}
