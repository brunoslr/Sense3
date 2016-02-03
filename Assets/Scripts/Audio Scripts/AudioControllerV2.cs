using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioControllerV2 : MonoBehaviour {

    private Layer[] layers;
    private int totalLayers;   // Total number of layers - bass, rythm etc etc
    public List<int> availableLayers; //Available layers that are not currently playing but available
    private int lastPlayedLayerID;
    private int numOfCollectedLayers;

    public delegate void FadeinQue();

    public static event FadeinQue fadeInLayers;

    public int NumOfCollectedLayers{
        get
        {
            return numOfCollectedLayers;
        }
        set
        {
            numOfCollectedLayers = value;
        }
    }

	// Use this for initialization
	void Start () {
        lastPlayedLayerID = 0;
        layers = this.gameObject.GetComponents<Layer>();
        totalLayers = layers.Length;
        for (int i = 0; i < totalLayers; i++)
            availableLayers.Add(i);
        CoreSystem.onSoundEvent += updateNumOfCollectedLayers;
    }

    //Randomly chooses one player from the list of layers that are not playing, and plays a track from that layer
    //If all tracks are playing, then starts playing a new Track in a random layer
    public void playNewTrack(int pan, float volume)
    {
        int layerID, range;

        if(availableLayers.Count > 0)
            range = availableLayers.Count - 1;
        else
            range = totalLayers;
 
        layerID = Random.Range(0, range);

        if (availableLayers.Count > 0)
        {
            layerID = availableLayers[layerID];
            availableLayers.Remove(layerID);
        }

        int temp = lastPlayedLayerID;

        lastPlayedLayerID = layerID;

        //Pass the previously playing audioSource's time sample to sync with
        layers[layerID].playNewTrack(layers[temp].getTimeSample());
        layers[layerID].setPanAndVol(pan, volume);

        FadeOutLayers();
    }

    public void setCurrentPan(int pan, float volume)
    {
        layers[lastPlayedLayerID].setPanAndVol(pan, volume);
    }

    public void stopCurrentTrack()
    {
        layers[lastPlayedLayerID].StopTrack();
        availableLayers.Add(lastPlayedLayerID);
    }

    int numOfPlayingLayers()
    {
        return totalLayers - availableLayers.Count;
    }


    public void resetLayers()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i].StopTrack();
        }
        availableLayers.Clear();
        for (int i = 0; i < totalLayers; i++)
            availableLayers.Add(i);

    }


    public void stopAllTracks()
    {
        for (int i = 0; i < totalLayers; i++)
        {
            layers[i].StopTrack();
        }
    }

    /// <summary>
    /// Resets the controller to the way it was at the start of the game
    /// </summary>
    public void resetController()
    {
        stopAllTracks();
        Start();
    }

    /// <summary>
    /// Increments the number of Sound Tracks Collected everytime we pick a sound
    /// </summary>
    /// 
    public void updateNumOfCollectedLayers()
    {
        //Increments the sound count
        numOfCollectedLayers++;
        // Cal the trigger the function UpdateSoundPickUp
        CoreSystem.UpdateSoundPickup(numOfCollectedLayers.ToString());

    }


    public void FadeOutLayers()
    {
        for(int i=0;i < totalLayers; i++)
        {
            if(i != lastPlayedLayerID)
            {
                layers[i].startFadeOut();
            }
        }
    }

    public void FadeInLayers()
    {
        fadeInLayers();
    }

    #if UNITY_EDITOR
    // Update is called once per frame
    void Update () {

       // Debug.Log(NumOfCollectedLayers);
	}
    #endif
}
