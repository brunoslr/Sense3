using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioController : MonoBehaviour {

    public AudioClip[] soundtracks;
    private int counter, trackCounter, maxLevel;
    private AudioSource[] audioSources;

	// Use this for initialization
	void Start () {
        maxLevel = soundtracks.Length /2;
        trackCounter = 0;
        counter = 0;

        for(int i=0; i< maxLevel; i++)
        {
            this.gameObject.AddComponent<AudioSource>();
        }
        audioSources = this.gameObject.GetComponents<AudioSource>();

        for(int i=0; i< maxLevel; i++)
        {
            audioSources[i].clip = soundtracks[i];
            audioSources[i].mute = true;
            audioSources[i].panStereo = 0;
            audioSources[i].volume = 1;
            audioSources[i].loop = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void incrementCounter()
    {
        //if(other.name.Contains("Sound"))
        //playTrackAt(counter);
        counter = (counter +1) % maxLevel;
        trackCounter = (trackCounter + 1) % (maxLevel * 2);
    }

    public void playCurrent(float pan)
    {
        //incrementTrack();
        audioSources[counter].clip = soundtracks[trackCounter];
        audioSources[counter].panStereo = pan;
        playTrackAt(counter);
        //audioSources[counter].mute = false;
        //audioSources[counter].timeSamples = audioSources[0].timeSamples;
        //audioSources[counter].Play();
        
    }

    public void setCurrentPan(float pan)
    {
        if(counter<maxLevel)
            audioSources[counter].panStereo = pan;
    }

    public void stopCurrent()
    {
        audioSources[counter].mute = true ;
    }

    public void stopTrackAt(int i)
    {
        if (i < maxLevel && i > 0)
            audioSources[i].mute = true;
    }

    public void playTrackAt(int i)
    {
        //Debug.Log(i + " " + maxLevel);
        if (i < maxLevel && i >= 0)
        {
            audioSources[i].mute = false;
            audioSources[i].timeSamples = audioSources[0].timeSamples;
            audioSources[i].Play();
        }
    }
}
