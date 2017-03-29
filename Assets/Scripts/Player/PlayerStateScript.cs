using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStateScript : MonoBehaviour {

    public string LossScenario_sceneName;
    public string WinScenario_sceneName;
    public int finalRunTime = 15;
    public GameObject playerAudio;
    public Text endText;

    private bool loadNextOverride = true;
    private static int maxLevel;
    private static int playerLevel; // Changed it to static, might affect player sound levels.
    private bool finalState;
    private LevelLoader levelLoader;
    private GameObject mainCamera;


    // UI EVENTS
    public delegate void HUDeventHandler();
    // All the functions that needs to be called when a sound is picked will be subscribed to this event
    public static event HUDeventHandler updateSoundPickup;

    //Initializing values that will be accessed from other scripts on awake
    void Awake()
    {
        if (playerAudio != null && (playerAudio.GetComponents<Layer>().Length > 0))
            maxLevel = playerAudio.GetComponents<Layer>().Length;
        else
            maxLevel = 7;

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        //#--------------------------------------------------------------
        // Order of subscription is the order of execution of events. 
        // Putting it in Awake make sure these functions are given higher priority over other functions that are subscribed to this event in other classes
        //#--------------------------------------------------------------

        EventBusManager.onSoundEvent += incrementPlayerLevel;
        EventBusManager.onObstacleEvent += decrementPlayerLevel;
    }

    // Use this for initialization
    void Start () {
        playerLevel = 0;
        finalState = false;
        levelLoader = gameObject.AddComponent<LevelLoader>();
       
	}


    void OnDisable()
    {
        EventBusManager.onSoundEvent -= incrementPlayerLevel;
        EventBusManager.onObstacleEvent -= decrementPlayerLevel;
    }

    private void checkState()
    {
        if (playerLevel >= maxLevel)
        {
            finalState = true;
            StartCoroutine(checkWin());
        }
        else if(playerLevel < 0)
        {
            //Player lost
            endText.text = "GAME ENDED!! \n";
            loadNextOverride = false;
            StartCoroutine(LossCountDown());
            mainCamera.transform.parent = null;
            //levelLoader.LoadScene(LossScenario_sceneName);
        }
        else
        {
            finalState = false;
        }

    }

    public static int GetMaxPlayerLevel()
    {
        return maxLevel;
    }

    public static int GetPlayerLevel()
    {
        return playerLevel;
    }

    private IEnumerator checkWin()
    {
        int i = 0;
        while( i < finalRunTime && finalState == true)
        {
            i++;
            yield return new WaitForSeconds(1.0f);
        }

        if(finalState)
        {
            levelLoader.LoadScene(WinScenario_sceneName);
        }
    }

    private IEnumerator LossCountDown()
    {
        int i = 0;
        while (i < 10 && loadNextOverride == false)
        {
            i++;
            endText.text = "GAME ENDED!! \n Press any key to continue ... (" + (10 - i).ToString() + ")" ;
            yield return new WaitForSeconds(1.0f);
        }

        levelLoader.LoadScene(LossScenario_sceneName);
    }

    private void updatePlayerLevel()
    {
        checkState();
        if(updateSoundPickup != null)
            updateSoundPickup();

    }


    public void incrementPlayerLevel()
    {
        playerLevel++;
        updatePlayerLevel();
    }

    public void decrementPlayerLevel()
    {
        playerLevel--;
        updatePlayerLevel();
    }


    void FixedUpdate()
    {
        if (Input.anyKeyDown == true && loadNextOverride == false)
            loadNextOverride = true;
    }

}
