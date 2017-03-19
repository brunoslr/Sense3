using UnityEngine;
using System.Collections;
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
        CoreSystem.onSoundEvent += startTrail;
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

    // Update is called once per frame
	void Update () {
	
	}
}
