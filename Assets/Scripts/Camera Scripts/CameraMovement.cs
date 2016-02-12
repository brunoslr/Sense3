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
    public float tilt;
    private PlayerMovement playerMovement;

    void Start()
    {
        transform.position = player.transform.position + initialOffset;
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z + initialOffset.z);
    }

    public void RotateCamera(float dir)
    {
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, dir * tilt);
    }
}
