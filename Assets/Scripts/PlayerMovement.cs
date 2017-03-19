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

    public float initialSpeed = 100;      // initial speed of player.
    public float sideSpeedMul = 1;      // factor multiplied by forward speed to get side speed of player.
    public float vertSpeedMul = 0.1f;    // factor multiplied by forward speed to get up down speed of player
    public float speedMultiplier = 0.25f;   // factor multiplied to increase forward speed.
    public float sideSpeedInc = 1;
    public float vertSpeedInc = 0.1f;
    public float horClamp = float.MaxValue;
    public float vertClamp = 7;         // max vertical disp.
    public float horTilt = 30;
    public float moveFactor;
    public float vertTilt = 5;
    public float linezOffset = 200;
    public float linexScale = 200;
    public float jumpVel;
    public float slowDownFactor;

    public uint maxSpeedCounter = 5;    // max no. of times speed can boost or increase.
    public float trailTime = 2;         // time of fire trail in sec.
    [HideInInspector] public uint speedCounter;      // current boost counter.

    public GameMode gameMode = GameMode.BOOST;
    public GameObject mainCamera;


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
    private bool isGrounded;
    private int collisionTempTime;
    private bool coolDownflag = false;
   
    private Rigidbody rigidBody;

    private SoundEffectsManager soundEffectsManager;

    private CameraMovement cameraMovement;

    private LineRenderer lineRenderer;

    [HideInInspector]public bool playerInsideMine;

    void Start()
    {
        ResetSpeed();
        if (gameMode == GameMode.BOOST)
        {
            CoreSystem.onSoundEvent += IncreasePlayerSpeed;
            CoreSystem.onObstacleEvent += ReducePlayerSpeed;
        }

        startxPos = transform.position.x;
        soundEffectsManager = GetComponent<SoundEffectsManager>();
        rigidBody = this.GetComponent<Rigidbody>();
        cameraMovement = mainCamera.GetComponent<CameraMovement>();
        lineRenderer = this.GetComponent<LineRenderer>();
        isGrounded = true;
        collisionTempTime = CoreSystem.coolDownTimeInSeconds;
        coolDownflag = false;   
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
        CheckPlayerGrounded();
    }

    void FixedUpdate()
    {
        if (gameMode == GameMode.CONSTINC)
            IncreasePlayerSpeed();

        if (Input.GetButton("Jump") && isGrounded)
        {
            JumpPlayer();
        }
    }

    void LateUpdate()
    {
        lineRenderer.SetPosition(0, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 0.5f));
        lineRenderer.SetPosition(1, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + linezOffset));
    }

    void MovePlayerForward()
    {
        transform.Translate(new Vector3(0.0f, 0.0f, 1.0f) * forwardSpeed * Time.deltaTime, Space.World);
    }

    void MovePlayerSideways()
    {
        horAxis = Input.GetAxis("Horizontal");
        if (horAxis == 0.0f )
        {   
            initialSideSpeed = 0.0f;
            curSideSpeedInc = sideSpeedInc;
            if(!playerInsideMine)
                cameraMovement.RotateCamera(0.0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0.0f, 0.0f, 0.0f), moveFactor);
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

            if (horAxis < 0.0f)
            {
                soundEffectsManager.MovePlayerLeftSound();
            }

            if (horAxis > 0.0f)
            {
                soundEffectsManager.MovePlayerRightSound();
            }
                horAxis = horAxis / Mathf.Abs(horAxis);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0.0f, 0.0f, -horAxis * horTilt), moveFactor);
            transform.Translate(new Vector3(1.0f, 0.0f, 0.0f) * horAxis * initialSideSpeed * Time.deltaTime, Space.World);
            cameraMovement.RotateCamera(horAxis);
        }
    }

    void MovePlayerVertical()
    {
        vertAxis = Input.GetAxis("Vertical");
        if (vertAxis == 0.0f)
        {
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
        }
    }

    void JumpPlayer()
    {
        isGrounded = false;
        rigidBody.AddForce(Vector3.up * jumpVel, ForceMode.VelocityChange);
        rigidBody.useGravity = true;
        soundEffectsManager.JumpPlayerSound();
    }

    void CheckPlayerGrounded()
    {
        if (transform.position.y < 0.0f)
        {
            rigidBody.useGravity = false;

            transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
        
            isGrounded = true;
        }
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
        if (!coolDownflag)
        {
            if (speedCounter > 0)
            {
                --speedCounter;
                forwardSpeed = (1.0f + (speedMultiplier * speedCounter)) * initialSpeed;
                finalSideSpeed = forwardSpeed * sideSpeedMul;
                finalVertSpeed = forwardSpeed * vertSpeedMul;
            }
            coolDownflag = true;
            if (PlayerStateScript.GetPlayerLevel() >= 0)
            {
                StartCoroutine(CoolDown());
            }
        }
    }

    IEnumerator CoolDown()
    {
        forwardSpeed *= slowDownFactor;
        yield return new WaitForSeconds(collisionTempTime);
        forwardSpeed /= slowDownFactor;
        coolDownflag = false;
    }

    IEnumerator EndTrail()
    {
        yield return new WaitForSeconds(trailTime);
        this.gameObject.GetComponent<TrailRenderer>().enabled = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            CoreSystem.ExecuteOnObstacleCollision();
        }
    }
}