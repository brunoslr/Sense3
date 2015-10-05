using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    public GameObject player;

    private float offset;

    void Start()
    {
        offset = transform.position.z - player.transform.position.z;
    }

    void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z + offset);
    }
}
