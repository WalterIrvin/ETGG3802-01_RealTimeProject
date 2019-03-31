using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class loadGameOnClick : MonoBehaviour
{
    private AudioSource source;

    void Start()
    {
        source = gameObject.GetComponentInParent(typeof(AudioSource)) as AudioSource;
    }

    public void loadByIndex(int sceneIndex)
    {
        source.Play(0);
        SceneManager.LoadScene(sceneIndex);
    }
}
