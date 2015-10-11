using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioController : MonoBehaviour {

    public AudioClip[] soundtracks;
    private int counter,maxLevel;
    private AudioSource[] audioSources;

	// Use this for initialization
	void Start () {
        maxLevel = soundtracks.Length;
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

    public void playPermanent()
    {
        //if(other.name.Contains("Sound"))
        if(counter < maxLevel)
        {
            audioSources[counter].panStereo = 0;
            audioSources[counter].mute = false;
            audioSources[counter].timeSamples = audioSources[0].timeSamples;
            audioSources[counter].Play();
            counter++;
        }
    }

    public void playTemp(int pan)
    {
        audioSources[counter].panStereo = pan;
        audioSources[counter].mute = false;
        audioSources[counter].timeSamples = audioSources[0].timeSamples;
        audioSources[counter].Play();
    }

    public void stopTemp()
    {
        audioSources[counter].mute = true ;
    }
}
