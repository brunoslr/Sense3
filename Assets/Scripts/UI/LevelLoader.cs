using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    private string startScene = "test2";

    public Button startButton;
    public Button quitButton;
     
	// Use this for initialization
	void Start () {
        startButton.onClick.AddListener(() => { LoadScene(startScene); });
        quitButton.onClick.AddListener(() => { QuitGame(); });

	}

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
    }
}
