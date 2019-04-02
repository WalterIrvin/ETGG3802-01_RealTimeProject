using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextWaveButton : MonoBehaviour
{
    public GameObject spawner;

    // Update is called once per frame
    void NextWave()
    {
        spawner.BroadcastMessage("SkipWave");
        Debug.Log("Spawning next wave");
    }
}
