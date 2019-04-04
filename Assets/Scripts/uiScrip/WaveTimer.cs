using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTimer : MonoBehaviour
{
    public WaveHandler waveSpawner;
    public Counter attachedCounter;

    void Update()
    {
        if (waveSpawner.GetComponent<WaveHandler>().getIsActive())
        {
            attachedCounter.setCounted(0);
            return;
        }

        float tmp1 = waveSpawner.GetComponent<WaveHandler>().waveInterval;

        float tmp2 = waveSpawner.GetComponent<WaveHandler>().downTime.Elapsed.Seconds;

        float tmp3 = tmp1 - tmp2;

        attachedCounter.setCounted(tmp3);
    }
}
