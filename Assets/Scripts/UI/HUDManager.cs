using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {
    public Text score;
    public Text soundPickups;
    public GameObject soundHud;

    // Use this for initialization
    void Start () {
        soundHud = GameObject.Find("SoundPickupText");
        soundPickups = soundHud.GetComponent<Text>();
        // Subscribe the function to eventmanager event
        PlayerStateScript.updateSoundPickup += this.DisplaySoundCount; 
    }
	
	// Update is called once per frame
	void Update () {
	
	}
   
    public void DisplaySoundCount(string score) {
		soundPickups.text += score;
    }
    void OnDestroy()
    {
        PlayerStateScript.updateSoundPickup -= this.DisplaySoundCount;
    }
}
