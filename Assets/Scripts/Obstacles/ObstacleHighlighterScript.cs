using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHighlighterScript : MonoBehaviour
{
    public GameObject selectedBlock;
    public Material oldMaterial;
    public Material highlightMaterial;
    public GameObject TowerPrefab;
    public GameObject theTowerOnThisBlock;
    public bool hasTower = false;
    public bool isTowerUpgraded = false;
    private Vector3 pos;


    private void OnMouseOver()
    {
        GetComponent<Renderer>().material.color = highlightMaterial.color;
    }

    private void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = oldMaterial.color;
    }

    private void OnMouseDown()
    {
        selectedBlock.GetComponent<ChangeSelectedBlock>().currentlySelectedBlock = gameObject;
    }

    public void spawnTowerOnThisBlock()
    {
        Debug.Log("In the SpawnTowerOnThisBlock func");
        pos = transform.position;
        pos.y += 0.325f;
        Debug.Log(pos);
        theTowerOnThisBlock = Instantiate(TowerPrefab, pos, Quaternion.identity) as GameObject;
        //Instantiate(TowerPrefab, pos, Quaternion.identity);
    }

    public void upgradeTowerOnThisBlock()
    {
        theTowerOnThisBlock.GetComponent<TowerScript>().Upgrade_RapidFire();
    }
}
