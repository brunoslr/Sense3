using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
          // other.gameObject.GetComponent<CoreSystem>().GameReset();
          SceneManager.LoadScene("test2");
        }
    }
}
