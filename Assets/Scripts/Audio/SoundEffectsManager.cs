using UnityEngine;
using System.Collections;

public class SoundEffectsManager : MonoBehaviour
{

    public AudioClip visObsCrashSound;
    public AudioClip minObsCrashSound;
    public AudioClip audPickupSound;
    public AudioClip audMissedSound;
    public AudioClip movPlayerLeftSound;
    public AudioClip movPlayerRightSound;
    public AudioClip jumpPlayerSound;

    private AudioSource visObsAudioSource;
    private AudioSource minObsAudioSource;
    private AudioSource audPickupAudioSource;
    private AudioSource audMissedAudioSource;
    private AudioSource movPlayerAudioSource;
    private AudioSource jumpPlayerAudioSource;

    public float masterVolume;

    // Use this for initialization
    void Start()
    {
        visObsAudioSource = this.gameObject.AddComponent<AudioSource>();
        minObsAudioSource = this.gameObject.AddComponent<AudioSource>();
        audPickupAudioSource = this.gameObject.AddComponent<AudioSource>();
        audMissedAudioSource = this.gameObject.AddComponent<AudioSource>();
        movPlayerAudioSource = this.gameObject.AddComponent<AudioSource>();
        jumpPlayerAudioSource = this.gameObject.AddComponent<AudioSource>();
        CoreSystem.onObstacleEvent += VisualObstacleCrashSound;
        CoreSystem.onSoundEvent += AudioObstaclePickupSound;
        CoreSystem.trackMissedEvent += AudioObstacleMissSound;
    }

    void OnDestroy()
    {
        CoreSystem.onObstacleEvent -= VisualObstacleCrashSound;
        CoreSystem.onSoundEvent -= AudioObstaclePickupSound;
        CoreSystem.trackMissedEvent -= AudioObstacleMissSound;
    }

    // Update is called once per frame
    void Update()
    {
       // visObsAudioSource.volume = masterVolume;
        //minObsAudioSource.volume = masterVolume; 
        //audPickupAudioSource.volume = masterVolume;
        //movPlayerAudioSource.volume = masterVolume;
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

    public void AudioObstacleMissSound()
    {
        audMissedAudioSource.PlayOneShot(audMissedSound);
    }

    // This sound should propogate from left ear to right ear if player moves right and vice versa
    public void MovePlayerLeftSound()
    {
        movPlayerAudioSource.PlayOneShot(movPlayerLeftSound);
    }

    public void MovePlayerRightSound()
    {
        movPlayerAudioSource.PlayOneShot(movPlayerRightSound);
    }

    public void JumpPlayerSound()
    {
        jumpPlayerAudioSource.PlayOneShot(jumpPlayerSound);
    }
}
