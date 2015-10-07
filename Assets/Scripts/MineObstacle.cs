using UnityEngine;
using System.Collections;
using XInputDotNetPure;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]

public class MineObstacle : MonoBehaviour {

    private IEnumerator stopVibration;
    private bool breaknow;
	// Use this for initialization
	void Start () {
        stopVibration = StartVibration();
        breaknow = false;
        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(StartVibration());
        }
    }

    void OnTriggerExit(Collider other)
    {
        //StopCoroutine(stopVibration);
        StopVibration();
    }

    IEnumerator StartVibration()
    {
        float increment = 0.0f;
        while (increment <= 1.0f)
        {
            GamePad.SetVibration(0, increment, increment);
            increment += 0.01f;
            if(breaknow)
            {
                break;
            }
            yield return null;
        }
        StopVibration();
        breaknow = false;
    }

    void StopVibration()
    {
        breaknow = true;
        StopCoroutine(stopVibration);
        GamePad.SetVibration(0, 0.0f, 0.0f);
    }
}
