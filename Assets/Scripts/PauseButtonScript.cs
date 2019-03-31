using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PauseButtonScript : MonoBehaviour
{

    private bool isPaused;
    public GameObject waveHandlerObject;
    public Sprite pausedSprite;
    public Sprite playSprite;
    private Image curImg;
    void Start()
    {
        curImg = this.gameObject.GetComponent<Image>();
        isPaused = false;
    }

    public void PauseGame()
    {
        curImg.sprite = playSprite;
        isPaused = true;
        waveHandlerObject.GetComponent<WaveHandler>().PAUSED = true;
        Time.timeScale = 0;
        Debug.Log("Pausing Game");
    }

    public void UnpauseGame()
    {
        curImg.sprite = pausedSprite;
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
