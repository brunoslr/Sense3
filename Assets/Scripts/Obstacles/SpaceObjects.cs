using UnityEngine;
using System.Collections;

public class SpaceObjects : MonoBehaviour {
    public float speed;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(Vector3.up * speed * Time.deltaTime,Space.World);
	}
}
