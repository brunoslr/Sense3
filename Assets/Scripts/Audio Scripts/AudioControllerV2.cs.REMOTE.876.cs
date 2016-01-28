using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioControllerV2 : MonoBehaviour {

    private Layer[] layers;
    private int totalLayers;
    public List<int> availableLayers;
    private int lastPlayedLayerID;

	// Use this for initialization
	void Start () {
        layers = this.gameObject.GetComponents<Layer>();
        totalLayers = layers.Length;
        availableLayers.Clear();
        for (int i = 0; i < totalLayers; i++)
            availableLayers.Add(i);
	}
	
    //Randomly chooses one player from the list of layers that are not playing, and plays a track from that layer
    //If all tracks are playing, then starts playing a new Track in a random layer
    public void playNewTrack(int pan, float volume)
    {
        int layerID, range;

        if(availableLayers.Count > 0)
            range = availableLayers.Count - 1;
        else
            range = totalLayers-1;
 
        layerID = Random.Range(0, range);
        layerID = availableLayers[layerID];

        if (availableLayers.Count > 0)
            availableLayers.Remove(layerID);

        lastPlayedLayerID = layerID;

        layers[layerID].playNewTrack();
        layers[layerID].setPanAndVol(pan, volume);
    }

    public void setCurrentPan(int pan, float volume)
    {
        layers[lastPlayedLayerID].setPanAndVol(pan, volume);
    }

    public void stopCurrentTrack()
    {
        layers[lastPlayedLayerID].stop();
        availableLayers.Add(lastPlayedLayerID);
    }

    public void stopAllTracks()
    {
        for (int i = 0; i < totalLayers; i++)
        {
            layers[i].stop();
        }
    }

    public void resetController()
    {
        stopAllTracks();
        Start();
    }

	// Update is called once per frame
	void Update () {
	
	}
}
