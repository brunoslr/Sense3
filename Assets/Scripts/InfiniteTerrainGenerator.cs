using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InfiniteTerrainGenerator : MonoBehaviour
{
    public GameObject player;

    //Visual obstacles placement requirements
    public GameObject[] visualObstaclePrefabs;
    public float xOffsetVisual;
    public float yOffsetVisual;
    public float zOffsetVisual;

    //Sound obstacle placement requirements
    public GameObject soundObstaclePrefab;
    public float xOffsetSound;
    public float yOffsetSound;
    public float zOffsetFromPlayer;

    public int xClamp;

    private Vector3 playerPosition;

    private Terrain[,] _terrainGrid = new Terrain[3, 3];
    private Terrain linkedTerrain;
    private Terrain currentTerrain;
    private int xOffset;
    private int zOffset;

    private List<GameObject> visualObstacles;

    private List<GameObject> loadedObstacles;
    private GameObject currentObstacle;
    private GameObject soundObstacle;

    private GameObject cubeTest;

    public float zPosFront;
    public float zPosSide;
    private float xPos;
    private int lowerClamp;
    private int upperClamp;

    private float[] zPosition = new float [3];
    private int counter = 0;
    private int mainCounter = 1;
    private float soundPickupZValue = 200f;

    // Use this for initialization
    void Start()
    {
        lowerClamp = (int) player.transform.position.x - xClamp;
        upperClamp = (int) player.transform.position.x + xClamp;

        InitializeTerrain();

        UpdateTerrainPositionsAndNeighbors();

        visualObstacles = new List<GameObject>();
        loadedObstacles = new List<GameObject>();

        GameObject temp;
        for (int i = 0; i < visualObstaclePrefabs.Length; i++)
        {
            temp = Instantiate(visualObstaclePrefabs[i]);
            temp.SetActive(false);
            loadedObstacles.Add(temp);
        }

        zPosFront = zOffsetVisual;

        soundObstacle = Instantiate(soundObstaclePrefab);

        InitializeVisualObstacles();
        GenerateSoundObstacles(soundPickupZValue);
    }

    private void InitializeTerrain()
    {
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

    private GameObject GetNextObstacle(int pick)
    {
        return loadedObstacles[pick];
    }

    private void InitializeVisualObstacles()
    {
        int pick;
        float xPos = _terrainGrid[1,1].transform.position.x + _terrainGrid[1,1].terrainData.size.x / 2;
        for (int i = 0; i < 3; i++)
        {
            pick = Random.Range(0, loadedObstacles.Count);
            currentObstacle = GetNextObstacle(pick);
            currentObstacle.SetActive(true);
            loadedObstacles.RemoveAt(pick);
            visualObstacles.Add(currentObstacle);

            currentObstacle.transform.position = new Vector3(xPos, yOffsetVisual, zPosFront);
            zPosition[i] = zPosFront;
            zPosFront += zOffsetVisual; 
        }
    }

    void GenerateVisualObstacles()
    {
        int pick;
        float xPos = _terrainGrid[1, 1].transform.position.x + _terrainGrid[1, 1].terrainData.size.x / 2;
        pick = Random.Range(0, loadedObstacles.Count);
        currentObstacle = GetNextObstacle(pick);
        currentObstacle.SetActive(true);
        loadedObstacles.RemoveAt(pick);
        visualObstacles.Add(currentObstacle);

        currentObstacle.transform.position = new Vector3(xPos, yOffsetVisual, zPosFront);
        zPosition[counter] = zPosFront;
        counter = (counter + 1) % 3;
        zPosFront += zOffsetVisual;
    }

    void DeleteVisualObstacles()
    {
        Transform visualObstacle;
        for (int i = 0; i < visualObstacles.Count; i++)
        {
            visualObstacle = visualObstacles[i].transform;
            if (visualObstacle.position.z + 150.0f < player.transform.position.z)
            {
                visualObstacle.gameObject.SetActive(false);
                visualObstacles.Remove(visualObstacle.gameObject);
                loadedObstacles.Add(visualObstacle.gameObject);
            }
        }
    }

    void GenerateSoundObstacles(float zPosition)
    {
        soundObstacle.GetComponentInChildren<PickUpScript>().pickedUp = false;
        soundObstacle.transform.position = new Vector3(Random.Range(_terrainGrid[0, 0].transform.position.x, _terrainGrid[0, 2].transform.position.x + _terrainGrid[0, 2].terrainData.size.x), yOffsetSound, zPosition);
    }

    void DeleteSoundObstacles()
    {

    }

    private void MoveVisualObstacles()
    {
        for (int i = 0; i < visualObstacles.Count; i++)
        {
            if (visualObstacles[i].transform.position.x < _terrainGrid[0, 0].transform.position.x)
            {
                visualObstacles[i].transform.position = (new Vector3(_terrainGrid[0, 2].transform.position.x + _terrainGrid[0, 2].terrainData.size.x / 2, visualObstacles[i].transform.position.y, visualObstacles[i].transform.position.z));
            }

            if (visualObstacles[i].transform.position.x > _terrainGrid[0, 2].transform.position.x + _terrainGrid[0, 2].terrainData.size.x)
            {
                visualObstacles[i].transform.position = (new Vector3(_terrainGrid[0, 0].transform.position.x + _terrainGrid[0, 2].terrainData.size.x / 2, visualObstacles[i].transform.position.y, visualObstacles[i].transform.position.z));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTerrainGrid();

        UpdateVisualObstacles();

        DeleteVisualObstacles();

        UpdateSoundObstacle();
    }

    private void UpdateVisualObstacles()
    {
        //only when changhe tile in z
        if (player.transform.position.z > zOffsetVisual * mainCounter)
        {
            GenerateVisualObstacles();
            mainCounter++;
        }

        xPos = player.transform.position.x;
        if ((int) xPos < lowerClamp)
        {
            upperClamp = lowerClamp;
            lowerClamp -= 2 * xClamp;
            //Debug.Log("Left");
            CreateNewPuzzleOnLeft();
        }

        if ((int) xPos > upperClamp)
        {
            lowerClamp = upperClamp;
            upperClamp += 2 * xClamp;
            //Debug.Log("Right");
            CreateNewPuzzleOnRight();
        }
    }

    private void UpdateSoundObstacle()
    {
        if (player.transform.position.z % 2000 <= 1)
        {
            GenerateSoundObstacles(player.transform.position.z + zOffsetFromPlayer + soundObstacle.transform.localScale.z / 2.0f);
        }
    }

    private void UpdateTerrainGrid()
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
    }
}

    private void CreateNewPuzzleOnLeft()
    {
        zPosSide = zPosition[counter];
        float xPos = _terrainGrid[1, 1].transform.position.x - 3 * _terrainGrid[1, 1].terrainData.size.x;
        int pick = Random.Range(0, loadedObstacles.Count);
        GameObject currentObstacle = GetNextObstacle(pick);
        currentObstacle.SetActive(true);
        loadedObstacles.RemoveAt(pick);
        visualObstacles.Add(currentObstacle);

        currentObstacle.transform.position = new Vector3(xPos, yOffsetVisual, zPosSide);
    }

    private void CreateNewPuzzleOnRight()
    {
        zPosSide = zPosition[counter];
        float xPos = _terrainGrid[1, 1].transform.position.x + 3 * _terrainGrid[1, 1].terrainData.size.x;
        int pick = Random.Range(0, loadedObstacles.Count);
        GameObject currentObstacle = GetNextObstacle(pick);
        currentObstacle.SetActive(true);
        loadedObstacles.RemoveAt(pick);
        visualObstacles.Add(currentObstacle);

        currentObstacle.transform.position = new Vector3(xPos, yOffsetVisual, zPosSide);
    }
}