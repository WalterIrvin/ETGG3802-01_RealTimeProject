using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewLevel : MonoBehaviour
{
    private static int levelNumber = 3;
    private AudioSource source;

    void Start()
    {
        source = gameObject.GetComponentInParent(typeof(AudioSource)) as AudioSource;
    }

    public void loadNextLevel()
    {
        source.Play(0);
        levelNumber++;
        SceneManager.LoadScene(levelNumber);
    }
    public void resetLevel()
    {
        levelNumber = 3;
    }
    public void mainMenu()
    {
        source.Play(0);
        resetLevel();
        SceneManager.LoadScene(0);
    }
}
