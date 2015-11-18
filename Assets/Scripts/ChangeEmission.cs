using UnityEngine;
using System.Collections;

public class ChangeEmission : MonoBehaviour {
    private Material material;
    private float emission;
    // Use this for initialization
    void Start () {
        material = this.GetComponent<MeshRenderer>().material;
        material.EnableKeyword("_EMISSION");
       
    }
	
	// Update is called once per frame
	void Update () {
        emission = Mathf.Sin(Time.time);
        material.SetColor("_EmissionColor", (1 - emission) * material.GetColor("_Color"));
        material.SetColor("_Metallic", emission * material.GetColor("_Color"));
        material.SetColor("_Glossiness", emission * material.GetColor("_Color"));
    }
}
