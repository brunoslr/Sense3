using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]

public class soundObstacle : MonoBehaviour {

    //private AudioSource[] audioSource;
    //private AudioSource CenteraudioSource;
	
    // Use this for initialization
	void Start () {
        //audioSource = this.gameObject.GetComponents<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
                       
            Vector3 playerPosition = other.gameObject.transform.position;
            float pan = this.transform.position.x - playerPosition.x;

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

            other.gameObject.GetComponentInChildren<AudioController>().playTemp((int) pan);
  
        }
    }
    
    

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponentInChildren<AudioController>().stopTemp();
        }
       // audioSource[0].loop = false;
       // audioSource[1].loop = false;
       // audioSource[0].mute = true;
       // audioSource[1].mute = true;
    }
}
