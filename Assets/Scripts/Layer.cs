using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Layer : MonoBehaviour {

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
	
    public void playNewTrack()
    {
        int newIndex;
        do
        {
          newIndex = Random.Range(0, totalTracks - 1);
        } while (newIndex == currentTrackIndex);

        currentTrackIndex = newIndex;
        audioSource.clip = audioTracks[currentTrackIndex];
        audioSource.mute = false;
    }

    public void playCurrentTrack()
    {
        audioSource.clip = audioTracks[currentTrackIndex];
        audioSource.mute = false;
    }

    public void stop()
    {
        audioSource.mute = true;
    }

    public void setPanAndVol(float pan, float volume)
    {
        audioSource.panStereo = pan;
        audioSource.volume = volume;
    }

	// Update is called once per frame
	void Update () {
	
	}


}
