using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXVolumeUpdater : MonoBehaviour
{
    private VolumeHandlerScript volumeHandler;
    private AudioSource source;
    public float AudioFactor = 1; // for sounds that are louder that they should be

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();

        volumeHandler = GameObject.FindGameObjectsWithTag("Volume")[0].GetComponent<VolumeHandlerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        source.volume = AudioFactor * volumeHandler.max_sfx; ;
    }
}
