using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawnerScript : MonoBehaviour
{
    public List<EnemySpawnerScript.mookTier> mookList;
    public List<EnemySpawnerScript.bossTier> bossList;
    public Text NewWave;
    public Text YouWin;
    public int numToWin = 3;
    public int scalingFactor;
    private int curWave = 1;

    public void addNewMookType(EnemySpawnerScript.mookTier newMook)
    {
        mookList.Add(newMook);
    }

    public void addNewBossType(EnemySpawnerScript.bossTier newBoss)
    {
        bossList.Add(newBoss);
    }

    public void newWave()
    {
        NewWave.gameObject.SetActive(true);
        foreach(EnemySpawnerScript.mookTier mook in mookList)
        {
            mook.amount += scalingFactor * curWave;
        }
        foreach(EnemySpawnerScript.bossTier boss in bossList)
        {
            boss.amount += scalingFactor * curWave;
        }
    }

    void Update()
    {
        if(curWave >= numToWin)
        {
            Debug.Log("You Win, Here's a cookie!");
            YouWin.gameObject.SetActive(true);
        }
    }
}
