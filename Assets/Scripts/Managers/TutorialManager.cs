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
   
    //Plane update requirements
    public GameObject planePrefab; 
    private float sizeOfPlaneX;
    private float sizeOfPlaneZ;
    private GameObject[,] planes = new GameObject[3, 3];

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

        SetupPlaneGrid();
    }
	
    void OnDisable()
    {
        EventBusManager.onObstacleEvent -= CheckVisualCollision;
    }

    // Update is called once per frame
    void Update () 
    {
        UpdatePlane();
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

    // plane

    private void SetupPlaneGrid()
    {
        sizeOfPlaneX = planePrefab.GetComponent<Renderer>().bounds.size.x;
        sizeOfPlaneZ = planePrefab.GetComponent<Renderer>().bounds.size.z;
        Vector3 planePosition;
        for (int i = 0; i < 3; i++)
        {
            for (int c = 0; c < 3; c++)
            {
                planes[i, c] = Instantiate(planePrefab);
                planes[i, c].name = i + "," + c;
                planePosition = new Vector3((float)sizeOfPlaneX * (i - 1), -1, (float)sizeOfPlaneZ * ((c - 1) * -1));
                planes[i, c].transform.position = planePosition;
            }
        }
    }

    private void UpdatePlane()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int c = 0; c < 3; c++)
            {
                float planeXMin = planes[i, c].transform.position.x - (sizeOfPlaneX / 2);
                float planeXMax = planeXMin + sizeOfPlaneX;
                float planeZMin = planes[i, c].transform.position.z - (sizeOfPlaneZ / 2);
                float planeZMax = planeZMin + sizeOfPlaneZ;
                if (playerGameObject.transform.position.x > planeXMin && playerGameObject.transform.position.x < planeXMax && playerGameObject.transform.position.z > planeZMin && playerGameObject.transform.position.z < planeZMax)
                {
                    UpdatePlaneGrid(i, c);
                }
            }
        }
    }

    private void UpdatePlaneGrid(int x, int z)
    {
        if (x == 0)
        {
            MoveLeft();
        }
        else if (x == 2)
        {
            MoveRight();
        }
        if (z == 0)
        {
            MoveAhead();
        }
    }

    private void MoveLeft()
    {
        GameObject[,] newPlanes = new GameObject[3, 3];
        for (int i = 0; i < 3; i++)
        {
            Vector3 addPos = new Vector3(-sizeOfPlaneX * 3, 0, 0);
            planes[2, i].transform.position += addPos;
        }
        for (int i = 0; i < 3; i++)
        {
            int c = i - 1;
            c = (c == -1) ? 2 : c;
            newPlanes[i, 0] = planes[c, 0];
            newPlanes[i, 1] = planes[c, 1];
            newPlanes[i, 2] = planes[c, 2];
        }
        planes = newPlanes;
    }

    private void MoveRight()
    {
        GameObject[,] newPlanes = new GameObject[3, 3];
        for (int i = 0; i < 3; i++)
        {
            Vector3 addPos = new Vector3(sizeOfPlaneX * 3, 0, 0);
            planes[0, i].transform.position += addPos;
        }
        for (int i = 0; i < 3; i++)
        {
            int c = i + 1;
            c = (c == 3) ? 0 : c;
            newPlanes[i, 0] = planes[c, 0];
            newPlanes[i, 1] = planes[c, 1];
            newPlanes[i, 2] = planes[c, 2];
        }
        planes = newPlanes;
    }

    private void MoveAhead()
    {
        GameObject[,] newPlanes = new GameObject[3, 3];
        for (int i = 0; i < 3; i++)
        {
            Vector3 addPos = new Vector3(0, 0, sizeOfPlaneZ * 3);
            planes[i, 2].transform.position += addPos;
        }
        for (int i = 0; i < 3; i++)
        {
            int c = i - 1;
            c = (c == -1) ? 2 : c;
            newPlanes[0, i] = planes[0, c];
            newPlanes[1, i] = planes[1, c];
            newPlanes[2, i] = planes[2, c];
        }
        planes = newPlanes;
    }
}
