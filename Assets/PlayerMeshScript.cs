using UnityEngine;
using System.Collections;

public class PlayerMeshScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //updates blend shape related to the player
    public void updatePlayerShape()
    {
        this.GetComponentInChildren<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 50);
    }


}
