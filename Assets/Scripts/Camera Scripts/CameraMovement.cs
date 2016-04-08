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
    public float rumbleShakeLow;
    public float rumbleShakeHigh;
    [HideInInspector] public float rumbleTilt;
    public float pickupTilt;
    public float moveTilt;
    public float moveFactor;
    public float shakeFactor;
    //private PlayerMovement playerMovement; // Removed - Unused
    private float initialRotationX;

    void Start()
    {
        transform.position = player.transform.position + initialOffset;
        initialRotationX = transform.rotation.eulerAngles.x;
        CoreSystem.onSoundEvent += RumbleCameraPickup;
        CoreSystem.onObstacleEvent += RumbleCameraPickup;
    }

    void OnDestroy()
    {
        CoreSystem.onSoundEvent -= RumbleCameraPickup;
        CoreSystem.onObstacleEvent -= RumbleCameraPickup;
    }

    void Awake()
    {
        GameObject[] camera = GameObject.FindGameObjectsWithTag("MainCamera");
        for (int i = 0; i < camera.Length; i++)
        {
            camera[i].GetComponent<Camera>().farClipPlane *= 5.0f;
        }
    }

    void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z + initialOffset.z);
    }

    public void RotateCamera(float dir)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(initialRotationX, 0.0f, dir * moveTilt), moveFactor);
    }

    public void RumbleCamera()
    {
        transform.rotation = Quaternion.Euler(initialRotationX, 0.0f, Mathf.Sin(Time.time * shakeFactor) * rumbleTilt);
    }

    public void RumbleCameraPickup()
    {
        //float currentTime = Time.time;
        //while(Time.time - currentTime <= 1.0f)
            transform.rotation = Quaternion.Euler(initialRotationX, 0.0f, Mathf.Sin(Time.time * shakeFactor) * pickupTilt);
    }
}
