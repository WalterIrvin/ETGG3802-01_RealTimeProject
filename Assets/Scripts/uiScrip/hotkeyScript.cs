using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hotkeyScript : MonoBehaviour
{
    //Menu Bar
    public Button fileButton;
    public Button buildMenu;
    public Button researchMenu;
    public Button settingsMenu;
    public Button pauseButton;
    public Button nextWaveButton;

    //Tower Buttons
    public Button buildTower;
    public Button upgradeTower;
    public Button slow1Upgrade;
    public Button slow2Upgrade;
    public Button slow3Upgrade;
    public Button rapid1Upgrade;
    public Button rapid2Upgrade;
    public Button rapid3Upgrade;

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


        //####OTHER BUTTONS####\\

        //Build a tower on the selected block
        if (Input.GetKeyDown(KeyCode.Q)) { buildTower.onClick.Invoke(); }

        //Upgrade the selected tower (slowing 1)
        if (Input.GetKeyDown(KeyCode.W))
        {
            slow1Upgrade.onClick.Invoke();
            upgradeTower.onClick.Invoke();
        }

        //Upgrade the selected tower (rapid fire 1)
        if (Input.GetKeyDown(KeyCode.E))
        {
            rapid1Upgrade.onClick.Invoke();
            upgradeTower.onClick.Invoke();
        }

        //Upgrade the selected tower (slowing 2)
        if (Input.GetKeyDown(KeyCode.S))
        {
            slow2Upgrade.onClick.Invoke();
            upgradeTower.onClick.Invoke();
        }

        //Upgrade the selected tower (rapid fire 2)
        if (Input.GetKeyDown(KeyCode.D))
        {
            rapid2Upgrade.onClick.Invoke();
            upgradeTower.onClick.Invoke();
        }

        //Upgrade the selected tower (slowing 3)
        if (Input.GetKeyDown(KeyCode.X))
        {
            slow3Upgrade.onClick.Invoke();
            upgradeTower.onClick.Invoke();
        }

        //Upgrade the selected tower (rapid fire 3)
        if (Input.GetKeyDown(KeyCode.C))
        {
            rapid3Upgrade.onClick.Invoke();
            upgradeTower.onClick.Invoke();
        }
    }
}
