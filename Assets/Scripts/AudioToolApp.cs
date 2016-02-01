using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioToolApp : MonoBehaviour {

    private GameObject playerAudio;
    private InputField filePathField;
    private InputField fileNameField;
    private InputField totalTracksField;
    private string filePath;
    private int LayerNumber;
    private bool refreshed;
    private Layer lastLayer;
    private Text outputText;
    private Text LayerListText;

    public GameObject removeButton;

    // Use this for initialization
    void Start () {
        lastLayer = null;
        LayerNumber = 0;
        refreshed = false;
        removeButton.SetActive(false);
        outputText = GameObject.Find("OutputText").GetComponent<Text>();
        LayerListText = GameObject.Find("LayerList").GetComponent<Text>();
        playerAudio = GameObject.FindGameObjectWithTag("Player");
        playerAudio = playerAudio.transform.FindChild("PlayerAudio").gameObject;
        filePathField = GameObject.Find("FilePath").GetComponent<InputField>();
        fileNameField = GameObject.Find("FileName").GetComponent<InputField>();
        totalTracksField = GameObject.Find("TotalTracks").GetComponent<InputField>();
    }
	
    public void AddLayer()
    {
        //Read the inpute from the available fields
        string fileName = fileNameField.text;
        int totalTracks = int.Parse(totalTracksField.text);
        filePath = filePathField.text;

        outputText.text = "Tracks Added : \n";
        LayerListText.text += "\n\t" + (++LayerNumber)+ " " + fileName + " (" + totalTracks + " tracks)";

        //Create a new Layer in the player
        lastLayer = playerAudio.AddComponent<Layer>();
        Layer newLayer = lastLayer;
        newLayer.layername = fileName;
        newLayer.audioTracks = new AudioClip[totalTracks];

        //This is required to load audio
        WWW wtf;


        for (int i = 1; i <= totalTracks; i++)
        {
            string index = "00" + i;
            index = index.Substring(index.Length - 2);

            outputText.text += fileName + index + ".wav" +"\n";

            //temporary handle
            AudioClip clip;
            
            //Load the Audio
            wtf = new WWW("file:///"+filePath + fileName + index + ".wav");
            while (!wtf.isDone)
            {
                //Wait till the audio is completly loaded
                Debug.Log("Loading");
            }

            clip = wtf.GetAudioClip(false);
            clip.name = fileName + i.ToString();
            newLayer.audioTracks[i - 1] = clip;
        }

        refreshed = false;
        if (lastLayer != null)
            removeButton.SetActive(true);
        //Reset the values in the layer
        newLayer.restartLayer();
    }

    public void RemoveLayer()
    {
        if(lastLayer == null)
        {
            outputText.text = "No Layer attached to Delete";
            return;
        }
        outputText.text = " Last Layer Removed";
        LayerListText.text += " <REMOVED>";
        LayerNumber--;
        lastLayer.destroySelf();
        lastLayer = null;
        removeButton.SetActive(false);
        refreshed = false;
    }

    public void Reset()
    {
        playerAudio.GetComponent<AudioControllerV2>().playNewTrack(0, 1);
        AudioSource[] sources = playerAudio.GetComponents<AudioSource>();
        Layer[] layers = playerAudio.GetComponents<Layer>();

        for (int i = sources.Length - 1; i >= 0; i--)
        {
            Destroy(layers[i]);
            Destroy(sources[i]);
        }

        LayerListText.text = "Layer List:";
        outputText.text = "All Layers have been removed";

        refreshed = false;
    }
    public void PlayNewTrack()
    {
        if(refreshed == false)
        {
            playerAudio.GetComponent<AudioControllerV2>().resetController();
            refreshed = true;
        }

        playerAudio.GetComponent<AudioControllerV2>().playNewTrack(0,1);

        AudioSource[] sources = playerAudio.GetComponents<AudioSource>();

        outputText.text = "Currently Playing Tracks: ";
        for (int i = 0; i < sources.Length; i++)
        {
            if(sources[i].isPlaying)
                outputText.text += "\n" + sources[i].clip.name;
        }
    }

    public void StopAllTracks()
    {
        refreshed = false;
        outputText.text = "All tracks Stopped";
        playerAudio.GetComponent<AudioControllerV2>().stopAllTracks();
    }
}
