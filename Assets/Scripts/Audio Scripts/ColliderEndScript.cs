using UnityEngine;
using System.Collections;

public class ColliderEndScript : MonoBehaviour {

    private AudioControllerV2 audioController;

    void Awake()
    {
        audioController = GameObject.Find("PlayerAudio").GetComponentInChildren<AudioControllerV2>();
    }
    // Use this for initialization
    void Start () {
        
    }

    void OnTriggerEnter(Collider other)
    {
        audioController.disableScope();
    }

    // Update is called once per frame
    void Update () {
	
	}
}
