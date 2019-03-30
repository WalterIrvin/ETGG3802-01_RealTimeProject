using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButtonScript : MonoBehaviour
{

    private bool isPaused;
    public GameObject waveHandlerObject;


    void Start()
    {
        isPaused = false;
    }

    public void PauseGame()
    {
        isPaused = true;
        waveHandlerObject.GetComponent<WaveHandler>().PAUSED = true;
        Time.timeScale = 0;
        Debug.Log("Pausing Game");
    }

    public void UnpauseGame()
    {
        isPaused = false;
        waveHandlerObject.GetComponent<WaveHandler>().PAUSED = false;
        Time.timeScale = 1;
        Debug.Log("UnPausing Game");
    }

    public void PauseButton()
    {
        if(isPaused)
        {
            UnpauseGame();
        }
        else
        {
            PauseGame();
        }
    }
}
