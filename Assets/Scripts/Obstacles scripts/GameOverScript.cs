using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<CoreSystem>().GameReset();
        }
    }
}
