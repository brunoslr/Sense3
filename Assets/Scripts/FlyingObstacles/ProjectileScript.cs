using UnityEngine;
using System.Collections;
using DynamicObstacles;
public class ProjectileScript : MeteorStyle {

    public Vector3 finalPosistion;
    public Vector3 startPosition;
    public Vector3 direction;
    public float _time;
    public float speed;
    public Vector3 vel;

	// Use this for initialization
	void OnEnable () {
        var playerPosition = GameObject.Find("Player").transform.position;

        startPosition = playerPosition + Vector3.forward * 1000 + (Vector3)(Random.insideUnitCircle *300);
        finalPosistion = playerPosition - startPosition; // direction towards the player
        Vector3 temp = (Vector3)(Random.insideUnitCircle * 100);
        finalPosistion += temp;
        this.transform.position = startPosition;
        finalPosistion.Normalize();
        this.gameObject.GetComponent<Rigidbody>().AddForce(finalPosistion * 500f, ForceMode.VelocityChange);
        this.transform.GetChild(0).GetComponent<ParticleSystem>().GetComponent<Renderer>().material = particleMaterial;
    }

    void OnDisable()
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.zero; 
    }
}
