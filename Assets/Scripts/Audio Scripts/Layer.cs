using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Layer : MonoBehaviour {

    public string layername;                    //For ease of understanding. Can be exploited in later prototypes to automatically load all audio of a particular genre/layer.
    public string FolderName;
    public int totalTracks;
    public AudioClip[] audioTracks;             //This is public only because another script access this to load (AudioToolApp.cs)
    public float FadeDuration;

    private AudioSource audioSource;
    private int currentTrackIndex;
    private float minVol;
    private float maxVol;

	// Use this for initialization
	void Start () {
        audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.timeSamples = 0;
        loadAudioClips();
        FadeDuration = audioTracks[0].length;
        FadeDuration /= 4.0f;
        totalTracks = audioTracks.Length;
        audioSource.loop = true;
        audioSource.mute = true;
        audioSource.Play();
        currentTrackIndex = 0;
        minVol = 0.4f;
        maxVol = 0.8f;
        AudioControllerV2.fadeInLayers += startFadeIn;
        
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

    public void Mute()
    {
        audioSource.mute = true;
    }
    public void UnMute()
    {
        audioSource.mute = false;
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

    void OnDestroy()
    {
        AudioControllerV2.fadeInLayers -= startFadeIn;
    }
 
    public void startFadeIn()
    {
        StartCoroutine(FadeIn());
    }
    public void startFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float diff = (audioSource.volume - minVol) / (10.0f * FadeDuration);
        while (audioSource.volume > minVol)
        {
            audioSource.volume -= diff;
            yield return new WaitForSeconds(0.1f * FadeDuration);
        }
    }

    //TODO add validation to check if layer exists before access 
    IEnumerator FadeIn()
    {
        float diff = (maxVol - audioSource.volume)/(10.0f * FadeDuration); 
        while (audioSource.volume < maxVol)
        {
            //audioSource.volume = Mathf.Lerp(audioSource.volume, maxVol, FadeDuration);
            //yield return 0;
            audioSource.volume += diff;
            yield return new WaitForSeconds(0.1f * FadeDuration);
        }
    }
}
