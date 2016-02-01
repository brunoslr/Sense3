using UnityEngine;
using System.Collections;


/// <summary>
/// This class acts the central hub of the state machine implemented in the game architecture.
/// It recieves data and calls corresponding functions in the respective components.
/// 
/// This Script is applied directly on the player(the parent) object.
/// </summary>
public class CoreSystem : MonoBehaviour {

    // If you feel a particular call is going to be repeated lots of times,
    // create a reference to it here. Querying again and again would be expensive.
    // Naming convention : object_the_script_si acctached_to Initials of the script
    private PlayerMovement    playerLM;
    private Effects         cameraEf;
    private AudioControllerV2 aController;
    private SoundEffectsManager soundEffectsManager;

    public bool boostMode;
    public bool constIncMode;
    public bool constSpeedMode;
    public GameObject startPosition;
	// Use this for initialization
	void Start () {
        playerLM = this.gameObject.GetComponent<PlayerMovement>();
        cameraEf = Camera.main.GetComponent<Effects>();
        aController = this.gameObject.transform.FindChild("PlayerAudio").GetComponent<AudioControllerV2>();
        soundEffectsManager = this.gameObject.GetComponent<SoundEffectsManager>();
	}
    
    public void ExecuteOnPickUp()
    {
        if(boostMode)
            playerLM.IncreasePlayerSpeed();
        cameraEf.startTrail();
        aController.updateNumOfCollectedLayers();
       // cameraEf.startFishEye(3);
    }

    public void ExecuteOnHit()
    {
        if(boostMode)
            playerLM.ReducePlayerSpeed();
        soundEffectsManager.VisualObstacleCrashSound();
    }
    
	void FixedUpdate()
    {
        if (constIncMode)
            playerLM.IncreasePlayerSpeed();
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.J))
        {
            GameReset();
        }
	}
    public void GameReset()
    {
        playerLM.ResetSpeed();
        this.GetComponent<TrailRenderer>().enabled = false;
        this.gameObject.transform.position = startPosition.transform.position;
        aController.resetLayers();
    }
}
