using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveHandler : MonoBehaviour
{
    private int currentWave = 0;
    private int numWaves;
    private bool isActive = false;
    private System.Diagnostics.Stopwatch downTime = new System.Diagnostics.Stopwatch();
    public float waveInterval;
    private List<GameObject> mSpawners = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject tmpObj in GameObject.FindGameObjectsWithTag("Spawner"))
            mSpawners.Add(tmpObj);

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

        downTime.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWave >= numWaves)
            return; // advance to next level/win the game

        if (!isActive && downTime.Elapsed.Seconds > waveInterval)
        {
            Debug.Log("Next Wave Start");
            // spawn the next wave
            foreach (GameObject tmpObj in mSpawners)
                tmpObj.GetComponent<EnemySpawnerScript>().SpawnWave(currentWave);

            isActive = true;
        }

        foreach (GameObject tmpObj in mSpawners)
        {
            if (tmpObj.GetComponent<EnemySpawnerScript>().waveOver && isActive)
            {
                Debug.Log("Wave Over");
                isActive = false;
                currentWave += 1;

                downTime.Reset();
                downTime.Start();
            }
        }
    }
}
