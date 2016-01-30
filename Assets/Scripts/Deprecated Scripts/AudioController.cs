// ATENTION!!! No longer in Use. Refactored and renamed as "AudioControllerV2.cs"

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AudioController : MonoBehaviour {

    public float minVol = 0.65f;    
    public AudioClip[] soundtracks;

    private int counter, trackCounter, maxLevel;
    private AudioSource[] audioSources;
    private AudioSource tempSource;

	// Use this for initialization
	void Start () {
        maxLevel = soundtracks.Length;
        trackCounter = 0;
        counter = 0;

        //The total number of audio source will be equal to the total number of tracks/2 
        // and one more for the temp source
        for(int i=0; i<= maxLevel; i++)
        {
            this.gameObject.AddComponent<AudioSource>();
        }

        audioSources = this.gameObject.GetComponents<AudioSource>();

        //The temp source is the last audio source attached to the object
        tempSource = audioSources[maxLevel];

        for(int i=0; i< maxLevel; i++)
        {
            audioSources[i].clip = soundtracks[i];
            audioSources[i].mute = true;
            audioSources[i].panStereo = 0;
            audioSources[i].volume = 1;
            audioSources[i].loop = true;
            audioSources[i].Play();
        }

        //Set the values the same as other source for temp too
        setAudioSourceValues(tempSource, audioSources[0]);
	}
	
	// Update is called once per frame
	void Update () {

        int baseSample = audioSources[0].timeSamples;
        tempSource.timeSamples = baseSample;
        for(int i =1; i < maxLevel; i++)
        {
            audioSources[i].timeSamples = baseSample;
        }

	}

    //a = b
    private void setAudioSourceValues(AudioSource a, AudioSource b)
    {
        a.clip = b.clip;
        a.mute = b.mute;
        a.panStereo = b.panStereo;
        a.volume = b.volume;
        a.loop = b.loop;
    }

    //incrementing the counter
    public void incrementCounter()
    {
        //stopCurrentTrack();
        //First set the current audio source to play the track "tempSource" is playing
        setAudioSourceValues(audioSources[counter], tempSource);
        playTrackAt(counter);

        //Increment the coutner and the track values 
        counter = (counter +1) % maxLevel;
        trackCounter = (trackCounter + 1) % (maxLevel * 2);

        //Update the tempSource with the next track to be played when entering a sound region
        tempSource.clip = soundtracks[trackCounter];

        //This plays the track along with the track about to be substituted
    }

    //Does the exact opposite of the above
    public void decrementCounter()
    {
        counter = (counter - 1 + maxLevel) % maxLevel;
        trackCounter = (trackCounter - 1 + (maxLevel * 2)) % (maxLevel * 2);

        stopTrackAt(counter);
        tempSource.clip = soundtracks[trackCounter];
    }

    //These functions only change the values ofthe temp audio source
    public void playCurrent(int pan, float Volume)
    {
        tempSource.panStereo = pan;
        tempSource.mute = false;
        tempSource.volume = Volume;
        tempSource.Play();
        reduceOverallVol();
    }

    public void setCurrentPan(int pan, float Volume)
    {
        tempSource.mute = false;
        tempSource.panStereo = pan;
        tempSource.volume = Volume;
    }

    public void stopCurrent()
    {
        tempSource.mute = true;
        increaseOverallVol();
    }


    //A handle to play or stop a track.
    public void stopTrackAt(int i)
    {
        if (i < maxLevel && i >= 0)
            audioSources[i].mute = true;
    }

    public void playTrackAt(int i)
    {
        if (i < maxLevel && i >= 0)
        {
            audioSources[i].mute = false;
            audioSources[i].timeSamples = audioSources[0].timeSamples;
            audioSources[i].Play();
        }
    }

    private void reduceOverallVol()
    {
        for(int i=0; i < audioSources.Length; i++)
        {
            audioSources[i].volume = minVol;
        }
    }

    private void increaseOverallVol()
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].volume = 1.0f;
        }
    }
}
