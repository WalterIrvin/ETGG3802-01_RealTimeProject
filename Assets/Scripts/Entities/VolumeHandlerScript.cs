using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeHandlerScript : MonoBehaviour
{
    public Slider MusicSlider;
    public Slider SFXSlider;

    public float max_music;
    public float max_sfx;

    // Start is called before the first frame update
    void Start()
    {
        max_music = MusicSlider.value;
        max_sfx = MusicSlider.value;
    }

    // Update is called once per frame
    void Update()
    {
        max_music = MusicSlider.value;
        max_sfx = SFXSlider.value;
    }
}
