using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class LaneMovement : MonoBehaviour 
{
    public float sideSpeed;
    public float sideDisp;
    public float forwardspeed;

   // public GameObject vibrate;

    private float horizontalAxis;
    private bool getInput;

    void Start()
    {
        getInput = true;
    }

	// Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * forwardspeed * Time.deltaTime);

        horizontalAxis = Input.GetAxis("Horizontal");
       
        if (horizontalAxis < 0 && getInput) 
        {
            getInput = false;
            StartCoroutine(ShiftLane(-1));

        }

        if (horizontalAxis > 0 && getInput)
        {
            getInput = false;
            StartCoroutine(ShiftLane(1));
        }

        //if (vibrate.transform.position.z - transform.position.z < 0.0f)
        //{
        //    StopVibration();
        //}

        //else if(vibrate.transform.position.z - transform.position.z < 10.0f)
        //{
        //    StartCoroutine(StartVibration());
        //}
    }

    IEnumerator ShiftLane(int moveDirection)
    {
        float displacement = Mathf.Clamp(transform.position.x + (moveDirection * sideDisp), -sideDisp, +sideDisp);
        while (Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(displacement)) > 0.0f)
        {
            float step = sideSpeed * Time.deltaTime;
            Vector3 target = new Vector3(displacement, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, target , step);
            yield return null;
        }

        getInput = true;
    }

    //IEnumerator StartVibration()
    //{
    //    float increment = 0.0f;
    //    while (increment <= 1.0f)
    //    {
    //        GamePad.SetVibration(0, increment, increment);
    //        increment += 0.01f;
    //        yield return null;
    //    }
    //}

    //void StopVibration()
    //{
    //    GamePad.SetVibration(0, 0.0f, 0.0f);
    //}
}
