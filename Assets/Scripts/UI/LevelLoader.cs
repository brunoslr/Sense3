using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene("test2");
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
