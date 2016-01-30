using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioToolApp : MonoBehaviour {

    private GameObject playerAudio;
    private InputField filePathField;
    private InputField fileNameField;
    private InputField totalTracksField;
    private string filePath; 

	// Use this for initialization
	void Start () {
        playerAudio = GameObject.FindGameObjectWithTag("Player");
        playerAudio = playerAudio.transform.FindChild("PlayerAudio").gameObject;
        filePathField = GameObject.Find("FilePath").GetComponent<InputField>();
        fileNameField = GameObject.Find("FileName").GetComponent<InputField>();
        totalTracksField = GameObject.Find("TotalTracks").GetComponent<InputField>();
    }
	
    public void AddLayer()
    {
        string fileName = fileNameField.text;
        int totalTracks = int.Parse(totalTracksField.text);
        filePath = filePathField.text;
        playerAudio.AddComponent<Layer>();
        for(int i=1;i<=totalTracks; i++)
            Debug.Log(filePath + fileName + i.ToString() + ".mp3");
    }

    public void RefreshController()
    {
        playerAudio.GetComponent<AudioControllerV2>().resetController();
    }
    public void onClick()
    {
        Debug.Log("works");
    }

}
