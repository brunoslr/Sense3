using UnityEngine;
using System.Collections;

public class CreateSO : MonoBehaviour {

    public Vector3 scaleSO;         // scale of sound obstacle

    private Transform soundPickup; // pickup inside sound obstacle

	// Use this for initialization
	void Start () {
        soundPickup = transform.GetChild(0);
        soundPickup.localScale = new Vector3(transform.localScale.x / 5.0f / 10.0f, transform.localScale.y / 4.0f, transform.localScale.z / transform.localScale.z / 1000.0f);

        soundPickup.localPosition = new Vector3(0.0f, 0.0f, (transform.localScale.z / 2.0f / transform.localScale.z) - (soundPickup.localScale.z * 5.0f / 10.0f));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
