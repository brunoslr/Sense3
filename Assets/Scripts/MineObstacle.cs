using UnityEngine;
using System.Collections;
using XInputDotNetPure;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]

public class MineObstacle : MonoBehaviour {

    private IEnumerator stopVibration;
    private bool breaknow;
    public GameObject Spikes;
    Spectrum _spectrum;
	// Use this for initialization
	void Start () {
        _spectrum = GameObject.Find("BGCamera").GetComponent<Spectrum>();
        stopVibration = StartVibration();
        breaknow = false;
        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        if (this.gameObject.transform.childCount > 2)
            Spikes = this.gameObject.transform.GetChild(1).gameObject;
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
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            LetsBounce();
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
