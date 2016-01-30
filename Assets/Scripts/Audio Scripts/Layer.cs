using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Layer : MonoBehaviour {

    public string layername;                    //For ease of understanding. Can be exploited in later prototypes to automatically load all audio of a particular genre/layer.
    public AudioClip[] audioTracks;

    private AudioSource audioSource;
    private int totalTracks;
    private int currentTrackIndex;

	// Use this for initialization
	void Start () {
        audioSource = this.gameObject.AddComponent<AudioSource>();
        totalTracks = audioTracks.Length;
        audioSource.loop = true;
        audioSource.mute = true;
        audioSource.Play();
        currentTrackIndex = 0;
	}
	public void restartLayer()
    {
        totalTracks = audioTracks.Length;
        audioSource.loop = true;
        audioSource.mute = true;
        currentTrackIndex = 0;
    }

    public void destroySelf()
    {
        Destroy(audioSource);
        Destroy(this);
    }

    public int getTimeSample()
    {
        return audioSource.timeSamples;
    }

    public void playNewTrack(int timeSample)
    {
        audioSource.Stop();
        int newIndex;
        do
        {
          newIndex = Random.Range(0, totalTracks);
        } while (newIndex == currentTrackIndex && totalTracks > 1);

        currentTrackIndex = newIndex;
        audioSource.clip = audioTracks[currentTrackIndex];
        audioSource.mute = false;
        audioSource.timeSamples = timeSample;
        audioSource.Play();
    }

    public void playCurrentTrack()
    {
        audioSource.clip = audioTracks[currentTrackIndex];
        audioSource.mute = false;
    }

    public void StopTrack()
    {
        audioSource.Stop();  
    }

    public bool isPlaying()
    {
        return !(audioSource.mute);
    }

    public void setPanAndVol(int pan, float volume)
    {
        audioSource.panStereo = pan;
        audioSource.volume = volume;
    }

	// Update is called once per frame
	void Update () {
	
	}


}
