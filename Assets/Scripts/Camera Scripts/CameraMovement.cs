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
    //private PlayerMovement playerMovement; // Removed - Unused
    public float initialRotationX;

    void Start()
    {
        transform.position = player.transform.position + initialOffset;
        initialRotationX = transform.rotation.eulerAngles.x;
        // playerMovement = player.GetComponent<PlayerMovement>(); // Removed - Unused
    }

    void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + initialOffset.y, player.transform.position.z + initialOffset.z);
    }

    public void RotateCamera(float dir)
    {
        transform.rotation = Quaternion.Euler(initialRotationX, 0.0f, dir * tilt);
    }

    public void RumbleCamera()
    {
        transform.rotation = Quaternion.Euler(initialRotationX, 0.0f, Mathf.Sin(Time.time) * tilt);
    }
}
