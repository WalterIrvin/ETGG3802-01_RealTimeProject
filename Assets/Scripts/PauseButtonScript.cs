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
    private AudioSource source;

    void Start()
    {
        source = gameObject.GetComponentInParent(typeof(AudioSource)) as AudioSource;
        curImg = this.gameObject.GetComponent<Image>();
        isPaused = false;
    }

    public void PauseGame()
    {
        curImg.sprite = playSprite;
        isPaused = true;
        waveHandlerObject.GetComponent<WaveHandler>().pause();
        Time.timeScale = 0;
        Debug.Log("Pausing Game");
    }

    public void UnpauseGame()
    {
        curImg.sprite = pausedSprite;
        isPaused = false;
        waveHandlerObject.GetComponent<WaveHandler>().unpause();
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
        source.Play(0);
    }
}
