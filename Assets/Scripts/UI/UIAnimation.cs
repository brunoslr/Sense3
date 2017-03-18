﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIAnimation : MonoBehaviour {
    public GameObject player;
    public float speed;
    public List<GameObject> menuElements;
    public Vector3[] positions;
    public Button startButton;
    public Button quitButton;
    public Button creditsButton;
    public Button tutorialsButton;
    public Button flowButton;
    public GameObject[] buttons;
    public GameObject center;
    public Vector3 temp;
    float time;
    public bool isRunning;
    public float smooth;
    public float radius;
    public GameObject currentItem;


    // Use this for initialization
    void Start()
    {
        
        if (startButton != null)
            startButton.onClick.AddListener(() => { StartGame(); });

        if (quitButton != null)
            quitButton.onClick.AddListener(() => { QuitGame(); });

        if (creditsButton != null)
            creditsButton.onClick.AddListener(() => { LoadScene("Credits");});

        if (tutorialsButton != null)
            tutorialsButton.onClick.AddListener(() => { LoadScene("Tutorial"); });

    }
    // Update is called once per frame
    void Update () {
       
        if (Input.GetButton("Submit"))
        {
            currentItem.GetComponentInChildren<Button>().onClick.Invoke();
        }

        time += Time.deltaTime;
        player.transform.Rotate(Vector3.up * Time.deltaTime * speed, Space.Self);
    }

    public void StartGame()
    {
        StartCoroutine("StartPlayerTakeOff");
      
    }
    public IEnumerator StartPlayerTakeOff()
    {   while (player.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0) < 100)
        {
            player.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, Mathf.Lerp(player.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0), 100, time * Time.deltaTime));
            yield return null;
            if (player.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0) > 98)
                LoadScene("Infinite");

        }
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
