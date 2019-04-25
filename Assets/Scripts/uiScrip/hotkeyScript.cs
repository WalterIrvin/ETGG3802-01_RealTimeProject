using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hotkeyScript : MonoBehaviour
{
    public Button fileButton;
    public Button buildMenu;
    public Button researchMenu;
    public Button settingsMenu;
    public Button pauseButton;
    public Button nextWaveButton;

    // Update is called once per frame
    void Update()
    {
        //Pause menu (that doesn't pause...)
        if (Input.GetKeyDown(KeyCode.Escape)) { fileButton.onClick.Invoke(); }

        //Open the build menu
        if (Input.GetKeyDown(KeyCode.Alpha1)) { buildMenu.onClick.Invoke(); }

        //Open the research menu
        if (Input.GetKeyDown(KeyCode.Alpha2)) { researchMenu.onClick.Invoke(); }

        //Open the settings menu
        if (Input.GetKeyDown(KeyCode.Alpha3)) { settingsMenu.onClick.Invoke(); }

        //Pause the game
        if (Input.GetKeyDown(KeyCode.Alpha4)) { pauseButton.onClick.Invoke(); }

        //Skip downtime and go to the next wave
        if (Input.GetKeyDown(KeyCode.Alpha5)) { nextWaveButton.onClick.Invoke(); }
    }
}
