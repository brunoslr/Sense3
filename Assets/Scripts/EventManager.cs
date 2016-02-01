using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {

    // Define delegates and events
    public delegate void HUDeventHandler(string message);
    // All the functions that needs to be called when score is updated will be subscribed to this event
    public static event HUDeventHandler updateScore;  
    // All the functions that needs to be called when a sound is picked will be subscribed to this event
    public static event HUDeventHandler updateSoundPickup;

    public delegate void OnCollisionEvent();

    public static event OnCollisionEvent onCollisionEvent;

    // Trigger Functions
    public static void UpdateScore(string score)
    {
        updateScore(score.ToString());
    }
    public static void UpdateSoundPickup(string score)
    {
        updateSoundPickup(score.ToString());
    }

    public static void ExecuteOnCollision()
    {
        onCollisionEvent();
    }

}
