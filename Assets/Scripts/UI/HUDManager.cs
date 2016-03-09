using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {
    public GameObject soundPickupImage;
    public Sprite[] spritesheet;
    public Text scoreText;
    // Use this for initialization
    void Start () {
        // Subscribe the function to eventmanager event
        PlayerStateScript.updateSoundPickup += this.DisplaySoundCount;
        soundPickupImage.GetComponent<Image>().sprite = spritesheet[0];
        scoreText = soundPickupImage.GetComponentInChildren<Text>();
    }
	
	// Update is called once per frame
	void Update () {
	}
   
    public void DisplaySoundCount() {
        if (PlayerStateScript.getPlayerLevel() >= 0 && PlayerStateScript.getPlayerLevel() <= 7)
        {
            soundPickupImage.GetComponent<Image>().sprite = spritesheet[PlayerStateScript.getPlayerLevel()];
            scoreText.text = PlayerStateScript.getPlayerLevel().ToString();
        }
    }
    void OnDestroy()
    {
        PlayerStateScript.updateSoundPickup -= this.DisplaySoundCount;
    }
}
