using UnityEngine;
using System.Collections;

public class DebugCamera : MonoBehaviour {

    public GameObject player;
    public float yOffset;
    public float xOffset;

	// Use this for initialization
	void Start () { 
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x + xOffset, yOffset, player.transform.position.z);
    }
}
