using UnityEngine;
using System.Collections;


/// <summary>
/// This class acts the central hub of the state machine implemented in the game architecture.
/// It recieves data and calls corresponding functions in the respective components.
/// 
/// This Script is applied directly on the player(the parent) object.
/// </summary>
public class CoreSystem : MonoBehaviour {

    // If you feel a particular call is going to be repeated lots of times,
    // create a reference to it here. Querying again and again would be expensive.
    // Naming convention : object_the_script_si acctached_to Initials of the script
    private LaneMovement    playerLM;
    private Effects         cameraEf;

	// Use this for initialization
	void Start () {
        playerLM = this.gameObject.GetComponent<LaneMovement>();
        cameraEf = Camera.main.GetComponent<Effects>();
	}
    
    public void executeOnPickUp()
    {
        playerLM.increasePlayerSpeed();
        cameraEf.startTrail();
        cameraEf.startFishEye(3);
    }

    public void executeOnHit()
    {
        playerLM.reducePlayerSpeed();
    }
    	
	// Update is called once per frame
	void Update () {
	
	}
}
