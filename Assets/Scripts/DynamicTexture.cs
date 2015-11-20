using UnityEngine;
using System.Collections;

public class DynamicTexture : MonoBehaviour 
{
    public ComputeShader computeShader;

    private RenderTexture tex;
    private int kernelHandle;
    public float[] FFT = new float[4];

	// Use this for initialization
	void Start () 
    {
        kernelHandle = computeShader.FindKernel("CSMain");

        tex = new RenderTexture(1920, 1080, 24);
        tex.enableRandomWrite = true;
        tex.Create();

        this.GetComponent<MeshRenderer>().material.mainTexture = tex;

   	}

	void Update () 
    {
        computeShader.SetFloat("t", Time.time);
        computeShader.SetFloats("fft", FFT);
        computeShader.SetTexture(kernelHandle, "Result", tex);
        computeShader.Dispatch(kernelHandle, 1920 / 32, 1080 / 32, 1);
	}
}
