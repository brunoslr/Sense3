using UnityEngine;
using System.Collections;

public class PlayerStateScript : MonoBehaviour {

    public string LossScenario_sceneName;
    public string WinScenario_sceneName;
    public int finalRunTime = 15;

    private int maxLevel;
    private int playerLevel;
    private bool finalState;
    private LevelLoader levelLoader;

    // UI EVENTS
    //public delegate void HUDeventHandler(string message);
    //// All the functions that needs to be called when score is updated will be subscribed to this event
    //public static event HUDeventHandler updateScore;
    //// All the functions that needs to be called when a sound is picked will be subscribed to this event
    //public static event HUDeventHandler updateSoundPickup;


    // Use this for initialization
    void Start () {
        playerLevel = 0;
        maxLevel = (GameObject.FindGameObjectWithTag("Player").GetComponentsInChildren<Layer>()).Length;
        finalState = false;
        CoreSystem.onSoundEvent += incrementPlayerLevel;
        CoreSystem.onObstacleEvent += decrementPlayerLevel;
	}

    private void checkState()
    {
        if (playerLevel > maxLevel)
        {
            finalState = true;
            StartCoroutine(checkWin());
        }
        else
            if(playerLevel < 0)
        {
            Debug.Log("Loading :" + LossScenario_sceneName);
            levelLoader.LoadScene(LossScenario_sceneName);
        }
        else
        {
            finalState = false;
        }

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
            Debug.Log("Loading :" + WinScenario_sceneName);
            levelLoader.LoadScene(WinScenario_sceneName);
        }
    }

    private void updatePlayerLevel()
    {
        Debug.Log(playerLevel);
        checkState();
        //updateSoundPickup(playerLevel.ToString());
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

    int getPlayerLevel()
    {
        return playerLevel;
    }

    //// U.I Trigger Functions
    //public void UpdateScore(string score)
    //{
    //    updateScore(score.ToString());
    //}
    //public static void UpdateSoundPickup(string score)
    //{
    //    updateSoundPickup(score.ToString());
    //}

	// Update is called once per frame
	void Update () {
	
	}
}
