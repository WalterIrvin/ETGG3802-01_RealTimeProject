using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveHandler : MonoBehaviour
{
    private int currentWave = 1;
    public int numWaves;
    private bool isActive = false;
    private System.Diagnostics.Stopwatch downTime = new System.Diagnostics.Stopwatch();
    public float waveInterval;
    private List<GameObject> mSpawners = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject tmpObj in GameObject.FindGameObjectsWithTag("Spawner"))
            mSpawners.Add(tmpObj);

        foreach (GameObject tmpObj in mSpawners)
            tmpObj.GetComponent<EnemySpawnerScript>().SpawnWave(currentWave);

        downTime.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWave > numWaves)
            return; // advance to next level/win the game

        if (!isActive && downTime.Elapsed.Seconds > waveInterval)
        {
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
