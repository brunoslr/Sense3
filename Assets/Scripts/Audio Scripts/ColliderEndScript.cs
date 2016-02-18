using UnityEngine;
using System.Collections;

public class ColliderEndScript : MonoBehaviour {

    private AudioControllerV2 audioController;

    // Use this for initialization
    void Start () {
        audioController = GameObject.Find("PlayerAudio").GetComponentInChildren<AudioControllerV2>();
    }

    void OnTriggerEnter(Collider other)
    {
        audioController.disableScope();
    }

    // Update is called once per frame
    void Update () {
	
	}
}
