using UnityEngine;
using System.Collections;
using XInputDotNetPure;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]

public class MineObstacle : MonoBehaviour
{
    public GameObject Spikes;
    Spectrum _spectrum;
	// Use this for initialization
	void Start () {
        _spectrum = GameObject.Find("BackgroundCamera").GetComponent<Spectrum>();
        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        if (this.gameObject.transform.childCount > 2)
            Spikes = this.gameObject.transform.GetChild(1).gameObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartVibration();
            if (Mathf.Abs(this.gameObject.transform.GetChild(0).position.z - other.gameObject.transform.position.z) < 8f)
            {
                Spikes.SetActive(true);
                LetsBounce();
                //other.gameObject.GetComponent<LaneMovement>().hit();
            }
        }
    }


    void LetsBounce()
    {
        for (int i = 0; i < 15; i++)
        {
            Vector3 prevScale = Spikes.transform.GetChild(i).localScale;
            prevScale.y = Mathf.Lerp(prevScale.y, _spectrum.spectrum[Random.Range(0, 10)] * 50, Time.deltaTime * 10);
            Spikes.transform.GetChild(i).localScale = prevScale;
            Spikes.transform.position = new Vector3(Spikes.transform.position.x, prevScale.y / 2 + 1, Spikes.transform.position.z);
        }
       
    }
    void OnTriggerExit(Collider other)
    {
        StopVibration();
    }

    void StartVibration()
    {
        GamePad.SetVibration(0, 1.0f, 1.0f);
    }

    void StopVibration()
    {
        GamePad.SetVibration(0, 0.0f, 0.0f);
    }
}
