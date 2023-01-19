using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class audio : MonoBehaviour
{
    public AudioMixer mixer;
    public string parameterName;
    float curVol;
    Slider myBeautifulSlider;
    public float min = 0.0001f;      // Decibels
    public float max = 1;


    private void Start()
    {
        myBeautifulSlider = GetComponent<Slider>();

        myBeautifulSlider.minValue = min;
        myBeautifulSlider.maxValue = max;

        mixer.GetFloat(parameterName,out curVol);

        myBeautifulSlider.value = Mathf.Pow(10, curVol / 20);
        Debug.Log("vol = " + curVol);

    }

    public void changeAudio() {

        mixer.SetFloat(parameterName, Mathf.Log10(myBeautifulSlider.value) * 20);
        mixer.GetFloat(parameterName, out curVol);
        Debug.Log("new Volume" + curVol);
    }


}

