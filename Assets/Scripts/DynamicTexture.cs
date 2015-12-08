using UnityEngine;
using System.Collections;

public class DynamicTexture : MonoBehaviour 
{
    public ComputeShader computeShader;

    private RenderTexture tex;
    private int kernelHandle;
    public float[] FFT = new float[4];
    private Vector2[] c = new Vector2[2];
	// Use this for initialization
	void Start () 
    {
        kernelHandle = computeShader.FindKernel("CSMain");

        tex = new RenderTexture(1920, 1080, 24);
        tex.enableRandomWrite = true;
        tex.Create();

        this.GetComponent<MeshRenderer>().material.mainTexture = tex;
        c[0] = new Vector2(0.0f, 0.0f);
        c[1] = new Vector2(0.0f, 0.0f);
   	}

	void Update () 
    {
        computeShader.SetFloat("t", Time.time);
        computeShader.SetFloats("fft", FFT);
        if (FFT[0] >= 0.000001f)
        {
            c[0].x = -0.18453326967811071f;
            c[0].y = Mathf.Sin(Time.time / 10.0f);
        }
        computeShader.SetVector("c1", c[0]);
        computeShader.SetVector("c2", c[1]);
        computeShader.SetTexture(kernelHandle, "Result", tex);
        computeShader.Dispatch(kernelHandle, 1920 / 32, 1080 / 32, 1);
	}
}
