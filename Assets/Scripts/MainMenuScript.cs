using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public GameObject menuScreen;
    public GameObject creditScene;

    public void doCredits()
    {
        menuScreen.SetActive(false);
        creditScene.SetActive(true);
    }

    public void doMenu()
    {
        Debug.Log("clicked go back");
        menuScreen.SetActive(true);
        creditScene.SetActive(false);
    }

    public void doQuit()
    {
        Application.Quit();
    }
}

