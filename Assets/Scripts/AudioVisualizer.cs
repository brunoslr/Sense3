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
    public Material lineMat;
    public int segment;
    Color LineColor;
    float x;
    float y;
    float z = 0f;
    float angle = 0f;
    void Awake()
    {
        //Get and store a reference to the following attached components:
        //AudioSource
        //this.aSource = GetComponent<AudioSource>();
        //LineRenderer
        lRenderer = GetComponent<LineRenderer>();

    }

    void Start()
    {
        lRenderer.SetVertexCount(segment + 1);
        lRenderer.useWorldSpace = false;
        lineMat = this.gameObject.GetComponent<LineRenderer>().material;
    }

    void Update()
    {
        
    }

    public void PlayerVisualizer(float radius)
    {
        lRenderer.enabled = true;
        CreatePoints(radius);
    }
    public void StopVisualizer()
    {
        lRenderer.enabled = false;

    }
    public void CreatePoints(float radius)
    {
        AudioListener.GetSpectrumData(this.samples, 0, FFTWindow.BlackmanHarris);
       
        for (int i = 0; i < (segment + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            lRenderer.SetPosition(i, new Vector3(x, samples[Random.Range(0, 7)] * Random.Range(0,5), z));
           
            angle += (360f / segment);

        }

        LineColor = new Color(Random.value, Random.value, Random.value);
        lRenderer.SetColors(LineColor, LineColor);
    }
}