using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

    public Vector3 finalPosistion;
    public Vector3 startPosition;
    public Vector3 direction;
    public float _time;
    public float speed;
    public Vector3 vel;

  public enum MeteorStyle { RANDOM, ORGANIZED };
  public MeteorStyle _meteorStyle;
  public enum Direction { LEFT, RIGHT, FORWARD, BACKWARD};
  public Direction _direction;
	// Use this for initialization
	void Start () {

        var playrPosition = GameObject.Find("Player").transform.position;
        if (_meteorStyle == MeteorStyle.RANDOM)
        {
        
            startPosition = playrPosition + Vector3.forward * 1000 + (Vector3)(Random.insideUnitCircle * 300);
            finalPosistion = playrPosition- startPosition; // direction towards the player
            Vector3 temp = (Vector3)(Random.insideUnitCircle * 100);
            finalPosistion += temp;
            this.transform.position = startPosition;
            finalPosistion.Normalize();
            this.gameObject.GetComponent<Rigidbody>().AddForce(finalPosistion * 500f, ForceMode.VelocityChange);
        }
        if (_meteorStyle == MeteorStyle.ORGANIZED)
        {
            Vector3 temp = (Vector3)(Random.insideUnitCircle * 500);
            if (_direction == Direction.LEFT || _direction == Direction.RIGHT)
            {
                if (_direction == Direction.LEFT)
                    this.transform.parent.transform.rotation = Quaternion.Euler(0, -90, 0);
                else
                    this.transform.parent.transform.rotation = Quaternion.Euler(0, 90, 0);
          
                startPosition = this.transform.position + new Vector3(0, temp.x, temp.y);
            }

            if (_direction == Direction.FORWARD || _direction == Direction.BACKWARD)
            {
             
                if (_direction == Direction.BACKWARD)
                    this.transform.parent.transform.rotation = Quaternion.Euler(0, -180, 0);
                startPosition = this.transform.position + new Vector3(temp.x, temp.y, 0);
            }

            finalPosistion = this.transform.parent.forward + (Vector3)(Random.insideUnitCircle * 100);
            this.transform.position = startPosition;
            this.gameObject.GetComponent<Rigidbody>().AddForce(transform.parent.forward * 500f, ForceMode.VelocityChange);
        }
      
        
    }


   
}
