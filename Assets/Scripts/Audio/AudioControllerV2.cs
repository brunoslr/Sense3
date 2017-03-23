using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioControllerV2 : MonoBehaviour {

    private Layer[] layers;
    private int totalLayers;   // Total number of layers - bass, rythm etc etc

    private List<int> availableLayers; //Available layers that are not currently playing but available
    private List<int> activeLayers;     //Layers that are currently playing.
    private int lastPlayedLayerID;
    //private int numOfCollectedLayers;
    private bool pickUpScope;
    public delegate void FadeinQue();
    public static event FadeinQue fadeInLayers;

    //public int NumOfCollectedLayers{
    //    get
    //    {
    //        return numOfCollectedLayers;
    //    }
    //    set
    //    {
    //        numOfCollectedLayers = value;
    //    }
    //}

	// Use this for initialization
	void Start () {
        pickUpScope = false;
        lastPlayedLayerID = 0;
        layers = this.gameObject.GetComponents<Layer>();
        totalLayers = layers.Length;

        availableLayers = new List<int>();
        activeLayers = new List<int>();
        availableLayers.Clear();
        activeLayers.Clear();
        for (int i = 0; i < totalLayers; i++)
            availableLayers.Add(i);
        EventBusManager.onSoundEvent += incrementLayerStack;
        EventBusManager.onObstacleEvent += stopOneTrack;
    }


    void OnDisable()
    {
        EventBusManager.onSoundEvent -= incrementLayerStack;
        EventBusManager.onObstacleEvent -= stopOneTrack;
    }

    //Randomly chooses one player from the list of layers that are not playing, and plays a track from that layer
    //If all tracks are playing, then starts playing a new Track in a random layer
    public void playNewTrack(int pan, float volume)
    {
        if (!pickUpScope)
        {
            int layerID, range;

            if (availableLayers.Count > 0)
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
            //activeLayers.Add(lastPlayedLayerID);

            //Pass the previously playing audioSource's time sample to sync with
            layers[layerID].playNewTrack(layers[temp].getTimeSample());
            layers[layerID].setPanAndVol(pan, volume);

            FadeOutLayers();

            //StartCoroutine(refreshLast());
            pickUpScope = true;
        }
        else
        {
            layers[lastPlayedLayerID].UnMute();
        }
    }

    public void disableScope()
    {
        if (pickUpScope)
        {
            pickUpScope = false;
            refreshLastPlayed();
        }
    }

    public void incrementLayerStack()
    {
        pickUpScope = false;
        layers[lastPlayedLayerID].setPanAndVol(0, 1.0f);
        activeLayers.Add(lastPlayedLayerID);
        //Increments the sound count
        //numOfCollectedLayers++;

        // Cal the trigger the function UpdateSoundPickUp
        //CoreSystem.UpdateSoundPickup(numOfCollectedLayers.ToString());
    }

    private void refreshLastPlayed()
    {
        if (activeLayers.Count > 0) 
            lastPlayedLayerID = activeLayers[activeLayers.Count - 1];
    }

    public void muteLast()
    {
        layers[lastPlayedLayerID].Mute();
    }

    public void unMuteLast()
    {
        layers[lastPlayedLayerID].UnMute();
    }

    //IEnumerator refreshLast()
    //{
    //    yield return new WaitForSeconds(4.0f * layers[0].FadeDuration);
    //    refreshLastPlayed();
    //    pickUpScope = false;
    //}

    public void setCurrentPan(int pan, float volume)
    {
        layers[lastPlayedLayerID].setPanAndVol(pan, volume);
    }

    public void stopOneTrack()
    {
        if (activeLayers.Count > 0)
        {
            int temp = Random.Range(0, activeLayers.Count - 1);
            temp = activeLayers[temp];
            layers[temp].StopTrack();
            activeLayers.Remove(temp);
            availableLayers.Add(temp);


            // Cal the trigger the function UpdateSoundPickUp
            //numOfCollectedLayers--;
            //CoreSystem.UpdateSoundPickup(numOfCollectedLayers.ToString());

        }
   
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

    public void FadeOutLayers()
    {
        for(int i=0;i < activeLayers.Count; i++)
        {
            layers[activeLayers[i]].startFadeOut();
        }
    }

    public void FadeInLayers()
    {
        if (fadeInLayers != null)
            fadeInLayers();

    }

    #if UNITY_EDITOR
    // Update is called once per frame
    void Update () {
	}
    #endif
}
