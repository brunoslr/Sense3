using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;


public class ForeGroundController : MonoBehaviour {

    public float scale = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
    public void setBloom(float bloom)
    {
        this.gameObject.GetComponent<BloomOptimized>().intensity = bloom * scale;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
