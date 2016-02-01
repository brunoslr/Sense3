using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Layer : MonoBehaviour {

    public string layername;                    //For ease of understanding. Can be exploited in later prototypes to automatically load all audio of a particular genre/layer.
    public string FolderName;
    public int totalTracks;
    public AudioClip[] audioTracks;             //This is public only because another script access this to load (AudioToolApp.cs)
   
    private AudioSource audioSource;
    private int currentTrackIndex;

	// Use this for initialization
	void Start () {
        audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.timeSamples = 0;
        loadAudioClips();
        totalTracks = audioTracks.Length;
        audioSource.loop = true;
        audioSource.mute = true;
        audioSource.Play();
        currentTrackIndex = 0;
    }

    private void loadAudioClips()
    {
        string path = "Audio Files/Music/";
        audioTracks = new AudioClip[totalTracks];
        for (int i = 1; i <= totalTracks; i++)
        {
            string index = "00" + i;
            index = index.Substring(index.Length - 2);

            string trackName = layername + index;

            audioTracks[i-1] = Resources.Load(path + FolderName + "/" + trackName, typeof(AudioClip)) as AudioClip;
            if (audioTracks[i - 1] == null)
                Debug.Log("Error Loading audio File : " + path + FolderName + "/" + trackName + ".wav");
            
        }
    }

	public void restartLayer()
    {
        if (audioSource != null)
        {
            audioSource.timeSamples = 0;
            audioSource.loop = true;
            audioSource.mute = true;
        }

        currentTrackIndex = 0;
        totalTracks = audioTracks.Length;
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
