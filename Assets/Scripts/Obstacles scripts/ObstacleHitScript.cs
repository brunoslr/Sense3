using UnityEngine;
using System.Collections;

/// <summary>
/// Triggers the hit() in the state manager.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class ObstacleHitScript : MonoBehaviour {

   
	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
      
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<LaneMovement>().hit();
        }
    }
}
