using UnityEngine;
using System.Collections;

public class DynamicTexture : MonoBehaviour 
{
    public ComputeShader computeShader;
    public Material skybox;

    private RenderTexture tex;
    private int kernelHandle;

    private float[] c1;   

	// Use this for initialization
	void Start () 
    {
        kernelHandle = computeShader.FindKernel("CSMain");

        tex = new RenderTexture(1920, 1080, 24);
        tex.enableRandomWrite = true;
        tex.Create();
        this.GetComponent<MeshRenderer>().material.mainTexture = tex;
        RenderSettings.skybox = skybox;
        skybox.SetTexture("_FrontTex", tex);
        skybox.SetTexture("_BackTex", tex);
        skybox.SetTexture("_TopTex", tex);
        skybox.SetTexture("_BottomTex", tex);
        skybox.SetTexture("_LeftTex", tex);
        skybox.SetTexture("_RightTex", tex);

        c1 = new float[2];
    }

	void Update () 
    {
        //computeShader.SetFloat("t", Time.time);
        c1[0] = -0.11253326967811071f;
        c1[1] = Mathf.Cos(Time.time / 10.0f);
        computeShader.SetFloats("c1", c1);
        computeShader.SetTexture(kernelHandle, "Result", tex);
        computeShader.Dispatch(kernelHandle, 1920 / 32, 1080 / 32, 1);
	}
}
