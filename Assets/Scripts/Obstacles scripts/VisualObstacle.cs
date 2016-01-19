using UnityEngine;
using System.Collections;

public class VisualObstacle : MonoBehaviour {
    public float sideSpeed;
    public float sideDisp;
    public float forwardspeed;
    public int probabilityOfMovingAgain;
    public int checkIfCanMoveAfterUpdates;

    // public GameObject vibrate;
    private bool canMove;
    private int tick;
    private int move;
    private int rand;

    void Start()
    {
        canMove = true;
        tick = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * -forwardspeed * Time.deltaTime);
        tick++;
        if (tick % checkIfCanMoveAfterUpdates == 0)
        {
            rand = Random.Range(0, 2);
            move = Random.Range(1, 11);
        }

        if (rand == 0 && canMove && move <= probabilityOfMovingAgain)
        {
            canMove = false;
            StartCoroutine(ShiftLane(-1));
            move = 0;
            rand = -1;
        }
        else if(rand == 1 && canMove && move <= probabilityOfMovingAgain)
        {
            canMove = false;
            StartCoroutine(ShiftLane(1));
            move = 0;
            rand = -1;
        }
    }

    IEnumerator ShiftLane(int moveDirection)
    {
        float displacement = Mathf.Clamp(transform.position.x + (moveDirection * sideDisp), -sideDisp, +sideDisp);
        while (Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(displacement)) > 0.0f)
        {
            float step = sideSpeed * Time.deltaTime;
            Vector3 target = new Vector3(displacement, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            yield return null;
        }
        canMove = true; 
    }
}
