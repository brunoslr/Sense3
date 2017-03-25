using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour {

    public GameObject soundObstacleprefab;
    public GameObject rumbleObstaclePrefab;
    public GameObject playerGameObject;
    public Text screenText;
    public GameObject obstaclePrefab;

    public string alignedWithSound;
    public string notAlignedWithSound;
    public string outofSound;
    public string gotTheSound;

    public string alignedWithRumble;
    public string notAlignedWithRumble;
    public string collidedRumble;
    public string passedRumble;

    public string passedVisual;
    public string collidedVisual;
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

    private bool visualCollided;
   
    private LevelLoader levelLoader;

    void Awake()
    {
        leftSoundObstacle = Instantiate(soundObstacleprefab, new Vector3(playerGameObject.transform.position.x - soundObstacleDispX, playerGameObject.transform.position.y, playerGameObject.transform.position.z + 100.0f), Quaternion.identity) as GameObject;
        rightSoundObstacle = Instantiate(soundObstacleprefab) as GameObject;
        rumbleObstacle = Instantiate(rumbleObstaclePrefab) as GameObject;
        visualObstacle = Instantiate(obstaclePrefab) as GameObject;

        rightSoundObstacle.SetActive(false);
        rumbleObstacle.SetActive(false);
        visualObstacle.SetActive(false);

        levelLoader = gameObject.AddComponent<LevelLoader>();
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

        visualCollided = false;

        EventBusManager.onObstacleEvent += CheckVisualCollision;

        FloorPlaneGrid.instance.SetupPlaneGrid();
    }
	
    void OnDisable()
    {
        EventBusManager.onObstacleEvent -= CheckVisualCollision;
    }

    void FixedUpdate()
    {
        FloorPlaneGrid.instance.UpdatePlaneOnPlayerPosition((int)playerGameObject.transform.position.x, (int)playerGameObject.transform.position.z);
       // UpdatePlane();
    }

    // Update is called once per frame
    void Update () 
    {
        if (Input.GetButton("Cancel"))
        {
            levelLoader.LoadScene("MainMenu");
        }
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

             if (Mathf.Abs(leftSoundPickup.position.x - playerGameObject.transform.position.x) >= leftSoundObstacle.transform.localScale.x / 2.0f)
             {
                 leftSoundObstacle.transform.Translate(new Vector3(0.0f, 0.0f, 1.0f) * playerMovement.initialSpeed * Time.deltaTime, Space.World);

                 if (leftSoundPickup.position.x - playerGameObject.transform.position.x > 0.0f)
                    screenText.text = outofSound + " Move right to get back in.";

                 else 
                     screenText.text = outofSound + " Move left to get back in.";
             }

             else if (Mathf.Abs(leftSoundPickup.position.x - playerGameObject.transform.position.x) >= checkPan)
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

            if (Mathf.Abs(rightSoundPickup.position.x - playerGameObject.transform.position.x) >= rightSoundObstacle.transform.localScale.x / 2.0f)
            {
                rightSoundObstacle.transform.Translate(new Vector3(0.0f, 0.0f, 1.0f) * playerMovement.initialSpeed * Time.deltaTime, Space.World);

                if (rightSoundPickup.position.x - playerGameObject.transform.position.x > 0.0f)
                    screenText.text = outofSound + " Move right to get back in.";

                else
                    screenText.text = outofSound + " Move left to get back in.";
            }

            else if (Mathf.Abs(rightSoundPickup.position.x - playerGameObject.transform.position.x) >= checkPan)
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

    void CheckVisualCollision()
    {
        screenText.text = collidedVisual;
        visualCollided = true;
    }

    void VisualUpdate()
    {
        if (playerGameObject.transform.position.z >= visualObstacle.transform.position.z)
        {
            if(!visualCollided)
                screenText.text = passedVisual;
            Invoke("SwitchToRumble", nextMechanicTimer);
        }

        else if(!visualCollided)
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
        if (playerGameObject.transform.position.z >= rumblePickup.position.z)
        {
            if(Mathf.Abs(playerGameObject.transform.position.x - rumblePickup.position.x) >= rumblePickup.lossyScale.x / 2.0f)
                screenText.text = passedRumble;
            else
                screenText.text = collidedRumble;

            Invoke("CallNextScene", nextMechanicTimer);
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

    void CallNextScene()
    {
        SceneManager.LoadScene("Infinite"); 
    }
}
