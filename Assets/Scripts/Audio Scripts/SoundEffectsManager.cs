using UnityEngine;
using System.Collections;

public class SoundEffectsManager : MonoBehaviour {

    public AudioClip visObsCrashSound;
    public AudioClip minObsCrashSound;
    public AudioClip audPickupSound;
    public AudioClip movPlayerSound;

    private AudioSource visObsAudioSource;
    private AudioSource minObsAudioSource;
    private AudioSource audPickupAudioSource;
    private AudioSource movPlayerAudioSource;

	// Use this for initialization
	void Start () {
	    visObsAudioSource =  this.gameObject.AddComponent<AudioSource>();
        minObsAudioSource = this.gameObject.AddComponent<AudioSource>();
        audPickupAudioSource = this.gameObject.AddComponent<AudioSource>();
        movPlayerAudioSource = this.gameObject.AddComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void VisualObstacleCrashSound()
    {
        visObsAudioSource.PlayOneShot(visObsCrashSound);
    }

    public void MineObstacleCrashSound()
    {
        minObsAudioSource.PlayOneShot(minObsCrashSound);
    }

    public void AudioObstaclePickupSound()
    {
        audPickupAudioSource.PlayOneShot(audPickupSound);
    }

    // This sound should propogate from left ear to right ear if player moves right and vice versa
    public void MovePlayerSound()
    {
        movPlayerAudioSource.PlayOneShot(movPlayerSound);
    }
}
