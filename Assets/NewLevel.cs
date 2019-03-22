using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewLevel : MonoBehaviour
{
    private static int levelNumber = 3;
    public void loadNextLevel()
    {
        levelNumber++;
        SceneManager.LoadScene(levelNumber);
    }
    public void resetLevel()
    {
        levelNumber = 3;
    }
    public void mainMenu()
    {
        resetLevel();
        SceneManager.LoadScene(0);
    }
}
