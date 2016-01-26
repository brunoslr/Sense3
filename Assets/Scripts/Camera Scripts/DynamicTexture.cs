using UnityEngine;
using System.Collections;

public class DynamicTexture : MonoBehaviour 
{
    public ComputeShader computeShader;
    public Material skybox;

    private RenderTexture tex;
    private int kernelHandle;

 
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

     
    }

	void Update () 
    {
        computeShader.SetFloat("t", Time.time);
        computeShader.SetTexture(kernelHandle, "Result", tex);
        computeShader.Dispatch(kernelHandle, 1920 / 32, 1080 / 32, 1);
	}
}
