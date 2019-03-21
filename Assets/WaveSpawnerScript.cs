using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawnerScript : MonoBehaviour
{
    public List<EnemySpawnerScript.mookTier> mookList;
    public List<EnemySpawnerScript.bossTier> bossList;
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
        if(curWave >= 10)
        {
            Debug.Log("You Win, Here's a cookie!");
        }
    }
}
