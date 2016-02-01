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
		if(other.gameObject.name == "Player"){
            pickedUp = true;
            other.gameObject.GetComponent<CoreSystem>().ExecuteOnPickUp();
		}
	}
}
