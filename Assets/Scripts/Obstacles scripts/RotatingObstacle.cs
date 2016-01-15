using UnityEngine;
using System.Collections;

/// <summary>
/// To Do refactor entire class, create proper functions with correct responsibility, add coments to each and every function
/// </summary>
public class RotatingObstacle : MonoBehaviour
{
    public float currentAngle;
    public float targetAngle;
    public bool posNegLerp;
    private Quaternion start;
    private Quaternion end;
    public float speed, distanceFromObs;
    private GameObject player;
    public bool moveIfPlayerClose = false;
    // Use this for initialization

    private float time;
    private float time1;
    private int dir;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform.FindChild("engines_player").gameObject;
        currentAngle = transform.localEulerAngles.z;
        if (posNegLerp)
        {
            targetAngle = -currentAngle;
        }

        start = Quaternion.AngleAxis(currentAngle, Vector3.forward);
        end = Quaternion.AngleAxis(targetAngle, Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {
        if(moveIfPlayerClose && Mathf.Abs(transform.position.z - player.transform.position.z) < distanceFromObs)
        {
            time += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(start, end, time * speed);
        }
        else if(!moveIfPlayerClose)
        {
            time += Time.deltaTime * speed * dir;

            if (time >= 1)
            {
                dir = -1;
            }

            else if (time <= 0)
            {
                dir = 1;
            }

            transform.rotation = Quaternion.Lerp(start, end, time);
        }
    }
}
