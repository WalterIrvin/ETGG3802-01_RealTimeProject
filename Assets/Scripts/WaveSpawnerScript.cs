using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ARCHIVED

public class WaveSpawnerScript : MonoBehaviour
{
    [System.Serializable]
    public class waveClass
    {
        public List<EnemySpawnerScript.mookTier> mookList;
        public List<EnemySpawnerScript.bossTier> bossList;
    }

    public List<waveClass> waveList;
    public Text NewWave;
    public Text YouWin;
    public int numToWin = 3;
    public int scalingFactor;
    public int curWave = 0;
    private int curLevelFactor = 1;

    public void newWave()
    {
        NewWave.gameObject.SetActive(true);
        Debug.Log(waveList.Count);
        if(curWave < waveList.Count)
        {
            foreach (EnemySpawnerScript.mookTier mook in waveList[curWave].mookList)
            {
                mook.amount += scalingFactor * curLevelFactor;
            }
            foreach (EnemySpawnerScript.bossTier boss in waveList[curWave].bossList)
            {
                boss.amount += scalingFactor * curLevelFactor;
            }
            curWave++;
        }
    }

    void Update()
    {
        if(curWave > waveList.Count)
        {
            Debug.Log("You Win, Here's a cookie!");
            YouWin.gameObject.SetActive(true);
        }
    }
}
