using UnityEngine;
using System.Collections;

public class AudioVisualizer : MonoBehaviour
{
    //An AudioSource object so the music can be played
    private AudioSource aSource;
    //A float array that stores the audio samples
    public float[] samples = new float[64];
    //A renderer that will draw a line at the screen
    private LineRenderer lRenderer;
    void Awake()
    {
        //Get and store a reference to the following attached components:
        //AudioSource
        this.aSource = GetComponent<AudioSource>();
        //LineRenderer
        this.lRenderer = GetComponent<LineRenderer>();
    }

    void Start()
    {
        lRenderer.SetVertexCount(64);
    }

    void Update()
    {
        aSource.GetSpectrumData(this.samples, 0, FFTWindow.BlackmanHarris);

        //For each sample
        for (int i = 0; i < samples.Length; i++)
        {   if(i<3)
            lRenderer.SetPosition(i, new Vector3(-65 + i*2, Mathf.Clamp(samples[i] * i * 25, Random.Range(0.5f,1), 75), 100));
            else
            lRenderer.SetPosition(i, new Vector3(-65 + i * 2, Mathf.Clamp(samples[i] * (25 + i * i), Random.Range(0.5f, 1), 350), 100));
        }
    }
}