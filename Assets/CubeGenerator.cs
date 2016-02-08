using UnityEngine;
using System.Collections;

public class CubeGenerator : MonoBehaviour {
    public int xDisp;
    public float zPos;
	// Use this for initialization
	void Start () {

        // I dont know how to get z from

        transform.GetChild(0).transform.position = new Vector3(transform.position.x - xDisp, 0.0f, zPos);
        transform.GetChild(1).transform.position = new Vector3(transform.position.x + xDisp, 0.0f, zPos);
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
