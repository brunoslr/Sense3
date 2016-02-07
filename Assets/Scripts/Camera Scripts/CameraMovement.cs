using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the camera movement, such that the camera is always behind the player.
/// </summary>
public class CameraMovement : MonoBehaviour
{
    public GameObject player;

    //How far the camera is supposed to be behind the player.
    public Vector3 initialOffset;

    void Start()
    {
        transform.position = player.transform.position + initialOffset;
    }

    void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z + initialOffset.z);
    }
}
