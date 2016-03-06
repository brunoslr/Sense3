using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour {

    private bool passedLeftSP;
    private bool passedRightSP;
    private bool leftPickedUp;
    private bool ctr;
  
    private int whichUpdate;

    //---------------

    public GameObject soundObstacleprefab;
    public GameObject rumbleObstaclePrefab;
    public GameObject playerGameObject;
    public Text screenText;
    public GameObject obstaclePrefab;

    public string alignedWithSound;
    public string notAlignedWithSound;
    public string gotTheSound;

    public string alignedWithRumble;
    public string notAlignedWithRumble;
    public string passedRumble;

    public string passedVisual;
    public string avoidVisual;

    public float soundObstacleDispX;
    public float nextMechanicTimer;

    private GameObject leftSoundObstacle;
    private GameObject rightSoundObstacle;
    private GameObject rumbleObstacle;
    private GameObject visualObstacle;

    private PlayerMovement playerMovement;

    private Transform leftSoundPickup;
    private Transform rightSoundPickup;
    private Transform rumblePickup;

    private float checkPan;

    private enum UpdateState { LEFTSOUND, RIGHTSOUND, VISUAL, HAPTIC };
    private UpdateState updateState;

    private enum SoundState { BEFORE, INSIDE, AFTER };
    private SoundState leftSoundState;
    private SoundState rightSoundState;


    void Awake()
    {
        leftSoundObstacle = Instantiate(soundObstacleprefab, new Vector3(playerGameObject.transform.position.x - soundObstacleDispX, playerGameObject.transform.position.y, playerGameObject.transform.position.z + 100.0f), Quaternion.identity) as GameObject;
        rightSoundObstacle = Instantiate(soundObstacleprefab) as GameObject;
        rumbleObstacle = Instantiate(rumbleObstaclePrefab) as GameObject;
        visualObstacle = Instantiate(obstaclePrefab) as GameObject;

        rightSoundObstacle.SetActive(false);
        rumbleObstacle.SetActive(false);
        visualObstacle.SetActive(false);
    }

	// Use this for initialization
	void Start () 
    {
        playerMovement = playerGameObject.GetComponent<PlayerMovement>();

        leftSoundPickup = leftSoundObstacle.transform.GetChild(0);
        rightSoundPickup = rightSoundObstacle.transform.GetChild(0);
        rumblePickup = rumbleObstacle.transform.GetChild(0);

        checkPan = leftSoundObstacle.transform.localScale.x / 10.0f;

        // --------

        leftSoundState = SoundState.INSIDE;
        rightSoundState = SoundState.BEFORE;

        updateState = UpdateState.LEFTSOUND;
	}
	
	// Update is called once per frame
	void Update () 
    {
        switch(updateState)
        {
            case UpdateState.LEFTSOUND:
                LeftSoundUpdate();
                break;

            case UpdateState.RIGHTSOUND:
                RightSoundUpdate();
                break;

            case UpdateState.VISUAL:
                VisualUpdate();
                break;

            case UpdateState.HAPTIC:
                RumbleUpdate();
                break;
        }
	}

    void LeftSoundUpdate()
    {  
        if(leftSoundState == SoundState.INSIDE)
        {
             if (leftSoundPickup.position.z - playerGameObject.transform.position.z <= 0)
             {
                 screenText.text = gotTheSound;
                 leftSoundState = SoundState.AFTER;
             }

            if (Mathf.Abs(leftSoundPickup.position.x - playerGameObject.transform.position.x) >= checkPan)
            {
                leftSoundObstacle.transform.Translate(new Vector3(0.0f, 0.0f, 1.0f) * playerMovement.initialSpeed * Time.deltaTime, Space.World);
                screenText.text = notAlignedWithSound;
            }

            else
            {
                screenText.text = alignedWithSound;
            }
        }

        if(leftSoundState == SoundState.AFTER)
        {
            screenText.text = gotTheSound;
            Invoke("SwitchToRight", nextMechanicTimer);
        }
    }

    void SwitchToRight()
    {
        updateState = UpdateState.RIGHTSOUND;
        rightSoundObstacle.transform.position = new Vector3(playerGameObject.transform.position.x + soundObstacleDispX, playerGameObject.transform.position.y, playerGameObject.transform.position.z);
        rightSoundObstacle.SetActive(true);
        rightSoundState = SoundState.INSIDE;
    }

    void RightSoundUpdate()
    {
        if (rightSoundState == SoundState.INSIDE)
        {
            if (rightSoundPickup.position.z - playerGameObject.transform.position.z <= 0)
            {
                screenText.text = gotTheSound;
                rightSoundState = SoundState.AFTER;
            }

            if (Mathf.Abs(rightSoundPickup.position.x - playerGameObject.transform.position.x) >= checkPan)
            {
                rightSoundObstacle.transform.Translate(new Vector3(0.0f, 0.0f, 1.0f) * playerMovement.initialSpeed * Time.deltaTime, Space.World);
                screenText.text = notAlignedWithSound;
            }

            else
            {
                screenText.text = gotTheSound;
                screenText.text = alignedWithSound;
            }
        }

        if (rightSoundState == SoundState.AFTER)
        {
            screenText.text = gotTheSound;
            Invoke("SwitchToVisual", nextMechanicTimer);
        }
    }

    void SwitchToVisual()
    {
        updateState = UpdateState.VISUAL;
        visualObstacle.transform.position = new Vector3(playerGameObject.transform.position.x, 0.0f, playerGameObject.transform.position.z + 800.0f);
        visualObstacle.SetActive(true);
    }

    void VisualUpdate()
    {
        if (playerGameObject.transform.position.z > visualObstacle.transform.position.z)
        {
            screenText.text = passedVisual;
            // Invike infinite scene after 2 seconds
        }

        else
        {
            screenText.text = avoidVisual;
        }
    }

    void SwitchToRumble()
    {
        updateState = UpdateState.HAPTIC;
        rumbleObstacle.transform.position = new Vector3(playerGameObject.transform.position.x + 40.0f, playerGameObject.transform.position.y, playerGameObject.transform.position.z + 1000.0f);
        rumbleObstacle.SetActive(true);
    }

    void RumbleUpdate()
    {
        if (playerGameObject.transform.position.z > rumblePickup.position.z)
        {
            screenText.text = passedRumble;
            Invoke("SwitchToRumble", nextMechanicTimer);
        }

        else
        {

            if (Mathf.Abs(rumbleObstacle.transform.position.x - playerGameObject.transform.position.x) >= rumbleObstacle.transform.localScale.x / 2.0f)
            {
                screenText.text = notAlignedWithRumble;
            }

            else
            {
                screenText.text = alignedWithRumble;
            }
        }
    }
}
