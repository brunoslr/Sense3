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

    private float forwardSpeed;     // current speed of the player at any point of time.
    private float initialSideSpeed;
    private float initialVertSpeed;
    private float finalSideSpeed;        // current side speed of the player at any point of time.
    private float finalVertSpeed;          // current up down speed of the player.
    private float curSideSpeedInc;
    private float curVertSpeedInc;

    public uint maxSpeedCounter;    // max no. of times speed can boost or increase.
    private uint speedCounter;      // current boost counter.
    public float trailTime;         // time of fire trail in sec.

    public GameMode gameMode;

    void Start()
    {
        ResetSpeed();
        if (gameMode == GameMode.BOOST)
        {
            CoreSystem.onSoundEvent += IncreasePlayerSpeed;
            CoreSystem.onObstacleEvent += ReducePlayerSpeed;
        }
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

    void MovePlayerForward()
    {
        transform.Translate(transform.forward * forwardSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (gameMode == GameMode.CONSTINC)
            IncreasePlayerSpeed();
    }

    void MovePlayerSideways()
    {
        if (Input.GetAxis("Horizontal") == 0.0f)
        {
            initialSideSpeed = 0.0f;
            curSideSpeedInc = sideSpeedInc;
        }
        else
        {
            if (transform.position.x > horClamp)
                transform.position = new Vector3(horClamp, transform.position.y, transform.position.z);

            if (transform.position.x < -horClamp)
                transform.position = new Vector3(-horClamp, transform.position.y, transform.position.z);

            if (initialSideSpeed < finalSideSpeed)
                initialSideSpeed += curSideSpeedInc;
      
            else
                initialSideSpeed = finalSideSpeed;

            transform.Translate(transform.right * Input.GetAxis("Horizontal") * initialSideSpeed * Time.deltaTime);
        }
    }

    void MovePlayerVertical()
    {
        if (Input.GetAxis("Vertical") == 0.0f)
        {
            MovePlayerBackToCenter();
            initialVertSpeed = 0.0f;
            curVertSpeedInc = vertSpeedInc;
        }
        else
        {
            if (transform.position.y > vertClamp)
                transform.position = new Vector3(transform.position.x, vertClamp, transform.position.z);

            if (transform.position.y < -vertClamp)
                transform.position = new Vector3(transform.position.x, -vertClamp, transform.position.z);

            if (initialVertSpeed < finalVertSpeed)
            {
                initialVertSpeed += curVertSpeedInc;
            }
            else
                initialVertSpeed = finalVertSpeed;

            transform.Translate(transform.up * Input.GetAxis("Vertical") * initialVertSpeed * Time.deltaTime);
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