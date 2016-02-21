using UnityEngine;

public class PipeWorld : MonoBehaviour {
    private PipeInstance[] pipes;

    public PipeInstance pipePrefab;
    public int pipeCount;

    private void Awake()
    {
        pipes = new PipeInstance[pipeCount];
        for (int i = 0; i < pipes.Length; i++)
        {
            PipeInstance pipeInstance = pipes[i] = Instantiate<PipeInstance>(pipePrefab);
            pipeInstance.transform.SetParent(transform, false);
            pipeInstance.Generate();

            if (i > 0)
            {
                pipeInstance.AlignWidth(pipes[i - 1]);
            }
        }
        AlignNextPipeWithOrigin();
    }

    public PipeInstance SetupFirstPipe()
    {
        transform.localPosition = new Vector3(0f, -pipes[1].CurveRadius, 0f);
        return pipes[1];
    }

    public PipeInstance SetupNextPipe()
    {
        ShiftPipes();
        AlignNextPipeWithOrigin();
        pipes[pipes.Length - 1].Generate();
        pipes[pipes.Length - 1].AlignWidth(pipes[pipes.Length - 2]);
        transform.localPosition = new Vector3(0f, -pipes[1].CurveRadius, 0f);
        return pipes[1];
    }

    private void ShiftPipes()
    {
        PipeInstance temp = pipes[0];
        for (int i = 1; i < pipes.Length; i++)
        {
            pipes[i - 1] = pipes[i];
        }
        pipes[pipes.Length - 1] = temp;
    }

    private void AlignNextPipeWithOrigin()
    {
        Transform transformToAlign = pipes[1].transform;

        for (int i = 0; i < pipes.Length; i++)
        {
            if (i != 1)
            {
                pipes[i].transform.SetParent(transformToAlign);
            }
        }

        transformToAlign.localPosition = Vector3.zero;
        transformToAlign.localRotation = Quaternion.identity;

        for (int i = 0; i < pipes.Length; i++)
        {
            if (i != 1)
            {
                pipes[i].transform.SetParent(transform);
            }
        }
    }
}
