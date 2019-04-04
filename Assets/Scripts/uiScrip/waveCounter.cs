using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveCounter : MonoBehaviour
{
    public WaveHandler waveSpawner;
    public Counter attachedCounter;

    void Update()
    {
        attachedCounter.setCounted(waveSpawner.getWaveNumber());
    }
}
