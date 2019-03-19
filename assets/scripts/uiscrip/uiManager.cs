using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class uiManager : MonoBehaviour
{
    GameObject[] pausedItems;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        pausedItems = GameObject.FindGameObjectsWithTag("ShowOnPause");
        hidePaused();
    }

    // Update is called once per frame
    void Update()
    {
        showPaused();
        if(Input.GetKeyDown(KeyCode.P))
        {
            //showPaused();
            if(Time.timeScale == 1)
            {
                Time.timeScale = 0;
                showPaused();
            } else if(Time.timeScale == 0)
            {
                Time.timeScale = 1;
                hidePaused();
            }
        }
    }

    public void pauseControl()
    {
        showPaused();
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            showPaused();
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            hidePaused();
        }
    }

    void showPaused()
    {
        foreach (GameObject g in pausedItems)
        {
            g.SetActive(true);
        }
    }

    void hidePaused()
    {
        foreach(GameObject g in pausedItems)
        {
            g.SetActive(false);
        }
    }

    public void loadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
