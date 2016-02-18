using UnityEngine;
using System.Collections;

public class RumblePickupCollide : MonoBehaviour {

    MineObstacle mineObstacle;

	// Use this for initialization
	void Start () {
        mineObstacle = transform.parent.GetComponent<MineObstacle>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            CoreSystem.ExecuteOnObstacleCollision();
            mineObstacle.state = 0;
        }
    }
}
