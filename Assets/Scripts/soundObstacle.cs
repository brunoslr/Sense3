using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]

public class soundObstacle : MonoBehaviour 
{
    //private AudioSource[] audioSource;
    //private AudioSource CenteraudioSource;
    
    public AudioVisualizer audioVisualizer;
    public GameObject shock;
    private bool ColliderEnterCount;
    // Use this for initialization
	void Start () {
        //audioSource = this.gameObject.GetComponents<AudioSource>();
        audioVisualizer = shock.GetComponent<AudioVisualizer>();
        ColliderEnterCount = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (ColliderEnterCount == false)
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
                other.gameObject.GetComponentInChildren<AudioController>().playCurrent((int)pan, volume);
            }
        }
    }

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
            
            other.gameObject.GetComponentInChildren<AudioController>().setCurrentPan((int) pan, volume);
            audioVisualizer.PlayerVisualizer(this.gameObject.transform.GetChild(0).position.z - other.gameObject.transform.position.z);
        }
    }
    


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponentInChildren<AudioController>().stopCurrent();
            audioVisualizer.StopVisualizer();
        }
       // audioSource[0].loop = false;
       // audioSource[1].loop = false;
       // audioSource[0].mute = true;
       // audioSource[1].mute = true;
    }
}
