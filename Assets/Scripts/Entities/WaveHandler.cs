using System.Collections;
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

    public void SkipWave()
    {
        if (!isActive)
            skipDowntime = true;
    }

    public int getWaveNumber()
    {
        return currentWave + 1;
    }

    public bool getIsActive()
    {
        return isActive;
    }
}
