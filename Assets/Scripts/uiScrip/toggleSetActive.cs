using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggleSetActive : MonoBehaviour
{
    public GameObject toggler;
    public bool startActive = false;
    private AudioSource source;

    void Start()
    {
        source = gameObject.GetComponentInParent(typeof(AudioSource)) as AudioSource;
    }

    public void toggleGameObject()
    {
        source.Play(0);
        startActive = !startActive;
        toggler.SetActive(startActive);
    }
}
