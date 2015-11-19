using UnityEngine;
using System.Collections;

public class PickUpScript : MonoBehaviour {

    public bool pickedUp;
	public GameObject player;

	private bool stopCheck = false;

    private AudioVisualizer audioVisualizer;

	// Use this for initialization
    void Start()
    {
        pickedUp = false;
    }

    void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.name == "Player")
		{
            pickedUp = true;
            other.gameObject.GetComponent<LaneMovement>().pickUp();
		}		
	}

	void Update()
	{
		if (!stopCheck)
			passedPickUp();
	}

	void passedPickUp()
	{
		if((player.transform.position.z - this.transform.position.z) > 0 && !pickedUp)
		{
			missedAnimation();
			stopCheck = true;
		}
	}

	void missedAnimation()
	{
        Debug.Log("Missed animation");
        audioVisualizer = this.transform.parent.GetComponent<AudioVisualizer>();
        audioVisualizer.PlayerVisualizer(10.0f);
	}
}
