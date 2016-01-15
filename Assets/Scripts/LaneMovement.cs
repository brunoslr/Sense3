using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class LaneMovement : MonoBehaviour 
{
    public Camera mainCamera;
    public float sideSpeed;
    public float sideDisp;
    public float forwardspeed;
    public float speedMultiplier;
    public float tilt;
    public float jumpDisp;
    public float jumpSpeed;
    public float trailTime = 2.0f;
    private uint hitCounter;
    private float baseSpeed;
    //public GameObject vibrate;

    private float horizontalAxis;
    //private bool getSideInput;
    private bool getJumpInput;
    float horizontalStep;
    Vector3 tempTrans;
    Quaternion temprot;
    RaycastHit rHit;
    Rigidbody rgbody;
    //float distanceToGround = 0;
    void Start()
    {
        hitCounter = 0;
        baseSpeed = forwardspeed;
        //getSideInput = true;
        getJumpInput = true;
        rgbody = this.gameObject.GetComponent<Rigidbody>();
    }

    public void pickUp()
    {
        if (hitCounter < 4)
        {
            hitCounter += 1;
            forwardspeed = (1.0f + (speedMultiplier * hitCounter)) * baseSpeed;
            sideDisp = forwardspeed * 2.0f;
        }
            this.gameObject.GetComponent<TrailRenderer>().enabled = true;
            StartCoroutine(endTrail());
            //Disable this after refactoring
            //this.gameObject.GetComponentInChildren<AudioController>().incrementCounter();
            //mainCamera.gameObject.GetComponent<ForeGroundController>().startBlur();
    }

    public void hit()
    {
        if (hitCounter > 0)
        {
            hitCounter -= 1;
            forwardspeed = (1.0f + (speedMultiplier * hitCounter)) * baseSpeed;
            sideDisp = forwardspeed * 2.0f;
            //Disable this after refactoring
            //this.gameObject.GetComponentInChildren<AudioController>().decrementCounter();
        }
        //mainCamera.gameObject.GetComponent<ForeGroundController>().setFishEye(1.0f);

    }

	// Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * forwardspeed * Time.deltaTime);

        horizontalAxis = Input.GetAxis("Horizontal");

        horizontalStep = horizontalAxis * sideSpeed * Time.deltaTime;
        tempTrans = new Vector3(horizontalStep, 0, 0);
         if(rgbody.useGravity==false)
        transform.Translate(tempTrans,Space.World);
        temprot = Quaternion.Euler(0, 0, -horizontalAxis * tilt);
        transform.rotation = temprot;
        if(Input.GetButtonDown("Jump") && getJumpInput)
        {
            getJumpInput = false;
            StartCoroutine(JumpShip());
  
        }



        if (Physics.Raycast(transform.position, -Vector3.up, out rHit, 5.0F))
        {
            //distanceToGround = rHit.distance;
       
        }
        else
            if ((getJumpInput == true))
                rgbody.useGravity = true;
  
      
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

    IEnumerator endTrail()
    {
        yield return new WaitForSeconds(trailTime);
        this.gameObject.GetComponent<TrailRenderer>().enabled = false;
    }
}
