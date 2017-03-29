using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MKGlowSystem;
//--#pragma warning disable 618 // Disable obsolete warning for spectrum class

public class Spectrum : MonoBehaviour
{
    //private float average = 0;
    public GameObject playerMesh;
    public FFTWindow window;
    public float[] spectrum;
    public MKGlow glow;
    public float sum = 0;
    private int sumFactor;


    void Start()
    {
        glow = this.gameObject.GetComponent<MKGlow>();
    }

    void FixedUpdate()
    {
        sum = 0;
        AudioListener.GetSpectrumData(spectrum, 0, window);
        for (int i = 0; i < 1024; i++)
        {
            sum += spectrum[i];
        }

        //playerMesh.GetComponent<Renderer>().material.SetColor("_MKGlowColor", Color.red);
        glow.GlowIntensity = Mathf.Clamp(sum * 2, 0.1f, 0.375f);
    }
}
