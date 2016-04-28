using UnityEngine;
using System.Collections;

public class UIMainMenu : MonoBehaviour
{
    public GameObject center;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") >0)
        {
            transform.RotateAround(center.transform.position, Vector3.up, 1);
            transform.forward = new Vector3(0, 0, 1);
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            transform.RotateAround(center.transform.position, Vector3.up, -1);
            transform.forward = new Vector3(0, 0, 1);
        }
    }
}