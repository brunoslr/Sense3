using UnityEngine;
using System.Collections;

public class HitZonePlane : MonoBehaviour {

    public GameObject playerGo;

	// Use this for initialization
	void Start () {
        transform.position = new Vector3(playerGo.transform.position.x, playerGo.transform.position.y, playerGo.transform.position.z + transform.localScale.z * 10.0f + 10.0f);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void LateUpdate()
    {
        transform.position = new Vector3(playerGo.transform.position.x, playerGo.transform.position.y, playerGo.transform.position.z + transform.localScale.z * 10.0f + 10.0f);
    }

    //void OnTriggerEnter(Collider other)
    //{

    //}

    void OnTriggerStay(Collider other)
    {
         if(other.gameObject.tag == "Obstacle")
        {
            other.gameObject.GetComponentInChildren<Renderer>().material.SetFloat("_MKGlowPower", 2.5f);
            other.gameObject.GetComponentInChildren<Renderer>().material.SetFloat("_MKGlowTexStrength", 1.0f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            other.gameObject.GetComponentInChildren<Renderer>().material.SetFloat("_MKGlowPower", 0.0f);
            other.gameObject.GetComponentInChildren<Renderer>().material.SetFloat("_MKGlowTexStrength", 0.0f);
        }
    }
}
