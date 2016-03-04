using UnityEngine;
using System.Collections;

public class PlanetCreator : MonoBehaviour {

    public GameObject[] planetPrefabs;
    public float YDisp = 1000;
    public float minDisp = 10000;

    private GameObject[] planets;
    private GameObject Player;
    private Vector3 previousPos;
    private int closestPrefabIndex;
    private int totalPrefabs;

	// Use this for initialization
	void Start () {
        totalPrefabs = planetPrefabs.Length;
        Player = GameObject.FindGameObjectWithTag("Player");
        planets = new GameObject[totalPrefabs];
        initPlanets();
        previousPos = Player.transform.position + new Vector3(minDisp, 0, 0);
        LateUpdate();
	}

    /// <summary>
    /// Create instances of the given Prefabs
    /// </summary>
    void initPlanets()
    {
        Debug.Log(totalPrefabs);
        for(int i=0;i < totalPrefabs; i++)
        {
            planets[i] = (GameObject)Instantiate(planetPrefabs[i], new Vector3(0, 0, - 1000000), new Quaternion(0,0,0,1));
        }
        //planets[10].transform.position = Player.transform.position + new Vector3(Random.Range(1.0f, 1.0f) * 1000, YDisp * (Random.Range(-1.0f, 1.0f)), minDisp);
        closestPrefabIndex = 99;
        //planets[1].transform.position = /*Player.transform.position + */new Vector3(0, 0, 0);
        //planets[2].transform.position = /*Player.transform.position + */new Vector3(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    void LateUpdate()
    {
        if(Vector3.Distance(Player.transform.position, previousPos) > minDisp)
        {
            previousPos = Player.transform.position;
            int index;
            do
            {
                index = Random.Range(0, totalPrefabs);
            } while (index == closestPrefabIndex);
            closestPrefabIndex = index;
            planets[index].transform.position = Player.transform.position + new Vector3(Random.Range(-1.0f,1.0f) * 5000,  YDisp * (Random.Range(-1.0f,1.0f)), minDisp);
            planets[index].transform.rotation = new Quaternion(Random.Range(-45, 45), Random.Range(-45, 45), Random.Range(-45, 45), 1.0f);
        }
    }
}
