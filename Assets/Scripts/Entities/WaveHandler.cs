﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveHandler : MonoBehaviour
{
    //public int currentMaxLengthWave = 0;
    private int currentWave = 0;
    private int numWaves;
    private bool isActive = true;
    private bool skipDowntime = false;
    public System.Diagnostics.Stopwatch downTime = new System.Diagnostics.Stopwatch();
    public float waveInterval;
    private List<GameObject> mSpawners = new List<GameObject>();
    public bool PAUSED;

    void Start()
    {
        PAUSED = false;
        mSpawners = new List<GameObject>(GameObject.FindGameObjectsWithTag("Spawner"));
        int tmp = 0;
        int max = 0;

        foreach (GameObject tmpObj in mSpawners)
        {
            tmp = tmpObj.GetComponent<EnemySpawnerScript>().WaveList.Count;
            if (tmp > max)
                max = tmp;
            tmpObj.GetComponent<EnemySpawnerScript>().SpawnWave(currentWave);
        }

        numWaves = max;
        Debug.Log(numWaves);
        //currentMaxLengthWave = checkBiggestWave();
    }

    public void SkipWave()
    {
        if(!isActive)
            skipDowntime = true;
    }

    public int getWaveNumber()
    {
        return currentWave;
    }

    void Update()
    {
        if(PAUSED)
        {
            return;
        }

        if (currentWave >= numWaves)
        {
            //Loading the nextlevel scene
            SceneManager.LoadScene(1);
        }

        if (!isActive && (downTime.Elapsed.Seconds > waveInterval || skipDowntime == true))
        {
            Debug.Log("Next Wave Start");
            skipDowntime = false;
            downTime.Reset();
            // spawn the next wave
            foreach (GameObject tmpObj in mSpawners)
            {
                tmpObj.GetComponent<EnemySpawnerScript>().SpawnWave(currentWave);
            }
            isActive = true;
        }

        else if (isActive)
        {
            bool endWave = true;
            /*foreach (GameObject tmpObj in mSpawners)
            {
                var tmpEnemyScript = tmpObj.GetComponent<EnemySpawnerScript>();
                var tmpWaveList = tmpEnemyScript.WaveList;
                //grabs only wavelists with more then 0 waves in them
                if (tmpWaveList.Count > 0)
                {
                    if ((ThisWaveLen(tmpEnemyScript) >= currentMaxLengthWave) && tmpEnemyScript.waveOver)
                    {
                        endWave = true;
                    }
                }
            }*/

            foreach (GameObject tmpObj in mSpawners)
            {
                if (!tmpObj.GetComponent<EnemySpawnerScript>().waveOver)
                    endWave = false;
            }

            if (endWave)
            {
                Debug.Log("Wave Over handler");
                isActive = false;
                ++currentWave;
                //currentMaxLengthWave = checkBiggestWave();
                downTime.Reset();
                downTime.Start();
            }
        }
    }

    public void pause()
    {
        PAUSED = true;

        if(!isActive)
            downTime.Stop();

        foreach (GameObject tmpObj in mSpawners)
        {
            tmpObj.GetComponent<EnemySpawnerScript>().pause();
        }
    }

    public void unpause()
    {
        PAUSED = false;

        if (!isActive)
            downTime.Start();

        foreach (GameObject tmpObj in mSpawners)
        {
            tmpObj.GetComponent<EnemySpawnerScript>().unpause();
        }
    }

    /*int ThisWaveLen(EnemySpawnerScript obj)
    {
        int x = 0;
        if(currentWave < obj.WaveList.Count)
        {
            x = (obj.WaveList[currentWave].GenericEnemyList.Count + obj.WaveList[currentWave].bossList.Count);
        }
        return x; 
    }*/

    /*public int checkBiggestWave()
    {
        int maxLengthWave = 0;
        for (int i = 0; i < mSpawners.Count - 1; ++i)
        {
            int aLen = 0;
            int bLen = 0;
            var A = mSpawners[i].GetComponent<EnemySpawnerScript>();
            var B = mSpawners[i+1].GetComponent<EnemySpawnerScript>();
            if (currentWave < A.WaveList.Count)
            {
                aLen = A.WaveList[currentWave].GenericEnemyList.Count + A.WaveList[currentWave].bossList.Count;
            }
            if (currentWave < B.WaveList.Count)
            {
                bLen = B.WaveList[currentWave].GenericEnemyList.Count + B.WaveList[currentWave].bossList.Count;
            }

            if (aLen > maxLengthWave || bLen > maxLengthWave)
            {
                maxLengthWave = (aLen > bLen) ? aLen : bLen;
            }
        }
        return maxLengthWave;
    }*/
}
