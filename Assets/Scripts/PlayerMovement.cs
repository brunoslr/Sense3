﻿using UnityEngine;
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
    public float initialSpeed;      // initial speed of player.
    public float sideSpeedMul;      // factor multiplied by forward speed to get side speed of player.
    public float vertSpeedMul;    // factor multiplied by forward speed to get up down speed of player
    public float speedMultiplier;   // factor multiplied to increase forward speed.

    private float forwardSpeed;     // current speed of the player at any point of time.
    private float sideSpeed;        // current side speed of the player at any point of time.
    private float vertSpeed;          // current up down speed of the player.

    public uint maxSpeedCounter;    // max no. of times speed can boost or increase.
    private uint speedCounter;      // current boost counter.

    public float trailTime;         // time of fire trail in sec.
    public float vertClamp;         // max vertical disp.

    void Start()
    {
        speedCounter = 0;
        forwardSpeed = initialSpeed;
        sideSpeed = forwardSpeed * sideSpeedMul;
        vertSpeed = forwardSpeed * vertSpeedMul;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayerForward();
        MovePlayerSideways();
        MovePlayerVertical();
        MovePlayerBackToCenter();
    }

    void MovePlayerForward()
    {
        transform.Translate(transform.forward * forwardSpeed * Time.deltaTime);
    }

    void MovePlayerSideways()
    {
        transform.Translate(transform.right * Input.GetAxis("Horizontal") * sideSpeed * Time.deltaTime);
    }

    void MovePlayerVertical()
    {
        if (transform.position.y > vertClamp)
            transform.position = new Vector3(transform.position.x, vertClamp, transform.position.z);

        if (transform.position.y < -vertClamp)
            transform.position = new Vector3(transform.position.x, -vertClamp, transform.position.z);

        transform.Translate(transform.up * Input.GetAxis("Vertical") * vertSpeed * Time.deltaTime);
    }

    void MovePlayerBackToCenter()
    {
        if (Input.GetAxis("Vertical") == 0.0f)
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 1.0f, transform.position.z), 0.01f);
    }

    public void IncreasePlayerSpeed()
    {
        if (speedCounter < maxSpeedCounter)
        {
            ++speedCounter;
            forwardSpeed = (1.0f + (speedMultiplier * speedCounter)) * initialSpeed;
            sideSpeed = forwardSpeed * sideSpeedMul;
            vertSpeed = forwardSpeed * vertSpeedMul;
        }
    }

    public void ReducePlayerSpeed()
    {
        if (speedCounter > 0)
        {
            --speedCounter;
            forwardSpeed = (1.0f + (speedMultiplier * speedCounter)) * initialSpeed;
            sideSpeed = forwardSpeed * sideSpeedMul;
            vertSpeed = forwardSpeed * vertSpeedMul;
        }
    }

    IEnumerator EndTrail()
    {
        yield return new WaitForSeconds(trailTime);
        this.gameObject.GetComponent<TrailRenderer>().enabled = false;
    }
}