﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followCamera : MonoBehaviour
{
    //private Camera m_Camera = Camera.main;

    //Orient the camera after all movement is completed this frame to avoid jittering
    void LateUpdate()
    {
        //transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
           // m_Camera.transform.rotation * Vector3.up);
    }
}
