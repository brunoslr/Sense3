using UnityEngine;
using System.Collections;

public class DynamicBackground : MonoBehaviour {

    public ComputeShader dynamicBackgroundShader;
    public Camera camera;

    private int kernelHandle;
    private float time;
    private RenderTexture dynamicBackgroundTexture; 
    private MeshRenderer meshRenderer;
    public Material skybox;

	// Use this for initialization
	void Start () 
    {
        meshRenderer = this.GetComponent<MeshRenderer>();
        kernelHandle = dynamicBackgroundShader.FindKernel("CSMain");
        dynamicBackgroundTexture = new RenderTexture(1024, 1024, 24);
        dynamicBackgroundTexture.enableRandomWrite = true;
        dynamicBackgroundTexture.isCubemap = true;
        dynamicBackgroundTexture.isPowerOfTwo = true;
        dynamicBackgroundTexture.Create();
        skybox = RenderSettings.skybox;
        skybox.SetTexture("_Tex", dynamicBackgroundTexture);
        //skybox.SetTexture("_FrontTex", dynamicBackgroundTexture);
        //skybox.SetTexture("_BackTex", dynamicBackgroundTexture);
        //skybox.SetTexture("_LeftTex", dynamicBackgroundTexture);
        //skybox.SetTexture("_RightTex", dynamicBackgroundTexture);
        //skybox.SetTexture("_UpTex", dynamicBackgroundTexture);
        //skybox.SetTexture("_DownTex", dynamicBackgroundTexture);
	}

    void RunShader()
    {
        dynamicBackgroundShader.SetFloat("time", Time.time);
        dynamicBackgroundShader.SetTexture(kernelHandle, "Result", dynamicBackgroundTexture);
        dynamicBackgroundShader.Dispatch(kernelHandle, 4, 4, 1);
    }

	// Update is called once per frame
	void Update () 
    {
        RunShader();
	}
}
