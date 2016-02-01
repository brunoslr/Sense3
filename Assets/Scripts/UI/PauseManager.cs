using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class PauseManager : MonoBehaviour {


    private string mainMenuScene = "MainMenu";

    public Button resumeButton;
    public Button restartButton;
    public Button mainMenuButton;
    public Button quitButton;

    public CanvasGroup panel;

    public bool IsPaused { get; private set; }
    float targetAlpha;
    public void Start()
    { 
        resumeButton.onClick.AddListener(() => { OnResumeButtonClick(); });
        restartButton.onClick.AddListener(() => { OnRestartButtonClick(); });
        quitButton.onClick.AddListener(() => { OnQuitButtonClick(); });
        mainMenuButton.onClick.AddListener(() => { OnMainMenuButtonClick(); });
    }

    private void OnQuitButtonClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnMainMenuButtonClick()
    {
        Pause();
        SceneManager.LoadScene(mainMenuScene);
    }

    private void OnRestartButtonClick()
    {
        Pause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnResumeButtonClick()
    {
        Pause();
    }

    public void Pause()
    {
        IsPaused = !IsPaused;
        panel.interactable = IsPaused;
        Time.timeScale = IsPaused ? 0 : 1;
        targetAlpha = IsPaused ? 1 : 0;
    }

    private void Update()
    {
        panel.alpha = Mathf.Lerp(panel.alpha, targetAlpha, 0.25f);
        if (Input.GetKeyUp(KeyCode.Escape)) 
        {
            Pause();
        }
    }
	
}
