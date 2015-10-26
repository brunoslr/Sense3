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
    float horizontalStep;
    Vector3 tempTrans;
    Quaternion temprot;
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
            forwardspeed = (1.0f + (0.75f * hitCounter)) * baseSpeed;
            this.gameObject.GetComponentInChildren<AudioController>().incrementCounter();
        }
    }

    public void hit()
    {
        if (hitCounter > 0)
        {
            hitCounter -= 1;
            forwardspeed = (1.0f + (0.75f * hitCounter)) * baseSpeed;
            this.gameObject.GetComponentInChildren<AudioController>().decrementCounter();
        }
    }

	// Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * forwardspeed * Time.deltaTime);

        horizontalAxis = Input.GetAxis("Horizontal");

        horizontalStep = horizontalAxis * sideSpeed * Time.deltaTime;
        tempTrans = new Vector3(horizontalStep, 0, 0);
        transform.Translate(tempTrans,Space.World);
        temprot = Quaternion.Euler(0, 0, -horizontalAxis * tilt);
        transform.rotation = temprot;
        if(Input.GetButtonDown("Jump") && getJumpInput)
        {
            getJumpInput = false;
            StartCoroutine(JumpShip());
        }
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
