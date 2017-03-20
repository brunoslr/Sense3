using UnityEngine;
using System.Collections;

public class UIMainMenu : MonoBehaviour
{
    public GameObject rotationPoint;
    private float angularVelocity = 0.4f;

    void Update()
    {
        if (this.gameObject.transform.position.z < 310)
            this.transform.parent.GetComponent<UIAnimation>().currentItem = this.gameObject;
        
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.RotateAround(rotationPoint.transform.position, Vector3.up, angularVelocity);
            transform.forward = new Vector3(0, 0, 1);
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            transform.RotateAround(rotationPoint.transform.position, Vector3.up, -angularVelocity);
            transform.forward = new Vector3(0, 0, 1);
        }
    }
}