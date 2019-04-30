using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FileMenuScript : MonoBehaviour
{
    public void newGame()
    {
        SceneManager.LoadScene("menuScreen");
    }

    public void quitGame()
    {
        Debug.Log("If you are running this in the editor, nothing will happen.");
        Application.Quit();
    }
}
