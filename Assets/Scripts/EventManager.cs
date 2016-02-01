using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {

    // Define delegates and events
    public delegate void HUDeventHandler(string message);
    public static event HUDeventHandler updateScore;
    public static event HUDeventHandler updateSoundPickup;

    // Trigger the events
    public static void UpdateScore(string score)
    {
        updateScore(score.ToString());
    }
    public static void UpdateSoundPickup(string score)
    {
        updateSoundPickup(score.ToString());
    }

}
