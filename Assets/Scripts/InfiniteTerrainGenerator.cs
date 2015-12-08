using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InfiniteTerrainGenerator : MonoBehaviour
{
    public GameObject player;
    public GameObject[] obstaclePrefabs;
    public float yPos;

    private Terrain[,] _terrainGrid = new Terrain[3, 3];
    private Terrain linkedTerrain;
    private Vector3 playerPosition;
    private Terrain currentTerrain;
    private int xOffset;
    private int zOffset;
    private List<GameObject> obstacles;

    // Use this for initialization
    void Start()
    {
        obstacles = new List<GameObject>();

        linkedTerrain = gameObject.GetComponent<Terrain>();

        _terrainGrid[0, 0] = Terrain.CreateTerrainGameObject(linkedTerrain.terrainData).GetComponent<Terrain>();
        _terrainGrid[0, 1] = Terrain.CreateTerrainGameObject(linkedTerrain.terrainData).GetComponent<Terrain>();
        _terrainGrid[0, 2] = Terrain.CreateTerrainGameObject(linkedTerrain.terrainData).GetComponent<Terrain>();
        _terrainGrid[1, 0] = Terrain.CreateTerrainGameObject(linkedTerrain.terrainData).GetComponent<Terrain>();
        _terrainGrid[1, 1] = Terrain.CreateTerrainGameObject(linkedTerrain.terrainData).GetComponent<Terrain>();
        _terrainGrid[1, 2] = Terrain.CreateTerrainGameObject(linkedTerrain.terrainData).GetComponent<Terrain>();
        _terrainGrid[2, 0] = Terrain.CreateTerrainGameObject(linkedTerrain.terrainData).GetComponent<Terrain>();
        _terrainGrid[2, 1] = linkedTerrain;
        _terrainGrid[2, 2] = Terrain.CreateTerrainGameObject(linkedTerrain.terrainData).GetComponent<Terrain>();

        UpdateTerrainPositionsAndNeighbors();
        GenerateObstacles();
    }

    private void UpdateTerrainPositionsAndNeighbors()
    {
        _terrainGrid[0, 0].transform.position = new Vector3(
           _terrainGrid[2, 1].transform.position.x - _terrainGrid[2, 1].terrainData.size.x,
           _terrainGrid[2, 1].transform.position.y,
           _terrainGrid[2, 1].transform.position.z + 2 * _terrainGrid[2, 1].terrainData.size.z);
        _terrainGrid[0, 1].transform.position = new Vector3(
           _terrainGrid[2, 1].transform.position.x,
           _terrainGrid[2, 1].transform.position.y,
           _terrainGrid[2, 1].transform.position.z + 2 * _terrainGrid[2, 1].terrainData.size.z);
        _terrainGrid[0, 2].transform.position = new Vector3(
           _terrainGrid[2, 1].transform.position.x + _terrainGrid[2, 1].terrainData.size.x,
           _terrainGrid[2, 1].transform.position.y,
           _terrainGrid[2, 1].transform.position.z + 2 * _terrainGrid[2, 1].terrainData.size.z);

        _terrainGrid[1, 0].transform.position = new Vector3(
           _terrainGrid[2, 1].transform.position.x - _terrainGrid[2, 1].terrainData.size.x,
           _terrainGrid[2, 1].transform.position.y,
           _terrainGrid[2, 1].transform.position.z + _terrainGrid[2, 1].terrainData.size.z);
        _terrainGrid[1, 1].transform.position = new Vector3(
           _terrainGrid[2, 1].transform.position.x,
           _terrainGrid[2, 1].transform.position.y,
           _terrainGrid[2, 1].transform.position.z + _terrainGrid[2, 1].terrainData.size.z);
        _terrainGrid[1, 2].transform.position = new Vector3(
           _terrainGrid[2, 1].transform.position.x + _terrainGrid[2, 1].terrainData.size.x,
           _terrainGrid[2, 1].transform.position.y,
           _terrainGrid[2, 1].transform.position.z + _terrainGrid[2, 1].terrainData.size.z);

        _terrainGrid[2, 0].transform.position = new Vector3(
           _terrainGrid[2, 1].transform.position.x - _terrainGrid[2, 1].terrainData.size.x,
           _terrainGrid[2, 1].transform.position.y,
           _terrainGrid[2, 1].transform.position.z);
        _terrainGrid[2, 2].transform.position = new Vector3(
           _terrainGrid[2, 1].transform.position.x + _terrainGrid[2, 1].terrainData.size.x,
           _terrainGrid[2, 1].transform.position.y,
           _terrainGrid[2, 1].transform.position.z);

        _terrainGrid[0, 0].SetNeighbors(null, null, _terrainGrid[0, 1], _terrainGrid[1, 0]);
        _terrainGrid[0, 1].SetNeighbors(_terrainGrid[0, 0], null, _terrainGrid[0, 2], _terrainGrid[1, 1]);
        _terrainGrid[0, 2].SetNeighbors(_terrainGrid[0, 1], null, null, _terrainGrid[1, 2]);

        _terrainGrid[1, 0].SetNeighbors(null, _terrainGrid[0, 0], _terrainGrid[1, 1], _terrainGrid[2, 0]);
        _terrainGrid[1, 1].SetNeighbors(_terrainGrid[1, 0], _terrainGrid[0, 1], _terrainGrid[1, 2], _terrainGrid[2, 1]);
        _terrainGrid[1, 2].SetNeighbors(_terrainGrid[1, 1], _terrainGrid[0, 2], null, _terrainGrid[2, 2]);

        _terrainGrid[2, 0].SetNeighbors(null, _terrainGrid[1, 0], _terrainGrid[2, 1], null);
        _terrainGrid[2, 1].SetNeighbors(_terrainGrid[2, 0], _terrainGrid[1, 1], _terrainGrid[2, 2], null);
        _terrainGrid[2, 2].SetNeighbors(_terrainGrid[2, 1], _terrainGrid[1, 2], null, null);
    }

    void GenerateObstacles()
    {
        for (int i = 0; i < 5; i++)
        {
            int pick = Random.Range(0, obstaclePrefabs.Length);
            GameObject currentObstacle = Instantiate(obstaclePrefabs[pick]);
            obstacles.Add(currentObstacle);

            float xPos = Random.Range(_terrainGrid[0, 0].transform.position.x, _terrainGrid[0, 2].transform.position.x + _terrainGrid[0, 2].terrainData.size.x);

            float zPos = Random.Range(player.transform.position.z + 30.0f, _terrainGrid[0, 0].transform.position.z + _terrainGrid[0, 2].terrainData.size.z);

            currentObstacle.transform.position = new Vector3(xPos, yPos, zPos);
        }
    }

    void DeleteObstacles()
    {
        Transform obstacle;
        for (int i = 0; i < obstacles.Count; i++)
        {
            obstacle = obstacles[i].transform;
            if (obstacle.position.z < player.transform.position.z)
            {
                obstacles.Remove(obstacle.gameObject);
                Destroy(obstacle.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = player.transform.position;
        currentTerrain = null;

        for (int z = 0; z < 3; z++)
        {
            for (int x = 0; x < 3; x++)
            {
                if ((playerPosition.x >= _terrainGrid[z, x].transform.position.x) &&
                    (playerPosition.x <= _terrainGrid[z, x].transform.position.x + _terrainGrid[z, x].terrainData.size.x) &&
                    (playerPosition.z >= _terrainGrid[z, x].transform.position.z) &&
                    (playerPosition.z <= _terrainGrid[z, x].transform.position.z + _terrainGrid[z, x].terrainData.size.z))
                {
                    xOffset = 1 - x;
                    zOffset = 2 - z;
                    currentTerrain = _terrainGrid[z, x];
                    break;
                }
            }

            if (currentTerrain != null) { break; }
        }

        if (currentTerrain != _terrainGrid[2, 1])
        {
            Terrain[,] newTerrainGrid = new Terrain[3, 3];
            for (int z = 0; z < 3; z++)
                for (int x = 0; x < 3; x++)
                {
                    int newX = x + xOffset;
                    if (newX < 0)
                        newX = 2;
                    else if (newX > 2)
                        newX = 0;
                    int newZ = z + zOffset;
                    if (newZ < 0)
                        newZ = 2;
                    else if (newZ > 2)
                        newZ = 0;
                    newTerrainGrid[newZ, newX] = _terrainGrid[z, x];
                }
            _terrainGrid = newTerrainGrid;
            UpdateTerrainPositionsAndNeighbors();
            DeleteObstacles();
            GenerateObstacles();
        }
    }
}