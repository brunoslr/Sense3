using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreditsUI : MonoBehaviour {

    public string[] creditsTitle;
    public string[] creditsStrings;

    public CanvasGroup leftPanel;
    public CanvasGroup rightPanel;

    public GameObject creditsLeft;
    public GameObject creditsRight;
    public GameObject creditsLeftTitle;
    public GameObject creditsRightTitle;

    private float targetAlphaLeft = 0.0f;
    private float targetAlphaRight = 0.0f;

    private SkinnedMeshRenderer shipMesh;
    private float blendShapeIncrementFactor = 11.11f;
    private int counter = 0;

    private LevelLoader levelLoader;
    // Use this for initialization
    void Start () {
        shipMesh = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<SkinnedMeshRenderer>();
        levelLoader = gameObject.AddComponent<LevelLoader>();
        InvokeRepeating("ChangeCredits", 0, 5.0f);
	}

    void Update()
    {
        if (Input.GetButton("Cancel"))
        {
            levelLoader.LoadScene("MainMenu");
        }
        leftPanel.alpha = Mathf.Lerp(leftPanel.alpha, targetAlphaLeft, 0.015f);
        rightPanel.alpha = Mathf.Lerp(rightPanel.alpha, targetAlphaRight, 0.015f);
    }

    private void ChangeCredits()
    {
        if (counter % 2 == 0 && counter < 9)
        {
            creditsLeftTitle.GetComponent<Text>().text = creditsTitle[counter];
            string tempText = creditsStrings[counter];
            tempText = tempText.Replace(",", "," + System.Environment.NewLine);
            creditsLeft.GetComponent<Text>().text = tempText;
            targetAlphaLeft = 1.0f;
            targetAlphaRight = 0.0f;
        }
        else if (counter % 2 == 1 && counter < 9)
        {
            creditsRightTitle.GetComponent<Text>().text = creditsTitle[counter];
            string tempText = creditsStrings[counter];
            tempText = tempText.Replace(",", "," + System.Environment.NewLine);
            creditsRight.GetComponent<Text>().text = tempText;
            targetAlphaLeft = 0.0f;
            targetAlphaRight = 1.0f;
        }
        shipMesh.SetBlendShapeWeight(0, counter * blendShapeIncrementFactor);
        counter++;
    }
}
