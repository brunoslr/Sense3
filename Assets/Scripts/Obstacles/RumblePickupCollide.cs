using UnityEngine;
using System.Collections;

public class RumblePickupCollide : MonoBehaviour {

    HapticObstacle hapticObstacle;

	// Use this for initialization
	void Start () {
        hapticObstacle = transform.parent.GetComponent<HapticObstacle>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            EventBusManager.ExecuteOnObstacleCollision();
            hapticObstacle.state = 0;
        }
    }
}
