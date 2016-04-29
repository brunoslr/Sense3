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

        if (this.gameObject.transform.position.z < 310)
            this.transform.parent.GetComponent<UIAnimation>().currentItem = this.gameObject;


        if (Input.GetAxis("Horizontal") >0)
        {
            transform.RotateAround(center.transform.position, Vector3.up, 0.2f);
            transform.forward = new Vector3(0, 0, 1);
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            transform.RotateAround(center.transform.position, Vector3.up, -0.2f);
            transform.forward = new Vector3(0, 0, 1);
        }
    }
}