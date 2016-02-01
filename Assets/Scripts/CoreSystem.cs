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

    // Use this for initialization

    // UI EVENTS
    public delegate void HUDeventHandler(string message);
    // All the functions that needs to be called when score is updated will be subscribed to this event
    public static event HUDeventHandler updateScore;
    // All the functions that needs to be called when a sound is picked will be subscribed to this event
    public static event HUDeventHandler updateSoundPickup;

    // On Collision Events
    public delegate void OnCollisionEvent();
    public static event OnCollisionEvent onSoundEvent;
    public static event OnCollisionEvent onObstacleEvent;

    // Trigger Functions
    public static void UpdateScore(string score)
    {
        updateScore(score.ToString());
    }
    public static void UpdateSoundPickup(string score)
    {
        updateSoundPickup(score.ToString());
    }
    // Trigger Function for OnCollisionEvents
    public static void ExecuteOnSoundCollision()
    {
        onSoundEvent();
    }
    public static void ExecuteOnObstacleCollision()
    {
        onObstacleEvent();
    }


 //   void Start () {
 //      // playerLM = this.gameObject.GetComponent<PlayerMovement>();
 //      // cameraEf = Camera.main.GetComponent<Effects>();
 //       //aController = this.gameObject.transform.FindChild("PlayerAudio").GetComponent<AudioControllerV2>();
 //      // soundEffectsManager = this.gameObject.GetComponent<SoundEffectsManager>();
   
	//}
    
    //public void ExecuteOnPickUp()
    //{
    //    //if(boostMode)
    //    //    playerLM.IncreasePlayerSpeed();
    //    //cameraEf.startTrail();
    //    //aController.updateNumOfCollectedLayers();
    //   // cameraEf.startFishEye(3);
    //}

    //public void ExecuteOnHit()
    //{
    //    //if(boostMode)
    //       // playerLM.ReducePlayerSpeed();
    //   // soundEffectsManager.VisualObstacleCrashSound();
    //}
    
	//void FixedUpdate()
 //   {
 //       if (constIncMode)
 //           playerLM.IncreasePlayerSpeed();
 //   }

	// Update is called once per frame
	//void Update () {
 //       if (Input.GetKeyDown(KeyCode.J))
 //       {
 //           GameReset();
 //       }
	//}
 
}
