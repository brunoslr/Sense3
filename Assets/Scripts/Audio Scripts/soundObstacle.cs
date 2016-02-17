using UnityEngine;
using System.Collections;

public class soundObstacle : MonoBehaviour 
{
    private float pan;
    private float checkPan;
    private AudioControllerV2 audioController;
    /// <summary>
    /// To Do: Place audio visualizer in the right place in the architecture.
    /// </summary>
    /// <param name=""></param>
    /// <param name="Start"></param>
    /// <returns></returns>
	void Start () {
        checkPan = this.transform.localScale.x / 10.0f;
        audioController = GameObject.Find("PlayerAudio").GetComponentInChildren<AudioControllerV2>();
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
        if (other.gameObject.tag == "Player")
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
            volume = 0.2f + Mathf.Max(1 - volume, 0);

            pan = this.transform.GetChild(0).position.x - playerPosition.x;

            if (Mathf.Abs(pan) >= checkPan)
            {
                pan = pan / Mathf.Abs(pan);
            }
            else
            {
                pan = 0;
            }

            //Refactored audio controller below
            audioController.playNewTrack((int)pan, volume);
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
            volume = volume / this.gameObject.GetComponent<BoxCollider>().transform.localScale.z;
            //Make sure the the volume never goes negative
            volume = Mathf.Max(1 - volume, 0);

            if (Mathf.Abs(pan) >= checkPan)
            {
                pan = pan / Mathf.Abs(pan);
            }

            else
            {
                pan = 0;
            }

            audioController.setCurrentPan((int)pan, volume);          
        }
    }
    
    /// <summary>
    /// Checks if the player has picked up the obstacle and if not, tells the theme to stop playing the current track
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            bool soundPicked = this.gameObject.GetComponentInChildren<PickUpScript>().pickedUp;
            if ( soundPicked == false)
            {
                audioController.muteLast();              
            }
            audioController.FadeInLayers();
        }
    }
}
