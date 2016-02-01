using UnityEngine;
using System.Collections;
#pragma warning disable 618 // Disable obsolete warning message
#pragma warning disable 649 // Disable field never used warning message
#pragma warning disable 414 // Disable private field never used warning message

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
    float radius = 2;
    void Awake()
    {
        //Get and store a reference to the following attached components:
        //AudioSource
        //this.aSource = GetComponent<AudioSource>();
        //LineRenderer
        //lRenderer = GetComponent<LineRenderer>();

    }

    void Start()
    {
        lRenderer.SetVertexCount(segment + 1);
        lRenderer.useWorldSpace = false;
        lineMat = this.gameObject.GetComponent<LineRenderer>().material;
       // InvokeRepeating("CreatePoints",0.1f, 0.2f);
    }

    void Update()
    {
        
    }

    public void PlayerVisualizer(float radius)
    {
        //lRenderer.enabled = true;
  
    }
    public void StopVisualizer()
    {
        //lRenderer.enabled = false;

    }
    public void CreatePoints()
    {

        AudioListener.GetSpectrumData(this.samples, 0, FFTWindow.BlackmanHarris);
       
        for (int i = 0; i < (segment + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            //lRenderer.SetPosition(i, new Vector3(x, samples[Random.Range(0, 7)] * Random.Range(0,5), z));
           // lRenderer.SetPosition(i, new Vector3(x, z, samples[Random.Range(0, 7)] * Random.Range(0, 5)));
            angle += (360f / segment);

        }

        LineColor = new Color(Random.value, Random.value, Random.value);
        //lRenderer.SetColors(LineColor, LineColor);
    }

}