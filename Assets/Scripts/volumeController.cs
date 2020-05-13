using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class volumeController : MonoBehaviour
{
    public AudioMixer mixer;

    public void SetMixerLevel (float sliderValue)
    {
        mixer.SetFloat("MixerVolume", Mathf.Log10(sliderValue) * 20);
    }
}
