using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;


public class ForeGroundController : MonoBehaviour {

    public float bloomScale = 1.0f;
    public float fishEyeScale = 1.0f;
    public float blurTime = 5.0f;
	// Use this for initialization
	void Start () {
	
	}
	
    public void setBloom(float bloom)
    {
        this.gameObject.GetComponent<BloomOptimized>().intensity = bloom * bloomScale;
    }

    public void setFishEye(float strength)
    {
        this.gameObject.GetComponent<Fisheye>().strengthX = strength * fishEyeScale;
        this.gameObject.GetComponent<Fisheye>().strengthY = strength * fishEyeScale;
        StartCoroutine(endFishEye());
        
    }

    public void startBlur()
    {
        this.gameObject.GetComponent<MotionBlur>().blurAmount = 0.92f;
        StartCoroutine(endBlur());
    }

    IEnumerator endFishEye()
    {
        yield return new WaitForSeconds(2.0f);
        this.gameObject.GetComponent<Fisheye>().strengthX = 0.0f;
        this.gameObject.GetComponent<Fisheye>().strengthY = 0.0f;
    }

    IEnumerator endBlur()
    {
        yield return new WaitForSeconds(blurTime);
        this.gameObject.GetComponent<MotionBlur>().blurAmount = 0.0f;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
