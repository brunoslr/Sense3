using UnityEngine;
using System.Collections;

public class LaneMovement : MonoBehaviour 
{
    public float sideSpeed;
    public float sideDisp;
    public float forwardspeed;
	
	// Update is called once per frame
	void Update () 
    {
        transform.Translate(transform.forward * forwardspeed * Time.deltaTime);
           
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(ShiftLane(-1));
        }

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(ShiftLane(1));
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
            yield return null;
        }
    }
}
