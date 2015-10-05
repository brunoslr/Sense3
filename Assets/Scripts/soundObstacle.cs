using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]

public class soundObstacle : MonoBehaviour {

    private AudioSource audioSource;
	// Use this for initialization
	void Start () {
        audioSource = this.gameObject.GetComponent<AudioSource>();
        //this.gameObject.GetComponent<BoxCollider>().center = new Vector3(10.0f,0.0f,this.gameObject.GetComponent<BoxCollider>().size.z / 2.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            audioSource.mute = false;
            Vector3 playerPosition = other.gameObject.transform.position;
            float pan = this.transform.position.x - playerPosition.x;
            
            if (Mathf.Abs(pan) >= 2.0f)
                pan = pan / Mathf.Abs(pan);
            else
                pan = 0;

            audioSource.panStereo = pan;
        }
    }

    void OnTriggerExit(Collider other)
    {
        audioSource.mute = true;
        //Destroy(this.gameObject);
    }
}
