using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenuAccess : MonoBehaviour
{
    public void accessOptionsMenu()
    {
        SceneManager.LoadScene("optionsMenu");
    }
}
