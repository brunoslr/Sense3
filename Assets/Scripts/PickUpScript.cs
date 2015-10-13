using UnityEngine;
using System.Collections;

public class PickUpScript : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter(Collider other){
		if(other.gameObject.name == "Player"){
            other.gameObject.GetComponentInChildren<AudioController>().incrementCounter();
		}
		
	}
	
}
