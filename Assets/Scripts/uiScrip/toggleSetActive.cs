using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggleSetActive : MonoBehaviour
{
    public GameObject toggler;
    public bool startActive = false;

    public void toggleGameObject()
    {
        startActive = !startActive;
        toggler.SetActive(startActive);
    }
}
