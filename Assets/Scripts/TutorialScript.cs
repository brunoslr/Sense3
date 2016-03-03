using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour {

    public GameObject soundObstacleprefab;
    public GameObject rumbleObstaclePrefab;
    public GameObject playerGO;
    public Text screenText;
    public GameObject obstaclePrefab;

    private GameObject leftSO;
    private GameObject rightSO;
    private GameObject rumbleObstacle;
    private GameObject visualObstacle;

    private PlayerMovement playerMovement;

    private Transform leftSP;
    private Transform rightSP;
    private Transform rumblePickup;

    private Transform arrowHead;

    private bool passedLeftSP;
    private bool passedRightSP;
    private bool leftPickedUp;
    private bool ctr;
    private float checkPan;
    private bool passedRumble;
    private int whichUpdate;

    void Awake()
    {
        leftSO = Instantiate(soundObstacleprefab, new Vector3(playerGO.transform.position.x - 40.0f, playerGO.transform.position.y, playerGO.transform.position.z + 100.0f), Quaternion.identity) as GameObject;
        rightSO = Instantiate(soundObstacleprefab, new Vector3(playerGO.transform.position.x + 40.0f, playerGO.transform.position.y, leftSO.transform.position.z + leftSO.transform.localScale.z), Quaternion.identity) as GameObject;
        rumbleObstacle = Instantiate(rumbleObstaclePrefab) as GameObject;
        rumbleObstacle.SetActive(false);
        visualObstacle = Instantiate(obstaclePrefab) as GameObject;
        visualObstacle.SetActive(false);
    }

	// Use this for initialization
	void Start () 
    {
        playerMovement = playerGO.GetComponent<PlayerMovement>();

        leftSP = leftSO.transform.GetChild(0);
        rightSP = rightSO.transform.GetChild(0);
        rumblePickup = rumbleObstacle.transform.GetChild(0);
        arrowHead = playerGO.transform.GetChild(4);

        passedLeftSP = false;
        passedRightSP = false;
        leftPickedUp = false;
        ctr = true;
        passedRumble = false;

        checkPan = leftSO.transform.localScale.x / 10.0f;

        whichUpdate = 0;
	}
	
	// Update is called once per frame
	void Update () 
    {
        switch(whichUpdate)
        {
            case 0:
                SoundUpdate();
                break;

            case 1:
                RumbleUpdate();
                break;

            case 2:
                JumpUpdate();
                break;
        }
              
	}

    void SoundUpdate()
    {
        if (leftSP.position.z - playerGO.transform.position.z <= 0 && ctr)
        {
            ctr = false;
            passedLeftSP = true;
            leftPickedUp = true;
        }
        if (rightSP.position.z - playerGO.transform.position.z <= 0)
        {
            passedRightSP = true;
            screenText.text = "Good Job! You got it!";


            Invoke("SwitchToRumble", 2);
            
        }

        if (!passedLeftSP)
        {
            if (Mathf.Abs(leftSP.position.x - playerGO.transform.position.x) >= checkPan)
            {
                leftSO.transform.Translate(new Vector3(0.0f, 0.0f, 1.0f) * playerMovement.initialSpeed * Time.deltaTime, Space.World);
                screenText.text = "Follow the sound until you hear it on both ears";
                arrowHead.rotation = Quaternion.Lerp(arrowHead.rotation, Quaternion.Euler(new Vector3(90.0f, -90.0f, 0.0f)), 0.01f);
            }
            else
            {
                arrowHead.rotation = Quaternion.Lerp(arrowHead.rotation, Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)), 0.01f);
                screenText.text = "Good job! Keep going straight till you get the sound";
            }
        }

        if (leftPickedUp)
        {
            leftPickedUp = false;
            rightSO.transform.position = new Vector3(playerGO.transform.position.x + 40.0f, playerGO.transform.position.y, playerGO.transform.position.z);
        }

        if (!passedRightSP)
        {
            if (Mathf.Abs(rightSP.position.x - playerGO.transform.position.x) >= checkPan)
            {
                rightSO.transform.Translate(new Vector3(0.0f, 0.0f, 1.0f) * playerMovement.initialSpeed * Time.deltaTime, Space.World);
                if (passedLeftSP)
                {
                    arrowHead.rotation = Quaternion.Lerp(arrowHead.rotation, Quaternion.Euler(new Vector3(90.0f, 90.0f, 0.0f)), 0.01f);
                    screenText.text = "Follow the sound until you hear it on both ears";
                }
            }
            else
            {
                if (passedLeftSP)
                {
                    arrowHead.rotation = Quaternion.Lerp(arrowHead.rotation, Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)), 0.01f);
                    screenText.text = "Good job! Keep going straight till you get the sound";
                }
            }
        }
    }

    void SwitchToRumble()
    {
        whichUpdate = 1;
        rumbleObstacle.transform.position = new Vector3(playerGO.transform.position.x + 40.0f, playerGO.transform.position.y, playerGO.transform.position.z + 1000.0f);
        rumbleObstacle.SetActive(true);
    }

    void RumbleUpdate()
    {
        if (playerGO.transform.position.z > rumblePickup.position.z)
        {
            passedRumble = true;
            screenText.text = "Good job! You avoided the blackhole.";
            Invoke("SwitchToJump", 2);
        }

        else
        {
            screenText.text = "Avoid the pull force of the blackhole by moving left or right. The increasing vibrations are an incdicator of how close you are.";
            arrowHead.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
        }
    }

    void SwitchToJump()
    {
        visualObstacle.transform.position = new Vector3(playerGO.transform.position.x, 0.0f, playerGO.transform.position.z + 800.0f);
        visualObstacle.SetActive(true);
        whichUpdate = 2;
    }

    void JumpUpdate()
    {
        screenText.text = "Avoid the visual obstacles by moving away(using arrow keys) or jumping(space or 'A') over them.";
    }
}
