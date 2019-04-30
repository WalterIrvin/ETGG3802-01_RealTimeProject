using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Button mainMenu;
    public Button vsync;
    public Button antiAliasing;

    private Text vsync_text;
    private Text antiAliasing_text;

    // Start is called before the first frame update
    void Start()
    {
        vsync_text = vsync.gameObject.GetComponentInChildren<Text>();
        antiAliasing_text = antiAliasing.gameObject.GetComponentInChildren<Text>();

        vsync_text.text = "Vsync: " + (QualitySettings.vSyncCount > 0);
        antiAliasing_text.text = "Anti-Aliasingc: " + (QualitySettings.antiAliasing > 0);
    }

    public void MainMenu_OnClick()
    {
        SceneManager.LoadScene("menuScreen");
    }

    public void Vsync_OnClick()
    {
        if (QualitySettings.vSyncCount > 0)
            QualitySettings.vSyncCount = 0;
        else
            QualitySettings.vSyncCount = 2;
        vsync_text.text = "Vsync: " + (QualitySettings.vSyncCount > 0);
    }

    public void AntiAliasing_OnClick()
    {
        if (QualitySettings.antiAliasing > 0)
            QualitySettings.antiAliasing = 0;
        else
            QualitySettings.antiAliasing = 2;
        antiAliasing_text.text = "Anti-Aliasing: " + (QualitySettings.antiAliasing > 0);
    }
}
