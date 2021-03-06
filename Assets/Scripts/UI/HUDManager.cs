﻿using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {
    public GameObject soundPickupImage;
    public Sprite[] spritesheet;
    public Text scoreText;
    // Use this for initialization

    void Awake()
    {
        PlayerStateScript.updateSoundPickup += this.DisplaySoundCount;
    }

    void Start () {
        // Subscribe the function to eventmanager event
       
        soundPickupImage.GetComponent<Image>().sprite = spritesheet[0];
        scoreText = soundPickupImage.GetComponentInChildren<Text>();
    }
	
   
    public void DisplaySoundCount() {
        if (PlayerStateScript.GetPlayerLevel() >= 0 && PlayerStateScript.GetPlayerLevel() <= 7)
        {
            soundPickupImage.GetComponent<Image>().sprite = spritesheet[PlayerStateScript.GetPlayerLevel()];
            scoreText.text = PlayerStateScript.GetPlayerLevel().ToString();
        }
    }
    void OnDestroy()
    {
        PlayerStateScript.updateSoundPickup -= this.DisplaySoundCount;
    }
}
