using UnityEngine;
using System.Collections;

public class MWObstacle : MonoBehaviour {
    public float currentAngle;
    public float targetAngle;
    public bool posNegLerp;
    private Quaternion start;
    private Quaternion end;
    public float speed;
    // Use this for initialization

    private float time;
    private int dir;

    void Start () {
        currentAngle = transform.localEulerAngles.z;
        if (posNegLerp)
        {
            targetAngle = -currentAngle;
        }

        start = Quaternion.AngleAxis(currentAngle, Vector3.forward);
        end = Quaternion.AngleAxis(targetAngle, Vector3.forward);
    }
	
	// Update is called once per frame
	void Update () {
        //currentAngle = transform.rotation.z;
        //if (currentAngle <= -45)
        //{
        //    targetAngle = 45;
        //    start = Quaternion.Euler(new Vector3(0,0, currentAngle));
        //    end = Quaternion.Euler(new Vector3(0,0,targetAngle));
        //}
        //else if (currentAngle >= 45)
        //{
        //    targetAngle = -45;
        //    start = Quaternion.Euler(new Vector3(0, 0, currentAngle));
        //    end = Quaternion.Euler(new Vector3(0, 0, targetAngle));
        //}

        time += Time.deltaTime * speed * dir;

        if (time >= 1)
        {
            dir = -1;
        }

    else if(time <=0)
        {
            dir = 1;
        }

        transform.rotation = Quaternion.Lerp(start, end, time);
	}
}
