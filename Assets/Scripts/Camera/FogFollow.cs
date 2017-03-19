using UnityEngine;
using System.Collections;

public class FogFollow : MonoBehaviour {

    private GameObject playerRef;
    private Vector3 offset;

	// Use this for initialization
	void Start () {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        offset = this.transform.position - playerRef.transform.position;
    }

    // Update is called once per frame
    void Update ()
    {  
        this.transform.position = playerRef.transform.position + offset; 
	}
}
