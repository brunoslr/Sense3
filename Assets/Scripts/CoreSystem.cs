using UnityEngine;
using System.Collections;
using System.Collections.Generic;


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
    public int coolDownTime = 2;
    public static int coolDownTimeInSeconds = 2;
    public static bool coolDownFlag = false;
    public static CoreSystem instance;                  // needed to call coroutines from STATIC functions

    // On Collision Events
    public delegate void OnCollisionEvent();
    public delegate void OnSoundMissEvent();
    public static event OnCollisionEvent onSoundEvent;
    public static event OnCollisionEvent onObstacleEvent;
    public static event OnSoundMissEvent trackMissedEvent;


    void Awake()
    {
        coolDownTimeInSeconds = coolDownTime;
        instance = this;
        coolDownFlag = false; 
    }

    // Trigger Function for OnCollisionSoundEvents -- Function is triggered when you pick up the sound
    public static void ExecuteOnSoundCollision()
    {
        if(onSoundEvent != null)
            onSoundEvent();
    }

    public static void ExecuteOnObstacleCollision()
    {
        if (!coolDownFlag)
        {
            if (onObstacleEvent != null)
                onObstacleEvent();

            coolDownFlag = true;

            instance.StartCoroutine(instance.CoolDown());
        }
        //LogSoundPickupSubscribedFunctions()
    }

    public static void ExecuteTrackMissEvent()
    {
        if (trackMissedEvent != null)
            trackMissedEvent();
    }


    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(coolDownTimeInSeconds);
        coolDownFlag = false;
    }

    //finding subscribed functions 
    public void LogSoundPickupSubscribedFunctions()
    {
        System.Delegate[] list = onSoundEvent.GetInvocationList();

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
