using UnityEngine;
using System.Collections;
using XInputDotNetPure;
using UnityStandardAssets.ImageEffects;

public class MineObstacle : MonoBehaviour
{
    public float freq;
    private float playerDist;
    private float endPos;
    private float totalDist;
    private float ratio;
    public State state;
    private GameObject player;
    public float pullForce;
    private GameObject camera;
    private CameraMovement cameraMovement;
    private Twirl cameraTwirl;
    private PlayerMovement playerMovement;
    private Transform rumblePickup;
    public enum State{ NEW, LOW, MED, HIGH};

	// Use this for initialization
	void Start () 
    {
        state = State.NEW;
        player = GameObject.Find("Player");
        camera =  GameObject.Find("MainCameraParent");
        cameraMovement = camera.GetComponent<CameraMovement>();
        playerMovement = player.GetComponent<PlayerMovement>();
        cameraTwirl = camera.GetComponent<Twirl>();
        cameraTwirl.radius.x = cameraTwirl.radius.y = 0.1f;
        cameraTwirl.angle = 150.0f;
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

        //var p = other.gameObject.transform.position;
        //p.z = endPos;

        //Color c = state == State.LOW ? Color.green
        //    : state == State.MED ? Color.yellow
        //    : state == State.HIGH ? Color.red
        //    : Color.magenta;

        //Debug.DrawLine(p, other.gameObject.transform.position, c);

        playerDist = Mathf.Abs(endPos - other.gameObject.transform.position.z);
        ratio = playerDist / totalDist;

        switch (state)
        {
            case State.NEW:
                state = State.LOW;
                StartCoroutine(SetVibrationPWM(0.4f, freq));
                break;
            case State.LOW:
                if (ratio < 0.8f)
                {
                    state = State.MED;
                    StopAllCoroutines();
                    GamePad.SetVibration(0, 0.0f, 0.0f);
                    StartCoroutine(SetVibrationPWM(0.8f, freq));
                }
                break;
            case State.MED:
                if (ratio < 0.4f)
                {
                    state = State.HIGH;
                    StopAllCoroutines();
                    GamePad.SetVibration(0, 0.0f, 0.0f);
                    StartCoroutine(SetVibrationPWM(1f, freq));
                }
                break;
            case State.HIGH:

                break;
            default:
                break;
        }
    }

    void PullPlayerToCenter(Collider other)
    {
        if(other.transform.position.x != transform.position.x)
            other.transform.position = Vector3.Lerp(other.transform.position, new Vector3(transform.position.x, other.transform.position.y, other.transform.position.z), pullForce);
    }

    void OnDestroy()
    {
        StopAllCoroutines();
        GamePad.SetVibration(0, 0.0f, 0.0f);
    }
}
