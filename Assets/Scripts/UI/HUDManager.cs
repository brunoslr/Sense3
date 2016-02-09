using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {
    public Text score;
    public Text soundPickups;
    public GameObject soundHud;
    string pickupText = "Sound Pickups: ";

    // Use this for initialization
    void Start () {
        soundHud = GameObject.Find("SoundPickupText");
        soundPickups = soundHud.GetComponent<Text>();
        PlayerStateScript.updateSoundPickup += this.DisplaySoundCount; 
        
        
        // Subscribe the function to eventmanager event
    }
	
	// Update is called once per frame
	void Update () {
	
	}
   
    public void DisplaySoundCount(string score) {
        soundPickups.text = pickupText + score;
    }
}
