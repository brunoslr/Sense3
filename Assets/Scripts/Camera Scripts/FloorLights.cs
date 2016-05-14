using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

#pragma warning disable 618 // Disable obsolete warning for spectrum class

public class FloorLights : MonoBehaviour {

    //private float average = 0;
    public FFTWindow window;
    public GameObject plane;
    public GameObject[] visualizer;
    public uint length = 256;
    public uint accuracy = 40;
    public uint fftPrecision = 5;
    public float checkTime = 1.5f;
    public Transform NewSP_handle;

    private Transform Player;
    private const uint fullLength = 1024; 
    private float planeLength;
    private float[] spectrum;
    private float[] spectrumVal;
    private float zscale;
    private bool collinear = false;
    private bool enableLights = false;

    // Use this for initialization
    void Start () {
        //round up length to the closest power of 2
        length--;
        length |= length >> 1;
        length |= length >> 2;
        length |= length >> 4;
        length |= length >> 8;
        length |= length >> 16;
        length++;

        fftPrecision = (uint) (length ^ ((length ^ fftPrecision) & - (Convert.ToInt32(fftPrecision < length))));

        Player = GameObject.FindGameObjectWithTag("Player").transform;

        spectrum = new float[fullLength];
        spectrumVal = new float[length];
        visualizer = new GameObject[length];
        planeLength = plane.transform.localScale.z;
        //NewSP_handle = gameObject.transform.GetChild(0);
        //Renderer renderer;
        Vector3 displacement = new Vector3(0, 0, 0);
        for(int i=0;i < length; i++)
        {
            visualizer[i] = Instantiate(plane);
            //renderer = visualizer[i].GetComponent<Renderer>();
            //renderer.material.color = (i%2 == 0 ) ? Color.blue : Color.green;
            visualizer[i].transform.parent = NewSP_handle;
            zscale = visualizer[i].transform.localScale.z;
            visualizer[i].transform.localPosition = /*this.transform.position + */new Vector3(0, -0.051f, -1.0f * i);
            visualizer[i].transform.localScale = new Vector3(0, 0, 0);
        }

        CoreSystem.onSoundEvent += incrementfftPrecision;
        CoreSystem.onObstacleEvent += decrementfftPrecision;
    }
	
    void displayLights()
    {
        AudioListener.GetSpectrumData(spectrum, 0, window);
        uint diff = fullLength / length;
        int j = 0;
        const float max = 0.25f;
        float carry = 0;
        for (int i = 0; i < length; i++)
        {
            j = 0;
            spectrumVal[i] = 0;
            while (j < diff)
            {
                spectrumVal[i] += spectrum[i * diff + (j++)];
            }
            if(spectrumVal[i] > 2 * max)
            {
                carry = spectrumVal[i] - (2 * max);
                spectrumVal[i] = max + UnityEngine.Random.Range(0.0f, max);
            }
            else if(spectrum[i] < max)
            {
                spectrum[i] += carry % max;
                carry -= (max + 1);
            }
            visualizer[i].transform.localScale = new Vector3(spectrumVal[i % fftPrecision] * 5.0f, 0.05f, 0.1f);
        }
    }

	// Update is called once per frame
	void Update () {

        //This is temporary : change when convenient
        if (Math.Abs(Player.transform.position.x - NewSP_handle.position.x) < accuracy &&
            Math.Abs(Player.transform.position.z - NewSP_handle.position.z) < 800)
        {
            collinear = true;
            StartCoroutine(checkPos());
        }
        else
            collinear = false;

        if (enableLights)
            displayLights();
        else
        {
            for (int i = 0; i < length; i++)
            {
                visualizer[i].transform.localScale = new Vector3(0, 0, 0);
            }
        }
    }

    private IEnumerator checkPos()
    {
        int i = 0;
        int temp = (int) checkTime * 10;
        while (i < checkTime)
        {
            yield return new WaitForSeconds(0.1f);
            i++;
        }

        if (collinear)
        {
            enableLights = true;
            //Debug.Log("lights are true");
        }
        else
            enableLights = false;
    }

    public void incrementfftPrecision()
    {
        fftPrecision += 5;
    }

    public void decrementfftPrecision()
    {
        fftPrecision -= 5;
    }
}
