using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]

public class soundObstacle : MonoBehaviour 
{
    //private AudioSource[] audioSource;
    //private AudioSource CenteraudioSource;
    
    public bool pickedUp;
    public GameObject shock;
    
    private AudioVisualizer audioVisualizer;
    private bool ColliderEnterCount;

    // Use this for initialization
	void Start () {
        //audioSource = this.gameObject.GetComponents<AudioSource>();
        audioVisualizer = shock.GetComponent<AudioVisualizer>();
        pickedUp = false;
        ColliderEnterCount = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Once the players the sound obstacle's collider, this calls a funciton in theme.
    /// Computes and sends the volume and pan as calculated according to the relative position of the player
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
    
        if(other.gameObject.tag == "Player")
        {
            if (ColliderEnterCount == false)        // This is to make sure that this runs only once
            {
                Vector3 playerPosition = other.gameObject.transform.position;

                //calculate the volume
                float volume;
                //We need the pickup collider's position because its located at the end of the collider
                Vector3 pickUpPosition = this.transform.GetChild(0).transform.position;
                volume = Vector3.Distance(playerPosition, pickUpPosition);
                //because every collider has a different size
                volume = volume / this.gameObject.GetComponent<BoxCollider>().size.z;
                //Make sure the the volume never goes negative
                volume = 0.1f + Mathf.Max(1 - volume, 0);

                float pan = this.transform.GetChild(0).position.x - playerPosition.x;

                //audioSource[0].timeSamples = other.gameObject.GetComponent<AudioSource>().timeSamples;
                //audioSource[1].timeSamples = other.gameObject.GetComponent<AudioSource>().timeSamples;

                if (Mathf.Abs(pan) >= 2.0f)
                {
                    pan = pan / Mathf.Abs(pan);
                    //audioSource[0].panStereo = pan;
                    //audioSource[0].volume = Mathf.Abs(pan);
                    //audioSource[0].mute = false;
                    //audioSource[0].Play();

                }
                else
                {
                    pan = 0;
                    //audioSource[1].panStereo = pan;
                    //audioSource[1].volume = 1 - Mathf.Abs(pan);
                    //audioSource[1].mute = false;
                    //audioSource[1].Play();
                }
                ColliderEnterCount = true;
                //other.gameObject.GetComponentInChildren<AudioController>().playCurrent((int)pan, volume);
                //Refactored audio controller below
                other.gameObject.GetComponentInChildren<AudioControllerV2>().playNewTrack((int)pan, volume);
            }
        }
    }

    /// <summary>
    /// Updates the volume and pan with respect to the current player position in the AudioControllerV2 script
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Vector3 playerPosition = other.gameObject.transform.position;
            float pan = this.transform.GetChild(0).position.x - playerPosition.x;

            //calculate the volume
            float volume;
            //We need the pickup collider's position because its located at the end of the collider
            Vector3 pickUpPosition = this.transform.GetChild(0).transform.position;
            volume = Vector3.Distance(playerPosition, pickUpPosition);
            //because every collider has a different size
            volume = volume / this.gameObject.GetComponent<BoxCollider>().size.z;
            //Make sure the the volume never goes negative
            volume = 0.1f + Mathf.Max(1 - volume, 0);

            if (Mathf.Abs(pan) >= 10.0f)
            {
                pan = pan / Mathf.Abs(pan);
            }

            else
            {
                pan = 0;
            }
            
            //other.gameObject.GetComponentInChildren<AudioController>().setCurrentPan((int) pan, volume);
            other.gameObject.GetComponentInChildren<AudioControllerV2>().setCurrentPan((int)pan, volume);
            audioVisualizer.PlayerVisualizer(this.gameObject.transform.GetChild(0).position.z - other.gameObject.transform.position.z);
        }
    }
    
    /// <summary>
    /// Checks if the player has picked up the obstacle and if not, tells the theme to stop playing the current track
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && this.gameObject.GetComponentInChildren<PickUpScript>().pickedUp == false)
        {
            //other.gameObject.GetComponentInChildren<AudioController>().stopCurrentTrack();
            other.gameObject.GetComponentInChildren<AudioControllerV2>().stopCurrentTrack();
            audioVisualizer.StopVisualizer();
        }
    }
}
