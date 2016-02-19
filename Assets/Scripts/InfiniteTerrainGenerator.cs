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
    public int xClamp;

    //Sound obstacle placement requirements
    public GameObject soundObstaclePrefab;
    public float xOffsetSound;
    public float yOffsetSound;
    public float zOffsetSound;
    public float zOffsetFromPlayerSound;

    //Tactile obstacle placement requirements
    public GameObject tactileObstaclePrefab;
    public float xOffsetTactile;
    public float yOffsetTactile;
    public float zOffsetTactile;
    public float zOffsetFromPlayerTactile;

    private Vector3 playerPosition;
    private int playerXPosition;
    //private float playerYPosition; removed -- Unused
    private int playerZPosition;

    private Terrain[,] _terrainGrid = new Terrain[3, 3];
    private Terrain linkedTerrain;
    private Terrain currentTerrain;
    private int xOffset;
    private int zOffset;

    private List<GameObject> visualObstacles;

    private List<GameObject> loadedVisualObstacles;
    private GameObject visualObstacle;

    private int zPosVisualObstacle;
    private int xPosVisualObstacle;

    private GameObject soundObstacle;
    private float zOffsetBetweenSoundObstacle;

    private GameObject tactileObstacle;
    private float zOffsetBetweenTactileObstacle;

    private int lowerClamp;
    private int upperClamp;

    private int nextZ;

    // Use this for initialization
    void Start()
    {
        lowerClamp = (int) player.transform.position.x - xClamp;
        upperClamp = (int) player.transform.position.x + xClamp;

        xPosVisualObstacle = (int) player.transform.position.x;

        InitializeTerrain();
        UpdateTerrainPositionsAndNeighbors();

        visualObstacles = new List<GameObject>();
        loadedVisualObstacles = new List<GameObject>();

        GameObject temp;
        for (int i = 0; i < visualObstaclePrefabs.Length; i++)
        {
            temp = Instantiate(visualObstaclePrefabs[i]);
            temp.SetActive(false);
            loadedVisualObstacles.Add(temp);
        }

        zPosVisualObstacle = (int) zOffsetVisual;
        nextZ = (int)(zOffsetVisual - (zOffsetVisual / 2));

        zOffsetBetweenSoundObstacle = zOffsetSound;
        soundObstacle = Instantiate(soundObstaclePrefab);
        soundObstacle.SetActive(false);

        zOffsetBetweenTactileObstacle = zOffsetTactile;
        tactileObstacle = Instantiate(tactileObstaclePrefab);
        tactileObstacle.SetActive(false);

        InitializeVisualObstacles();
    }

    void Update()
    {
        playerXPosition = (int)player.transform.position.x;
        //playerYPosition = player.transform.position.y; // Removed, not used
        playerZPosition = (int)player.transform.position.z;

        UpdateTerrainGrid();

        UpdateVisualObstacles();

        DeleteVisualObstacles();

        UpdateSoundObstacle();

        UpdateTactileObstacle();
    }

    private void InitializeVisualObstacles()
    {
        int pick;
		int rotX, rotY, rotZ;
        float xpos = xPosVisualObstacle -  (2 * xOffsetVisual);
        float zpos = zPosVisualObstacle;
        for (int i = 0; i < 4; ++i)
        {
            for (int j = 0; j < 5; j++)
            {
                pick = Random.Range(0, loadedVisualObstacles.Count);
                visualObstacle = loadedVisualObstacles[pick];
                visualObstacle.SetActive(true);
                loadedVisualObstacles.RemoveAt(pick);
                visualObstacles.Add(visualObstacle);
				rotX = Random.Range (0, 360);
				rotY = Random.Range (0, 360);
				rotZ = Random.Range (0, 360);
				visualObstacle.transform.Rotate (new Vector3 (rotX, rotY, rotZ));
                visualObstacle.transform.position = new Vector3(xpos, yOffsetVisual, zpos);
                xpos += xOffsetVisual;
                
            }
            zpos += zOffsetVisual;
            xpos = xPosVisualObstacle - (2 * xOffsetVisual);
        }
        zPosVisualObstacle = (int)(zpos - zOffsetVisual);
    }

    private void GenerateVisualObstacles()
    {
        int pick;
		int rotX, rotY, rotZ;
        float xpos = xPosVisualObstacle - (2 * xOffsetVisual);
        for (int i = 0; i < 5; ++i)
        {
            pick = Random.Range(0, loadedVisualObstacles.Count);
            visualObstacle = loadedVisualObstacles[pick];
            visualObstacle.SetActive(true);
            loadedVisualObstacles.RemoveAt(pick);
            visualObstacles.Add(visualObstacle);
			rotX = Random.Range (0, 360);
			rotY = Random.Range (0, 360);
			rotZ = Random.Range (0, 360);
			visualObstacle.transform.Rotate (new Vector3 (rotX, rotY, rotZ));
            visualObstacle.transform.position = new Vector3(xpos, yOffsetVisual, zPosVisualObstacle);
            xpos += xOffsetVisual;
        }
    }

    private void UpdateVisualObstacles()
    {
        if ((int)playerXPosition < lowerClamp)
        {
            xPosVisualObstacle -= (int)xOffsetVisual;
            upperClamp = lowerClamp;
            lowerClamp -= 2 * xClamp;
            //Debug.Log("Left");
            CreateNewPuzzleOnSide(-1.0f);
        }

        if ((int)playerXPosition > upperClamp)
        {
            xPosVisualObstacle += (int)xOffsetVisual;
            lowerClamp = upperClamp;
            upperClamp += 2 * xClamp;
            //Debug.Log("Right");
            CreateNewPuzzleOnSide(1.0f);
        }

        if ( playerZPosition > nextZ)
        {
            nextZ += (int)zOffsetVisual;
            zPosVisualObstacle += (int)zOffsetVisual;
            GenerateVisualObstacles();
        }
    }

    private void CreateNewPuzzleOnSide(float dir)
    {
        float xpos = xPosVisualObstacle + (2 * xOffsetVisual * dir);
        for (int i = -4; i < 1; i++)
        {
            Vector3 newPosition = new Vector3(xpos, yOffsetVisual, zPosVisualObstacle + (zOffsetVisual * i));
            if ((Physics.OverlapBox(newPosition, Vector3.one * ((xOffsetVisual / 2.0f) - 20.0f))).Length == 0)
            {
                int pick = Random.Range(0, loadedVisualObstacles.Count);
				int rotX, rotY, rotZ;
                GameObject visualObstacle = loadedVisualObstacles[pick];
                visualObstacle.SetActive(true);
				rotX = Random.Range (0, 360);
				rotY = Random.Range (0, 360);
				rotZ = Random.Range (0, 360);
				visualObstacle.transform.Rotate (new Vector3 (rotX, rotY, rotZ));
                loadedVisualObstacles.RemoveAt(pick);
                visualObstacles.Add(visualObstacle);

                visualObstacle.transform.position = new Vector3(xpos, yOffsetVisual, zPosVisualObstacle + (zOffsetVisual * i));
            }
        }
    }

    private void DeleteVisualObstacles()
    {
        Transform visualObstacle;
        for (int i = 0; i < visualObstacles.Count; i++)
        {
            visualObstacle = visualObstacles[i].transform;
            if (visualObstacle.position.z + 150.0f < playerZPosition)
            {
				visualObstacle.Rotate (Vector3.zero);
                visualObstacle.gameObject.SetActive(false);
                visualObstacles.Remove(visualObstacle.gameObject);
                loadedVisualObstacles.Add(visualObstacle.gameObject);
            }
        }
    }

    private void UpdateSoundObstacle()
    {
        if (playerZPosition > zOffsetSound)
        {
            zOffsetSound += zOffsetBetweenSoundObstacle;
            GenerateSoundObstacle(playerZPosition + zOffsetFromPlayerSound + soundObstacle.transform.localScale.z);
        }
    }

    private void GenerateSoundObstacle(float zPosition)
    {
        soundObstacle.SetActive(true);
        soundObstacle.GetComponentInChildren<PickUpScript>().pickedUp = false;
        soundObstacle.transform.position = new Vector3(Random.Range(lowerClamp, upperClamp), yOffsetSound, zPosition);
    }

    private void UpdateTactileObstacle()
    {
        if (playerZPosition > zOffsetTactile)
        {
            zOffsetTactile += zOffsetBetweenTactileObstacle;
            GenerateTactileObstacle(playerZPosition + zOffsetFromPlayerTactile + tactileObstacle.transform.localScale.z);
        }
    }

    private void GenerateTactileObstacle(float zPosition)
    {
        tactileObstacle.SetActive(true);
        tactileObstacle.transform.position = new Vector3(Random.Range(lowerClamp, upperClamp), yOffsetTactile, zPosition);
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
}