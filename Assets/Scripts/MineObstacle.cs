using UnityEngine;
using System.Collections;
using XInputDotNetPure;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]

public class MineObstacle : MonoBehaviour {

	// Use this for initialization
	void Start () {
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
        StopVibration();
    }

    IEnumerator StartVibration()
    {
        float increment = 0.0f;
        while (increment <= 1.0f)
        {
            GamePad.SetVibration(0, increment, increment);
            increment += 0.01f;
            yield return null;
        }
        StopVibration();
    }

    void StopVibration()
    {
        GamePad.SetVibration(0, 0.0f, 0.0f);
    }
}
