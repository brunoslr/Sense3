using UnityEngine;
using System.Collections;
using XInputDotNetPure;
using UnityStandardAssets.ImageEffects;

public class HapticObstacle : MonoBehaviour
{
    private float playerDist;
    private float endPos;
    private float totalDist;
    private float ratio;
    private GameObject player;
    private GameObject cam;
    private CameraMovement cameraMovement;
    private Twirl cameraTwirl;
    private PlayerMovement playerMovement;
    private Transform rumblePickup;
    private float currentPullforce;

    public float freq;
    public State state;
    public float minPullForce;
    public float maxPullForce;
    public float minBlur;
    public float maxBlur;
    public float minRadius;
    public float maxRadius;
  
    public enum State{ NEW, LOW, MED, HIGH};

	// Use this for initialization
	void Start () 
    {
        state = State.NEW;
        player = GameObject.Find("Player");
        cam =  GameObject.Find("MainCameraParent");
        cameraMovement = cam.GetComponent<CameraMovement>();
        playerMovement = player.GetComponent<PlayerMovement>();
        cameraTwirl = cam.GetComponentInChildren<Twirl>();
        cameraTwirl.radius.x = cameraTwirl.radius.y = minRadius;
        cameraTwirl.angle = minBlur;
        currentPullforce = minPullForce;
        rumblePickup = transform.GetChild(0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            return;
        }

        endPos = transform.position.z + transform.localScale.z / 2.0f;
        totalDist = Mathf.Abs(endPos - other.gameObject.transform.position.z);
        playerMovement.playerInsideMine = true;
        cameraTwirl.enabled = true;
        cameraTwirl.center = Camera.main.WorldToViewportPoint(rumblePickup.position);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            return;
        }

        CheckVibrationLevel(other);
        PullPlayerToCenter(other);
        cameraMovement.RumbleCamera();
        cameraTwirl.center = Camera.main.WorldToViewportPoint(rumblePickup.position);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            return;
        }

        StopAllCoroutines();
        GamePad.SetVibration(0, 0.0f, 0.0f);
        cameraTwirl.enabled = false;
    }

    void Update()
    {
        if (player.transform.position.z > endPos)
        {
            state = State.NEW;
            playerMovement.playerInsideMine = false;
            cameraTwirl.enabled = false;
        }
    }

    IEnumerator SetVibrationPWM(float activePortion, float freq)
    {
        float PWMDur = 1.0f / freq;
        float timeActive = activePortion * PWMDur;
        float timeInactive = PWMDur - timeActive;

        if (activePortion >= 1)
        {
            GamePad.SetVibration(0, 1.0f, 1.0f);
            yield break;
        }

        while (true)
        {
            GamePad.SetVibration(0, 1.0f, 1.0f);
            yield return new WaitForSeconds(timeActive);

            GamePad.SetVibration(0, 0.0f, 0.0f);
            yield return new WaitForSeconds(timeInactive);
        }
    }

    void CheckVibrationLevel(Collider other)
    {

        playerDist = Mathf.Abs(endPos - other.gameObject.transform.position.z);
        ratio = playerDist / totalDist;

        switch (state)
        {
            case State.NEW:
                state = State.LOW;
                cameraMovement.rumbleTilt = cameraMovement.rumbleShakeLow;
                StartCoroutine(SetVibrationPWM(0.4f, freq));
                currentPullforce = minPullForce;
                cameraTwirl.radius.x = cameraTwirl.radius.y = minRadius;
                cameraTwirl.angle = minBlur;
                rumblePickup.gameObject.GetComponent<MeshRenderer>().enabled = false;
                break;
            case State.LOW:
                if (ratio < 0.8f)
                {
                    state = State.MED;
                    cameraMovement.rumbleTilt = (cameraMovement.rumbleShakeLow + cameraMovement.rumbleShakeHigh) / 2.0f;
                    StopAllCoroutines();
                    GamePad.SetVibration(0, 0.0f, 0.0f);
                    StartCoroutine(SetVibrationPWM(0.8f, freq));
                    currentPullforce = (minPullForce + maxPullForce) / 2.0f;
                    cameraTwirl.radius.x = cameraTwirl.radius.y = (minRadius + maxRadius) / 2.0f;
                    cameraTwirl.angle = (minBlur + maxBlur) / 2.0f;
                }
                break;
            case State.MED:
                if (ratio < 0.4f)
                {
                    state = State.HIGH;
                    cameraMovement.rumbleTilt = cameraMovement.rumbleShakeHigh;
                    StopAllCoroutines();
                    GamePad.SetVibration(0, 0.0f, 0.0f);
                    StartCoroutine(SetVibrationPWM(1f, freq));
                    currentPullforce = maxPullForce;
                    cameraTwirl.radius.x = cameraTwirl.radius.y = maxRadius;
                    cameraTwirl.angle = maxBlur;
                    rumblePickup.gameObject.GetComponent<MeshRenderer>().enabled = true;
                }
                break;

            default:
                break;
        }
    }

    void PullPlayerToCenter(Collider other)
    {
        if(other.transform.position.x != transform.position.x)
            other.transform.position = Vector3.Lerp(other.transform.position, new Vector3(transform.position.x, other.transform.position.y, other.transform.position.z), currentPullforce);
    }

    void OnDestroy()
    {
        StopAllCoroutines();
        GamePad.SetVibration(0, 0.0f, 0.0f);
    }
}
