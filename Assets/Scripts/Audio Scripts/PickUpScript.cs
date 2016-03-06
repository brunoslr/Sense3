using UnityEngine;
using System.Collections;

public class PickUpScript : MonoBehaviour {

    public bool pickedUp;

	// Use this for initialization
    void Start()
    {
        pickedUp = false;
    }
    void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
            pickedUp = true;
		}
	}
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            pickedUp = true;
        }
    }

}
