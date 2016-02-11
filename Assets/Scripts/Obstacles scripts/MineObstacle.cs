using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class MineObstacle : MonoBehaviour
{
    private float playerDist;
    private float spikePos;
    private float totalDist;
    private float ratio;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            totalDist = Mathf.Abs(spikePos - other.gameObject.transform.position.z);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerDist = Mathf.Abs(spikePos - other.gameObject.transform.position.z);

            ratio = playerDist / totalDist;

            if(ratio > 0.5f)
            {
                StartCoroutine(StartVibration(5.0f));
            }

            else if(ratio < 0.5f && ratio > 0.3f)
            {
                StartCoroutine(StartVibration(0.5f));
            }

            else
            {
                StartCoroutine(StartVibration(0.0f));
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            StopVibration();
    }

    IEnumerator StartVibration(float waitTime)
    {
        GamePad.SetVibration(0, 1.0f, 1.0f);
        yield return new WaitForSeconds(waitTime);
    }

    void StopVibration()
    {
        GamePad.SetVibration(0, 0.0f, 0.0f);
    }

    void OnDestroy()
    {
        StopVibration();
    }
}
