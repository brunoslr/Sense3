using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class BackGroundScript : MonoBehaviour {

    public float AngleScale = 1.0f;
    public float RadiusScale = 1.0f;

	// Use this for initialization
	void Start () {
	
	}

    public void setTwirlAngle(float angle)
	{
        this.gameObject.GetComponent<Vortex>().angle = AngleScale * angle;
    }

    public void setTwirlRadius(float radius)
    {
        this.gameObject.GetComponent<Vortex>().radius = new Vector2(RadiusScale * radius,RadiusScale * radius);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
