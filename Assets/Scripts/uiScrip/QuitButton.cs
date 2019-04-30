using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void quitGame()
    {
        Debug.Log("If you are in the editor, the game won't actually quit...");
        Application.Quit();
    }
}
