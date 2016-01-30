using UnityEngine;
using System.Collections;
using XInputDotNetPure;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]

public class MineObstacle : MonoBehaviour
{
    public GameObject Spikes;

    public float triggerSpikes;

    private float playerDist;
    private float spikePos;
    private float totalDist;
    private float ratio;

	// Use this for initialization
	void Start () {
        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        if (this.gameObject.transform.childCount > 2)
            Spikes = this.gameObject.transform.GetChild(1).gameObject;

        spikePos = this.gameObject.transform.GetChild(0).position.z;
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

            if (playerDist < triggerSpikes)
            {
                Spikes.SetActive(true);
                SpikeAnimation();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            StopVibration();
    }

    /// <summary>
    /// Spike animation definition.
    /// To Do: The spikes are going up on to a currently randomly chosen height.
    /// Change this later to respond to the Audio output.
    /// To Do: Make the duration of animation a variable set by the designer.
    /// </summary>
    void SpikeAnimation()
    {
        for (int i = 0; i < 15; i++)
        {
            Vector3 prevScale = Spikes.transform.GetChild(i).localScale;
            prevScale.y = Mathf.Lerp(prevScale.y, Random.Range(0, 10) , Time.deltaTime * 10);
            Spikes.transform.GetChild(i).localScale = prevScale;
            Spikes.transform.position = new Vector3(Spikes.transform.position.x, prevScale.y / 2 + 1, Spikes.transform.position.z);
        }
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
}
