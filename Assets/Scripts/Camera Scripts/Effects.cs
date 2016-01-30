using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

/// <summary>
/// This script goes on the main camera.
/// It declares and defines all the effects taking place in the game.
/// 
/// Design the function such that the state manager only calls the start...(). 
/// The function should take steps to terminate by itself. The state manager 
/// should not have to call another function to terminate (unless its necessary).
/// </summary>
public class Effects : MonoBehaviour {

    // Variable which the designer can adjust for every effect are defined here.
    // Please give a default value to start with. We do not want any un-initialized error.
    public float bloomScale = 1.0f;
    public float fishEyeScale = 1.0f;
    public float blurTime = 5.0f;
    public float trailTime = 2.0f;

    private GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player");
        if (player == null)
            Debug.Log("Cannot find object with tag player");
	}

    #region Trail Renderer
    public void startTrail()
    {
        // Ensuring the player is there, and to avoid null reference errors.
        if (player != null)
        {
            if(player.GetComponent<TrailRenderer>() == null)
            {
                player.AddComponent<TrailRenderer>();
            }
            player.GetComponent<TrailRenderer>().enabled = true;
            StartCoroutine(endTrail());
        }
    }

    IEnumerator endTrail()
    {
        yield return new WaitForSeconds(trailTime);
        player.GetComponent<TrailRenderer>().enabled = false;
    }
    #endregion

    #region Bloom
    public void setBloom(float bloom)
    {
        this.gameObject.GetComponent<BloomOptimized>().intensity = bloom * bloomScale;
    }
    #endregion

    #region FishEye
    public void startFishEye(float strength)
    {
        if(this.gameObject.GetComponent<Fisheye>() == null)
        {
            this.gameObject.AddComponent<Fisheye>();
        }
        this.gameObject.GetComponent<Fisheye>().strengthX = strength * fishEyeScale;
        this.gameObject.GetComponent<Fisheye>().strengthY = strength * fishEyeScale;
        StartCoroutine(endFishEye());
    }

    IEnumerator endFishEye()
    {
        yield return new WaitForSeconds(2.0f);
        this.gameObject.GetComponent<Fisheye>().strengthX = 0.0f;
        this.gameObject.GetComponent<Fisheye>().strengthY = 0.0f;
    }
    #endregion 

    #region Blur
    public void startBlur()
    {
        if (this.gameObject.GetComponent<MotionBlur>() == null)
        {
            this.gameObject.AddComponent<MotionBlur>();
        }
        this.gameObject.GetComponent<MotionBlur>().blurAmount = 0.52f;
        StartCoroutine(endBlur());
    }

    IEnumerator endBlur()
    {
        yield return new WaitForSeconds(blurTime);
        this.gameObject.GetComponent<MotionBlur>().blurAmount = 0.0f;
    }
    #endregion
    
    // Update is called once per frame
	void Update () {
	
	}
}
