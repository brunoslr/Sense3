using UnityEngine;
using System.Collections.Generic;

public class PlanetCreator : MonoBehaviour {

    public GameObject[] Prefabs;
    public float YDisp = 10000;
    public int minDisp = 10000;

    private List<GameObject> galaxies;
    private GameObject Player;
    private int Counter;
    private int totalPrefabs;
    private List<GameObject> objects_in_front;
    private int sign;

	// Use this for initialization
	void Start () {
        totalPrefabs = Prefabs.Length;
        Player = GameObject.FindGameObjectWithTag("Player");
        Counter = (int) Player.transform.position.z - minDisp/2;
        galaxies = new List<GameObject>();
        objects_in_front = new List<GameObject>();
        initPlanets();
        sign = 1;
	}

    /// <summary>
    /// Create instances of the given Prefabs
    /// </summary>
    void initPlanets()
    {
        GameObject temp;
       for (int i = 0; i < totalPrefabs; i++)
       {
            temp = (GameObject)Instantiate(Prefabs[i]);
            temp.SetActive(false);
            galaxies.Add(temp);
       }
    }
	
	// Update is called once per frame
	void Update () {

	}

    void LateUpdate()
    {
        if(Counter + (minDisp/2.0f) <= Player.transform.position.z)
        {
            float x, y, z;
            int tempSign = Random.Range(-1, 1);
            tempSign = tempSign / Mathf.Abs(tempSign);

            x = Random.Range(1, 10) * sign;
            y = Random.Range(1, 10) * tempSign;
            z = 1.0f;

            Vector3 dir = new Vector3(x, y, z);
            dir = Vector3.Normalize(dir);
            //dir *= minDisp/2.0f;
            Debug.Log(dir);
            int index = Random.Range(0, galaxies.Count - 1);
            galaxies[index].transform.position = Player.transform.position + dir;
            galaxies[index].transform.forward = Vector3.Normalize(Player.transform.position - galaxies[index].transform.position);
            galaxies[index].SetActive(true);

            Vector3 dir2 = new Vector3(-x, y * tempSign, z);
            dir2 = Vector3.Normalize(dir2);
            Debug.Log(dir2);
            int index2;
            do
            {
                index2 = Random.Range(0, galaxies.Count - 1);
            } while (index2 == index);

            galaxies[index2].transform.position = Player.transform.position + dir2;
            galaxies[index2].transform.forward = Vector3.Normalize(Player.transform.position - galaxies[index2].transform.position);
            galaxies[index2].transform.Rotate(new Vector3(0, 0, 1), 180.0f);
            galaxies[index2].SetActive(true);

            sign *= (-1);
            Counter += minDisp;
        }
    }
}
