using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class LaneMovement : MonoBehaviour 
{
    public float sideSpeed;
    public float sideDisp;
    public float forwardspeed;
    public float tilt;
    public float jumpDisp;
    public float jumpSpeed;

    private uint hitCounter;
    private float baseSpeed;
   // public GameObject vibrate;

    private float horizontalAxis;
    private bool getSideInput;
    private bool getJumpInput;

    void Start()
    {
        hitCounter = 0;
        baseSpeed = forwardspeed;
        getSideInput = true;
        getJumpInput = true;
    }

    public void pickUp()
    {
        if (hitCounter < 4)
        {
            hitCounter += 1;
            Debug.Log(hitCounter);
            forwardspeed = (1.0f + (0.75f * hitCounter)) * baseSpeed;
            Debug.Log(baseSpeed);
            this.gameObject.GetComponentInChildren<AudioController>().incrementCounter();
        }
    }

    public void hit()
    {
        if (hitCounter > 0)
        {
            hitCounter -= 1;
            Debug.Log(hitCounter);
            forwardspeed = (1.0f + (0.75f * hitCounter)) * baseSpeed;
            this.gameObject.GetComponentInChildren<AudioController>().decrementCounter();
        }
    }

	// Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * forwardspeed * Time.deltaTime);

        horizontalAxis = Input.GetAxis("Horizontal");

        if (horizontalAxis < 0 && getSideInput) 
        {
            getSideInput = false;
            StartCoroutine(ShiftLane(-1));
        }

        if (horizontalAxis > 0 && getSideInput)
        {
            getSideInput = false;
            StartCoroutine(ShiftLane(1));
        }

        if(Input.GetButtonDown("Jump") && getJumpInput)
        {
            getJumpInput = false;
            StartCoroutine(JumpShip());
        }
    }

    IEnumerator ShiftLane(int moveDirection)
    {
        float displacement = Mathf.Clamp(transform.position.x + (moveDirection * sideDisp), -sideDisp, +sideDisp);
        while (Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(displacement)) > 0.0f)
        {
            float step = sideSpeed * Time.deltaTime;
            Vector3 target = new Vector3(displacement, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, target , step);
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, -moveDirection * tilt);
            yield return null;
        }

        getSideInput = true;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }

    IEnumerator JumpShip()
    {
        while(transform.position.y < jumpDisp)
        {
            float step = jumpSpeed * Time.deltaTime;
            transform.Translate(0.0f, step, 0.0f);
            yield return null;
        }

        while(transform.position.y > 1.0f)
        {
            float step = -jumpSpeed * Time.deltaTime;
            transform.Translate(0.0f, step, 0.0f);
            yield return null;
        }

        getJumpInput = true;
    }
}
