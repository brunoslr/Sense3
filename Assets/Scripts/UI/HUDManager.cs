using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {
    public GameObject soundPickupImage;
    public Sprite[] spritesheet;
    // Use this for initialization
    void Start () {
        // Subscribe the function to eventmanager event
        PlayerStateScript.updateSoundPickup += this.DisplaySoundCount;
        soundPickupImage.GetComponent<Image>().sprite = spritesheet[0];
    }
	
	// Update is called once per frame
	void Update () {
	}
   
    public void DisplaySoundCount(string score) {
        soundPickupImage.GetComponent<Image>().sprite = spritesheet[PlayerStateScript.getPlayerLevel()];
    }
    void OnDestroy()
    {
        PlayerStateScript.updateSoundPickup -= this.DisplaySoundCount;
    }
}
