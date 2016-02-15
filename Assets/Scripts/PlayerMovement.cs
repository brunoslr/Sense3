using UnityEngine;
using System.Collections;
using XInputDotNetPure;

/// <summary>
/// This script goes on the player game object.
/// 
/// It deals with getting input from the user and controling the character's movement.
/// It was decided to not split this into two (characterController and inputController)
/// because the user can only control the player and the input only directly affects 
/// the character. All other events in the game are triggered by the character/environment 
/// interaction.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    public enum GameMode{ BOOST, CONSTINC, CONSTSPEED };

    public float initialSpeed;      // initial speed of player.
    public float sideSpeedMul;      // factor multiplied by forward speed to get side speed of player.
    public float vertSpeedMul;    // factor multiplied by forward speed to get up down speed of player
    public float speedMultiplier;   // factor multiplied to increase forward speed.
    public float sideSpeedInc;
    public float vertSpeedInc;
    public float horClamp;
    public float vertClamp;         // max vertical disp.
    public float horTilt;
    public float vertTilt;
    public float linezOffset;
    public float linexScale;

    private float forwardSpeed;     // current speed of the player at any point of time.
    private float initialSideSpeed;
    private float initialVertSpeed;
    private float finalSideSpeed;        // current side speed of the player at any point of time.
    private float finalVertSpeed;          // current up down speed of the player.
    private float curSideSpeedInc;
    private float curVertSpeedInc;
    private float startxPos;
    private float horAxis;
    private float vertAxis;

    public uint maxSpeedCounter;    // max no. of times speed can boost or increase.
    private uint speedCounter;      // current boost counter.
    public float trailTime;         // time of fire trail in sec.

    public GameMode gameMode;

    private SoundEffectsManager soundEffectsManager;

    private Transform playerModel;

    public GameObject camera;
    private CameraMovement cameraMovement;

    private LineRenderer lineRenderer;

    void Start()
    {
        ResetSpeed();
        if (gameMode == GameMode.BOOST)
        {
            CoreSystem.onSoundEvent += IncreasePlayerSpeed;
            CoreSystem.onObstacleEvent += ReducePlayerSpeed;
        }

        startxPos = transform.position.x;
        soundEffectsManager = this.GetComponent<SoundEffectsManager>();
        playerModel = this.transform;
        cameraMovement = camera.GetComponent<CameraMovement>();
        lineRenderer = this.GetComponent<LineRenderer>();
    }
   public void ResetSpeed()
    {
        speedCounter = 0;
        forwardSpeed = initialSpeed;
        initialSideSpeed = 0.0f;
        initialVertSpeed = 0.0f;
        finalSideSpeed = forwardSpeed * sideSpeedMul;
        finalVertSpeed = forwardSpeed * vertSpeedMul;
        curSideSpeedInc = sideSpeedInc;
        curVertSpeedInc = vertSpeedInc;
    }
    // Update is called once per frame
    void Update()
    {
        MovePlayerForward();
        MovePlayerSideways();
        MovePlayerVertical();
    }

    void LateUpdate()
    {
        lineRenderer.SetPosition(0, new Vector3(this.transform.position.x - linexScale, this.transform.position.y, this.transform.position.z + linezOffset));
        lineRenderer.SetPosition(1, new Vector3(this.transform.position.x + linexScale, this.transform.position.y, this.transform.position.z + linezOffset));
    }

    void MovePlayerForward()
    {
        transform.Translate(new Vector3(0.0f, 0.0f, 1.0f) * forwardSpeed * Time.deltaTime, Space.World);
    }

    void FixedUpdate()
    {
        if (gameMode == GameMode.CONSTINC)
            IncreasePlayerSpeed();
    }

    void MovePlayerSideways()
    {
        horAxis = Input.GetAxis("Horizontal");
        if (horAxis == 0.0f)
        {
            initialSideSpeed = 0.0f;
            curSideSpeedInc = sideSpeedInc;
            cameraMovement.RotateCamera(0.0f);
        }
        else
        {
            if (transform.position.x > (startxPos + horClamp))
                transform.position = new Vector3(startxPos + horClamp, transform.position.y, transform.position.z);

            if (transform.position.x < (startxPos - horClamp))
                transform.position = new Vector3(startxPos - horClamp, transform.position.y, transform.position.z);

            if (initialSideSpeed < finalSideSpeed)
                initialSideSpeed += curSideSpeedInc;
      
            else
                initialSideSpeed = finalSideSpeed;
            

            transform.rotation = Quaternion.Euler(0.0f, 0.0f, -horAxis * horTilt);
            transform.Translate(new Vector3(1.0f, 0.0f, 0.0f) * horAxis * initialSideSpeed * Time.deltaTime, Space.World);
            cameraMovement.RotateCamera(horAxis);

            soundEffectsManager.MovePlayerSound();
        }
    }

    void MovePlayerVertical()
    {
        vertAxis = Input.GetAxis("Vertical");
        if (vertAxis == 0.0f)
        {
            //MovePlayerBackToCenter();
            initialVertSpeed = 0.0f;
            curVertSpeedInc = vertSpeedInc;
        }
        else
        {
            if (transform.position.y > vertClamp)
            {
                transform.position = new Vector3(transform.position.x, vertClamp, transform.position.z);
            }

            if (transform.position.y < -vertClamp)
            {
                transform.position = new Vector3(transform.position.x, -vertClamp, transform.position.z);
            }

            if (initialVertSpeed < finalVertSpeed)
            {
                initialVertSpeed += curVertSpeedInc;
            }
            else
                initialVertSpeed = finalVertSpeed;

            transform.rotation = Quaternion.Euler(-vertAxis * vertTilt, 0.0f, 0.0f);
            transform.Translate(new Vector3(0.0f, 1.0f, 0.0f) * vertAxis * initialVertSpeed * Time.deltaTime, Space.World);
            soundEffectsManager.MovePlayerSound();
        }
    }
    void MovePlayerBackToCenter()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 0.0f, transform.position.z), 0.01f);
    }

    public void IncreasePlayerSpeed()
    {
        if (speedCounter < maxSpeedCounter)
        {
            ++speedCounter;
            forwardSpeed = (1.0f + (speedMultiplier * speedCounter)) * initialSpeed;
            finalSideSpeed = forwardSpeed * sideSpeedMul;
            finalVertSpeed = forwardSpeed * vertSpeedMul;
        }
    }

    public void ReducePlayerSpeed()
    {
        if (speedCounter > 0)
        {
            --speedCounter;
            forwardSpeed = (1.0f + (speedMultiplier * speedCounter)) * initialSpeed;
            finalSideSpeed = forwardSpeed * sideSpeedMul;
            finalVertSpeed = forwardSpeed * vertSpeedMul;
        }
    }

    IEnumerator EndTrail()
    {
        yield return new WaitForSeconds(trailTime);
        this.gameObject.GetComponent<TrailRenderer>().enabled = false;
    }
}