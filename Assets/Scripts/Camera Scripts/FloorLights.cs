using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable 618 // Disable obsolete warning for spectrum class

public class FloorLights : MonoBehaviour {

    //private float average = 0;
    public FFTWindow window;
    public GameObject plane;
    public GameObject[] visualizer;
    public uint length = 256;

    private float planeLength;
    private float[] spectrum;
    private float zscale;

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

        spectrum = new float[length];
        visualizer = new GameObject[length];
        planeLength = plane.transform.localScale.z;
        Renderer renderer;
        Vector3 displacement = new Vector3(0, 0, (length / 2) * 0.03f);
        for(int i=0;i < length; i++)
        {
            visualizer[i] = GameObject.Instantiate(plane);
            renderer = visualizer[i].GetComponent<Renderer>();
            renderer.material.color = (i%2 == 0 ) ? Color.blue : Color.green;
            visualizer[i].transform.parent = this.transform;
            zscale = visualizer[i].transform.localScale.z;
            visualizer[i].transform.localPosition = /*this.transform.position + */new Vector3(0, -0.001f, -0.03f * i) + displacement;
            visualizer[i].transform.localScale = new Vector3(0, 0, 0);
        }
    }
	
	// Update is called once per frame
	void Update () {
        AudioListener.GetSpectrumData(spectrum, 0, window);
        for(int i=0;i< length; i++)
        {
            visualizer[i].transform.localScale = new Vector3(spectrum[i] * 10.0f, 0.5f, 0.003f);
        }

    }
}
