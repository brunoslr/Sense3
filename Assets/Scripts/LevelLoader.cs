using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {

    public Button startButton;
    public Button quitButton;
	// Use this for initialization
	void Start () {
        startButton.onClick.AddListener(() => { LoadGame(); });
        quitButton.onClick.AddListener(() => { QuitGame(); });

	}

    public void LoadGame()
    {
        Application.LoadLevel("test2");
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
