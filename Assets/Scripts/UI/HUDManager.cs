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
        soundPickups = soundHud.GetComponent<Text>();
        EventManager.updateSoundPickup += this.UpdateSoundPickup; // Subscribe the function to eventmanager event
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void UpdateSoundPickup(string score) {
        soundPickups.text = pickupText + score;
    }
}
