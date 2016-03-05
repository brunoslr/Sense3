using UnityEngine;
using System.Collections.Generic;

public class PlanetCreator : MonoBehaviour {

    public GameObject[] planetPrefabs;
    public float YDisp = 1000;
    public float minDisp = 10000;
    public float maxView = 25000;

    private List<GameObject> planets;
    private GameObject Player;
    private Vector3 previousPos;
    private int totalPrefabs;
    private List<GameObject> objects_in_front;
    private int visibility;

	// Use this for initialization
	void Start () {
        totalPrefabs = planetPrefabs.Length;
        Player = GameObject.FindGameObjectWithTag("Player");
        previousPos = Player.transform.position;
        planets = new List<GameObject>();
        objects_in_front = new List<GameObject>();
        visibility = (int)maxView / (int)minDisp;
        initPlanets();
	}

    /// <summary>
    /// Create instances of the given Prefabs
    /// </summary>
    void initPlanets()
    {
       for (int i = 0; i < totalPrefabs; i++)
       {
           planets.Add((GameObject)Instantiate(planetPrefabs[i], new Vector3(0, 0, -1000000), new Quaternion(0, 0, 0, 1)));
       }
    }
	
	// Update is called once per frame
	void Update () {
	    if(objects_in_front.Count > 0)
        {
            Vector3 playerToPlanet = objects_in_front[0].transform.position - Player.transform.position;
            float dot = Vector3.Dot(playerToPlanet, Player.transform.position);
            if(dot < 0)
            {
                planets.Add(objects_in_front[0]);
                objects_in_front.RemoveAt(0);
            }
        }
	}

    void LateUpdate()
    {
        while(objects_in_front.Count < visibility && planets.Count > 0)
        {
            int index;
            index = Random.Range(0, planets.Count - 1);
            objects_in_front.Add(planets[index]);
            previousPos = previousPos + new Vector3(Random.Range(-1.0f, 1.0f) * 5000, YDisp * (Random.Range(-1.0f, 1.0f)), minDisp);
            objects_in_front[objects_in_front.Count - 1].transform.position = previousPos;
            objects_in_front[objects_in_front.Count - 1].transform.rotation = new Quaternion(Random.Range(-45, 45), Random.Range(-45, 45), Random.Range(-45, 45), 1.0f);
        }
    }
}
