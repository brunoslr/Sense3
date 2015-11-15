using UnityEngine;

public class Player : MonoBehaviour {
    private float distanceTraveled;
    private float deltaToRotation;
    private float systemRotation;
    private float worldRotation;
    private float avatarRotation;
    private PipeInstance currentPipe;
    private Transform world;
    private Transform rotator;

    public float rotationVelocity;
    public float velocity;
    public PipeWorld pipeWorld;

    private void Start()
    {
        world = pipeWorld.transform.parent;
        rotator = transform.GetChild(0);
        currentPipe = pipeWorld.SetupFirstPipe();
        SetupCurrentPipe();
    }

    private void Update()
    {
        float delta = velocity * Time.deltaTime;
        distanceTraveled += delta;
        systemRotation += delta * deltaToRotation;

        if (systemRotation >= currentPipe.CurveAngle)
        {
            delta = (systemRotation - currentPipe.CurveAngle) / deltaToRotation;
            currentPipe = pipeWorld.SetupNextPipe();
            SetupCurrentPipe();
            deltaToRotation = 360f / (2f * Mathf.PI * currentPipe.CurveRadius);
            systemRotation = delta * deltaToRotation;
        }

        pipeWorld.transform.localRotation = Quaternion.Euler(0f, 0f, systemRotation);
        UpdateAvatarRotation();
    }

    private void UpdateAvatarRotation()
    {
        avatarRotation += rotationVelocity * Time.deltaTime * Input.GetAxis("Horizontal");

        if (avatarRotation < 0f)
        {
            avatarRotation += 360f;
        }
        else if (avatarRotation >= 360f)
        {
            avatarRotation -= 360f;
        }
        rotator.localRotation = Quaternion.Euler(avatarRotation, 0f, 0f);
    }

    private void SetupCurrentPipe()
    {
        deltaToRotation = 360f / (2f * Mathf.PI * currentPipe.CurveRadius);
        worldRotation += currentPipe.RelativeRotation;

        if (worldRotation < 0f)
        {
            worldRotation += 360f;
        }
        else if (worldRotation >= 360f)
        {
            worldRotation -= 360f;
        }
        world.localRotation = Quaternion.Euler(worldRotation, 0f, 0f);
    }
}
