using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class FadeScript : MonoBehaviour {
    public Image image;
    public float smooth;
    public float alphaTarget;
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
       // StartFade();
    }

    void StartFade()
    {
        float semi = 0.5f;
        float full = 1f;
        if (Vector3.Distance(this.transform.position, Vector3.zero) < 500)
        {
            alphaTarget = full;
        }
        else
            alphaTarget = semi;

        image.color = new Color(image.color.r,image.color.g,image.color.b, Mathf.Lerp(image.color.a, alphaTarget, Time.deltaTime * smooth));
    }
}
