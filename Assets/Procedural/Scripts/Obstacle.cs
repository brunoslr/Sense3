using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {
    private Transform rotator;

    private void Awake()
    {
        rotator = transform.GetChild(0);
    }

    public void Position (PipeInstance pipeInstance, float curveRotation, float ringRotation)
    {
        transform.SetParent(pipeInstance.transform, false);
        transform.localRotation = Quaternion.Euler(0f, 0f, -curveRotation);
        rotator.localPosition = new Vector3(0f, pipeInstance.CurveRadius);
        rotator.localRotation = Quaternion.Euler(ringRotation, 0f, 0f);
    }
}
