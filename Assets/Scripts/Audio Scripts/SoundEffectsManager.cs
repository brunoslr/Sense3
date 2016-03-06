using UnityEngine;
using System.Collections;

public class SoundEffectsManager : MonoBehaviour
{

    public AudioClip visObsCrashSound;
    public AudioClip minObsCrashSound;
    public AudioClip audPickupSound;
    public AudioClip movPlayerLeftSound;
    public AudioClip movPlayerRightSound;
    public AudioClip jumpPlayerSound;

    private AudioSource visObsAudioSource;
    private AudioSource minObsAudioSource;
    private AudioSource audPickupAudioSource;
    private AudioSource movPlayerAudioSource;

    public float masterVolume;

    // Use this for initialization
    void Start()
    {
        visObsAudioSource = this.gameObject.AddComponent<AudioSource>();
        minObsAudioSource = this.gameObject.AddComponent<AudioSource>();
        audPickupAudioSource = this.gameObject.AddComponent<AudioSource>();
        movPlayerAudioSource = this.gameObject.AddComponent<AudioSource>();
        CoreSystem.onObstacleEvent += VisualObstacleCrashSound;
        CoreSystem.onSoundEvent += AudioObstaclePickupSound;
        masterVolume = 0.03f;
    }

    void OnDestroy()
    {
        CoreSystem.onObstacleEvent -= VisualObstacleCrashSound;
        CoreSystem.onSoundEvent -= AudioObstaclePickupSound;
    }

    // Update is called once per frame
    void Update()
    {
        visObsAudioSource.volume = 1.0f;
        minObsAudioSource.volume = 1.0f; 
        audPickupAudioSource.volume = 1.0f;
        movPlayerAudioSource.volume = masterVolume;
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
        movPlayerAudioSource.PlayOneShot(jumpPlayerSound);
    }
}
