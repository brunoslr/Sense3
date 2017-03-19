using UnityEngine;
using System.Collections;

public class DynamicTexture : MonoBehaviour 
{
    public ComputeShader computeShader;
    //public Material skybox;
    public Texture2D palette;

    private RenderTexture tex;
    private int kernelHandle;

	// Use this for initialization
	void Start () 
    {
        kernelHandle = computeShader.FindKernel("CSMain");

        tex = new RenderTexture(512, 512, 24);
        tex.enableRandomWrite = true;
        tex.Create();
        //RenderSettings.skybox = skybox;
        //skybox.SetTexture("_FrontTex", tex);
        //skybox.SetTexture("_BackTex", tex);
        //skybox.SetTexture("_TopTex", tex);
        //skybox.SetTexture("_BottomTex", tex);
        //skybox.SetTexture("_LeftTex", tex);
        //skybox.SetTexture("_RightTex", tex);
        this.GetComponent<MeshRenderer>().material.mainTexture = tex;

        computeShader.SetTexture(kernelHandle, "palette", palette);
   
    }

	void Update () 
    {
        computeShader.SetFloat("t", Time.time);
        computeShader.SetTexture(kernelHandle, "Result", tex);
        computeShader.Dispatch(kernelHandle, 512 / 32, 512 / 32, 1);
	}
}
