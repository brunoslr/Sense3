﻿using UnityEngine;
using System.Collections.Generic;

public class InfinitePlaneGenerator : MonoBehaviour
{
    public GameObject planePrefab; 
    public GameObject player;
    public int numberOfObstaclesInEachDifficultyLevel;
    public int numberOfCopiesOfEachObstacle;

    //Visual obstacle requirements
    public GameObject[] visualObstaclePrefabs;
    public int visualDisplacementHorizontal;
    public int visualDisplacementHorizontalRandomRange;
    public int visualDisplacementForward;
    public int visualDisplacementForwardInitial;
    public int visualDisplacementVertical;

    //Sound obstacle placement requirements
    public GameObject soundObstaclePrefab;
    public int soundDisplacementRandomFactor;
    public int soundDisplacementFromPlayer;

    //Tactile obstacle placement requirements
    public GameObject tactileObstaclePrefab;
    public int tactileDisplacementInitial;
    public int tactileDisplacementRandomFactorLow;
    public int tactileDisplacementRandomFactorHigh;
    public int tactileDisplacementFromPlayer;

    //Dynamic obstacle placement  
    public GameObject dynamicObstaclePrefab;
    public int dynamicDisplacementRandomFactorLow;
    public int dynamicDisplacementRandomFactorHigh;

    [SerializeField]
    Transform obstacleCollector;

    private List<GameObject> visualObstacles;
    private List<GameObject> loadedVisualObstacles;
    private GameObject visualObstacle;

    //Plane update requirements
    private float sizeOfPlaneX;
    private float sizeOfPlaneZ;
    private GameObject[,] planes = new GameObject[3, 3];

    private int playerXPosition;
    //private float playerYPosition; removed -- Unused
    private int playerZPosition;

    private int xPosVisualObstacle;

    private GameObject soundObstacle;
    private int soundZScale;
    //private float initialSoundZScale;
    //private uint soundCounter;

    private GameObject tactileObstacle;
    private int tactileZScale;

    private GameObject dynamicObstacle;

    private int xClamp;
    private int lowerClamp;
    private int upperClamp;

    private int visualPlacementZTrigger;
    private int visualPlacementInitialTrigger;
    private int nextVisualZDisplacement;

    private int nextSoundZDisplacement;
    private int soundPlacementZTrigger;

    private int nextTactileZDisplacement;
    private int tactilePlacementZTrigger;

    private int dynamicPlacementZTrigger;

    private int currentPlayerLevel;
    private int previousPlayerLevel;

    private int[] rotationArray = { 0, 90, 180, 270 };

    private int start = 0, mid = 0, end = 0, limit = 0;

    void Start()
    {
        InitializePlayerVariables();

        SetupPlaneGrid();

        LoadVisualObstacles();

        LoadSoundObstacle();

        LoadTactileObstacle();

        InitializeVisualObstacles();

        CoreSystem.onSoundEvent += IncrementLevel;
        CoreSystem.onObstacleEvent += DecrementLevel;

        //soundCounter = 0;
    }

    void OnDisable()
    {
        CoreSystem.onSoundEvent -= IncrementLevel;
        CoreSystem.onObstacleEvent -= DecrementLevel;
    }

    private void InitializePlayerVariables()
    {
        playerXPosition = (int)player.transform.position.x;
        playerZPosition = (int)player.transform.position.z;
        previousPlayerLevel = PlayerStateScript.getPlayerLevel();
        currentPlayerLevel = previousPlayerLevel;
    }

    private void SetupPlaneGrid()
    {
        sizeOfPlaneX = planePrefab.GetComponent<Renderer>().bounds.size.x;
        sizeOfPlaneZ = planePrefab.GetComponent<Renderer>().bounds.size.z;
        Vector3 planePosition;
        for (int i = 0; i < 3; i++)
        {
            for (int c = 0; c < 3; c++)
            {
                planes[i, c] = Instantiate(planePrefab);
                planes[i, c].name = i + "," + c;
                planePosition = new Vector3((float)sizeOfPlaneX * (i - 1), -1, (float)sizeOfPlaneZ * ((c - 1) * -1));
                planes[i, c].transform.position = planePosition;
            }
        }
    }

    private void LoadVisualObstacles()
    {
        xClamp = visualDisplacementHorizontal / 2;
        lowerClamp = (int)player.transform.position.x - xClamp;
        upperClamp = (int)player.transform.position.x + xClamp;

        xPosVisualObstacle = (int)player.transform.position.x;

        visualObstacles = new List<GameObject>();
        loadedVisualObstacles = new List<GameObject>();

        GameObject temp = new GameObject();
        
        for (int i = 0; i < 3 * numberOfObstaclesInEachDifficultyLevel * numberOfCopiesOfEachObstacle; i++)
        {
            if (i < numberOfObstaclesInEachDifficultyLevel * numberOfCopiesOfEachObstacle)
            {
                InstantiateVisualObstacle(visualObstaclePrefabs[i % numberOfObstaclesInEachDifficultyLevel], false);
            }
            else if (i >= numberOfObstaclesInEachDifficultyLevel * numberOfCopiesOfEachObstacle && i < 2 * numberOfObstaclesInEachDifficultyLevel * numberOfCopiesOfEachObstacle)
            {
                InstantiateVisualObstacle(visualObstaclePrefabs[numberOfObstaclesInEachDifficultyLevel + (i % numberOfObstaclesInEachDifficultyLevel)], false);
            }
            else if (i >= 2 * numberOfObstaclesInEachDifficultyLevel * numberOfCopiesOfEachObstacle && i < 3 * numberOfObstaclesInEachDifficultyLevel * numberOfCopiesOfEachObstacle)
            {
                InstantiateVisualObstacle(visualObstaclePrefabs[2 * numberOfObstaclesInEachDifficultyLevel + (i % numberOfObstaclesInEachDifficultyLevel)], false);
            }          
        }
        start = 0;
        mid = start + ((numberOfObstaclesInEachDifficultyLevel * numberOfCopiesOfEachObstacle) / 3);
        end = mid + ((numberOfObstaclesInEachDifficultyLevel * numberOfCopiesOfEachObstacle) / 3);
        limit = end + ((numberOfObstaclesInEachDifficultyLevel * numberOfCopiesOfEachObstacle) / 3);
        visualPlacementZTrigger = visualDisplacementForwardInitial;
        visualPlacementInitialTrigger = visualDisplacementForwardInitial - visualDisplacementForward;
    }


    /// <summary>
    /// Instantiate clone game object based on the Original
    /// </summary>
    /// <param name="original"></param>
    public void InstantiateVisualObstacle(GameObject original, bool setActive)
    {
        GameObject obstacle = Instantiate(original);
        obstacle.transform.SetParent(obstacleCollector);
        loadedVisualObstacles.Add(obstacle);
        obstacle.SetActive(setActive);
    }

    private void LoadSoundObstacle()
    {
        soundObstacle = Instantiate(soundObstaclePrefab);
        soundObstacle.SetActive(false);
        soundZScale = (int)soundObstacle.transform.localScale.z;
        nextSoundZDisplacement = soundDisplacementFromPlayer + (soundZScale / 4);
        soundObstacle.transform.position = new Vector3(Random.Range(lowerClamp, upperClamp), 0, nextSoundZDisplacement);
        soundPlacementZTrigger = nextSoundZDisplacement + (soundZScale / 2) + 500;
        soundObstacle.SetActive(true);
    }

    private void LoadTactileObstacle()
    {
        tactileObstacle = Instantiate(tactileObstaclePrefab);
        tactileObstacle.SetActive(false);
        tactileZScale = (int)tactileObstacle.transform.localScale.z;
        nextTactileZDisplacement = tactileDisplacementInitial + (tactileZScale / 2);
        tactileObstacle.transform.position = new Vector3(Random.Range(lowerClamp, upperClamp), 0, nextTactileZDisplacement);
        tactilePlacementZTrigger = nextTactileZDisplacement + (tactileZScale / 2) + Random.Range(tactileDisplacementRandomFactorLow, tactileDisplacementRandomFactorHigh);
        tactileObstacle.SetActive(true);
    }

    private void InitializeVisualObstacles()
    {
        int pick;
        int rotX, rotY, rotZ;
        float xpos = xPosVisualObstacle - (2 * visualDisplacementHorizontal) + Random.Range(-visualDisplacementHorizontalRandomRange, visualDisplacementHorizontalRandomRange);
        float zpos = visualDisplacementForwardInitial;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                pick = Random.Range(start, limit);
                limit--;
                visualObstacle = loadedVisualObstacles[pick];
                visualObstacle.SetActive(true);
                loadedVisualObstacles.RemoveAt(pick);
                visualObstacles.Add(visualObstacle);
                if (visualObstacle.name != "Obstacle 3_easy(Clone)" && visualObstacle.name != "Obstacle 3_medium(Clone)" && visualObstacle.name != "Obstacle 3_hard(Clone)" &&
                    visualObstacle.name != "Obstacle 5_easy(Clone)" && visualObstacle.name != "Obstacle 5_medium(Clone)" && visualObstacle.name != "Obstacle 5_hard(Clone)")
                {
                    rotX = rotationArray[Random.Range(0, 4)];
                    rotY = rotationArray[Random.Range(0, 4)];
                    rotZ = rotationArray[Random.Range(0, 4)];
                    visualObstacle.transform.Rotate(new Vector3(rotX, rotY, rotZ));
                }
                visualObstacle.transform.position = new Vector3(xpos, visualDisplacementVertical, zpos);
                xpos += visualDisplacementHorizontal + Random.Range(-visualDisplacementHorizontalRandomRange, visualDisplacementHorizontalRandomRange);

            }
            zpos += ((5 - (i+1)) / 5.0f) * visualDisplacementForwardInitial;
            xpos = xPosVisualObstacle - (2 * visualDisplacementHorizontal) + Random.Range(-visualDisplacementHorizontalRandomRange, visualDisplacementHorizontalRandomRange);
        }
        nextVisualZDisplacement = (int)zpos;
    }

    void Update()
    {
        UpdatePlayer();

        DeleteVisualObstacles();

        UpdatePlane();

        if (currentPlayerLevel < 7)
        {
            UpdateVisualObstacles();

            UpdateSoundObstacle();

            UpdateTactileObstacle();
        }

        else
        {
            DisableObstacles();
        }
    }

    private void UpdatePlayer()
    {
        playerXPosition = (int)player.transform.position.x;
        playerZPosition = (int)player.transform.position.z;
        currentPlayerLevel = PlayerStateScript.getPlayerLevel();

        if (previousPlayerLevel != currentPlayerLevel)
        {
            if (currentPlayerLevel >= 0 && currentPlayerLevel <= 2)
            {
                tactileDisplacementRandomFactorLow = 3000;
                tactileDisplacementRandomFactorHigh = 4500;
            }
            else if (currentPlayerLevel >= 3 && currentPlayerLevel <= 5)
            {
                tactileDisplacementRandomFactorLow = 1500;
                tactileDisplacementRandomFactorHigh = 3000;
            }
            else if (currentPlayerLevel >= 6 && currentPlayerLevel <= 7)
            {
                tactileDisplacementRandomFactorLow = 0;
                tactileDisplacementRandomFactorHigh = 1500;
            }
            previousPlayerLevel = currentPlayerLevel;
        }
    }

    private void UpdatePlane()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int c = 0; c < 3; c++)
            {
                float planeXMin = planes[i, c].transform.position.x - (sizeOfPlaneX / 2);
                float planeXMax = planeXMin + sizeOfPlaneX;
                float planeZMin = planes[i, c].transform.position.z - (sizeOfPlaneZ / 2);
                float planeZMax = planeZMin + sizeOfPlaneZ;
                if (playerXPosition > planeXMin && playerXPosition < planeXMax && playerZPosition > planeZMin && playerZPosition < planeZMax)
                {
                    UpdatePlaneGrid(i, c);
                }
            }
        }
    }

    private void UpdatePlaneGrid(int x, int z)
    {
        if (x == 0)
        {
            MoveLeft();
        }
        else if (x == 2)
        {
            MoveRight();
        }
        if (z == 0)
        {
            MoveAhead();
        }
    }

    private void MoveLeft()
    {
        GameObject[,] newPlanes = new GameObject[3, 3];
        for (int i = 0; i < 3; i++)
        {
            Vector3 addPos = new Vector3(-sizeOfPlaneX * 3, 0, 0);
            planes[2, i].transform.position += addPos;
        }
        for (int i = 0; i < 3; i++)
        {
            int c = i - 1;
            c = (c == -1) ? 2 : c;
            newPlanes[i, 0] = planes[c, 0];
            newPlanes[i, 1] = planes[c, 1];
            newPlanes[i, 2] = planes[c, 2];
        }
        planes = newPlanes;
    }

    private void MoveRight()
    {
        GameObject[,] newPlanes = new GameObject[3, 3];
        for (int i = 0; i < 3; i++)
        {
            Vector3 addPos = new Vector3(sizeOfPlaneX * 3, 0, 0);
            planes[0, i].transform.position += addPos;
        }

        for (int i = 0; i < 3; i++)
        {
            int c = i + 1;
            c = (c == 3) ? 0 : c;
            newPlanes[i, 0] = planes[c, 0];
            newPlanes[i, 1] = planes[c, 1];
            newPlanes[i, 2] = planes[c, 2];
        }

        planes = newPlanes;
    }

    private void MoveAhead()
    {
        GameObject[,] newPlanes = new GameObject[3, 3];
        for (int i = 0; i < 3; i++)
        {
            Vector3 addPos = new Vector3(0, 0, sizeOfPlaneZ * 3);
            planes[i, 2].transform.position += addPos;
        }
        for (int i = 0; i < 3; i++)
        {
            int c = i - 1;
            c = (c == -1) ? 2 : c;
            newPlanes[0, i] = planes[0, c];
            newPlanes[1, i] = planes[1, c];
            newPlanes[2, i] = planes[2, c];
        }
        planes = newPlanes;
    }

    private void UpdateVisualObstacles()
    {

        if ((int)playerXPosition < lowerClamp)
        {
            xPosVisualObstacle -= (int)visualDisplacementHorizontal;
            upperClamp = lowerClamp;
            lowerClamp -= 2 * xClamp;
            CreateVisualObstaclesOnSide(-1.0f);
        }

        if ((int)playerXPosition > upperClamp)
        {
            xPosVisualObstacle += (int)visualDisplacementHorizontal;
            lowerClamp = upperClamp;
            upperClamp += 2 * xClamp;
            CreateVisualObstaclesOnSide(1.0f);
        }

        if (playerZPosition > visualPlacementZTrigger)
        {
            if (visualPlacementInitialTrigger > 100)
            {
                visualPlacementZTrigger += visualPlacementInitialTrigger;
                visualPlacementInitialTrigger -= visualDisplacementForward;
            }
            else
            {
                visualPlacementZTrigger += (int)visualDisplacementForward;
            }
            nextVisualZDisplacement += (int)visualDisplacementForward;
            GenerateVisualObstacles();
        }
    }

    private void GenerateVisualObstacles()
    {
        int pick;
        int rotX, rotY, rotZ;
        float xpos = xPosVisualObstacle - (2 * visualDisplacementHorizontal) + Random.Range(-visualDisplacementHorizontalRandomRange, visualDisplacementHorizontalRandomRange);
        for (int i = 0; i < 5; ++i)
        {
            do { 
            pick = Random.Range(start, limit);
                Debug.Log("Start: " + start + "Mid: " + mid + "End: " + end + "Limit: " + limit);
                Debug.Log("Pick: " + pick);
                Debug.Log("List Size: " + loadedVisualObstacles.Count);
                visualObstacle = loadedVisualObstacles[pick];
            if (currentPlayerLevel == 6)
                {
                   // Debug.Log("Start: " + start + "Mid: " + mid + "End: " + end + "Limit: " + limit);
                   // Debug.Log("Pick: " + pick);
                   // Debug.Log("List Size: " + loadedVisualObstacles.Count);
                }
        } while (visualObstacle == null) ;
            visualObstacle.SetActive(true);
            limit--;
            loadedVisualObstacles.RemoveAt(pick);
            visualObstacles.Add(visualObstacle);
            if (visualObstacle.name != "Obstacle 3_easy(Clone)" && visualObstacle.name != "Obstacle 3_medium(Clone)" && visualObstacle.name != "Obstacle 3_hard(Clone)" &&
                visualObstacle.name != "Obstacle 5_easy(Clone)" && visualObstacle.name != "Obstacle 5_medium(Clone)" && visualObstacle.name != "Obstacle 5_hard(Clone)")
            {
                rotX = rotationArray[Random.Range(0, 4)];
                rotY = rotationArray[Random.Range(0, 4)];
                rotZ = rotationArray[Random.Range(0, 4)];
                visualObstacle.transform.Rotate(new Vector3(rotX, rotY, rotZ));
            }
            visualObstacle.transform.position = new Vector3(xpos, visualDisplacementVertical, nextVisualZDisplacement);
            xpos += visualDisplacementHorizontal + Random.Range(-visualDisplacementHorizontalRandomRange, visualDisplacementHorizontalRandomRange);
            visualObstacle = null;
        }
    }

    private void CreateVisualObstaclesOnSide(float dir)
    {
        int pick;
        int rotX, rotY, rotZ;
        
        for (int i = -5; i < 1; i++)
        {
            float xpos = xPosVisualObstacle + (2 * visualDisplacementHorizontal * dir) + (Random.Range(0, visualDisplacementHorizontalRandomRange) * dir);
            Vector3 newPosition = new Vector3(xpos, 0, nextVisualZDisplacement + (visualDisplacementForward * i));
            if ((Physics.OverlapBox(newPosition, Vector3.one).Length <= 2))
            {
                do
                {
                    pick = Random.Range(start, limit-1);
                    visualObstacle = loadedVisualObstacles[pick];
                } while (visualObstacle == null);
                visualObstacle.SetActive(true);
                limit--;
                loadedVisualObstacles.RemoveAt(pick);
                visualObstacles.Add(visualObstacle);
                if (visualObstacle.name != "Obstacle 3_easy(Clone)" && visualObstacle.name != "Obstacle 3_medium(Clone)" && visualObstacle.name != "Obstacle 3_hard(Clone)" &&
                    visualObstacle.name != "Obstacle 5_easy(Clone)" && visualObstacle.name != "Obstacle 5_medium(Clone)" && visualObstacle.name != "Obstacle 5_hard(Clone)")
                {
                    rotX = rotationArray[Random.Range(0, 4)];
                    rotY = rotationArray[Random.Range(0, 4)];
                    rotZ = rotationArray[Random.Range(0, 4)];
                    visualObstacle.transform.Rotate(new Vector3(rotX, rotY, rotZ));
                }
                visualObstacle.transform.position = new Vector3(xpos, visualDisplacementVertical, nextVisualZDisplacement + (visualDisplacementHorizontal * i));
                visualObstacle = null;
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
                if (visualObstacle.gameObject.layer == 10)
                {
                    loadedVisualObstacles.Insert(0, visualObstacle.gameObject);
                }
                else if (visualObstacle.gameObject.layer == 11)
                {
                    loadedVisualObstacles.Insert(numberOfCopiesOfEachObstacle * numberOfObstaclesInEachDifficultyLevel, visualObstacle.gameObject);
                }
                else if (visualObstacle.gameObject.layer == 12)
                {
                    loadedVisualObstacles.Add(visualObstacle.gameObject);
                }
                visualObstacle.Rotate(Vector3.zero);
                visualObstacle.gameObject.SetActive(false);
                visualObstacles.Remove(visualObstacle.gameObject);
                //Destroy(visualObstacle.gameObject);
                limit++;
            }
        }
    }

    private void UpdateSoundObstacle()
    {
        if (playerZPosition > soundPlacementZTrigger)
        {
            nextSoundZDisplacement = playerZPosition + soundDisplacementFromPlayer + (soundZScale / 2) + Random.Range(-soundDisplacementRandomFactor, soundDisplacementRandomFactor);
            soundPlacementZTrigger = nextSoundZDisplacement + (soundZScale / 2) + 500;
            GenerateSoundObstacle(nextSoundZDisplacement);
        }
    }

    private void GenerateSoundObstacle(float zPosition)
    {
        soundObstacle.SetActive(true);
        soundObstacle.GetComponentInChildren<PickUpScript>().pickedUp = false;
        soundObstacle.transform.position = new Vector3(Random.Range(lowerClamp, upperClamp), 0, zPosition);
    }

    private void UpdateTactileObstacle()
    {
        if (playerZPosition > tactilePlacementZTrigger)
        {
            nextTactileZDisplacement = playerZPosition + tactileDisplacementFromPlayer + (tactileZScale / 2);
            tactilePlacementZTrigger = nextTactileZDisplacement + (tactileZScale / 2) + Random.Range(tactileDisplacementRandomFactorLow, tactileDisplacementRandomFactorHigh);
            GenerateTactileObstacle(nextTactileZDisplacement);
        }
    }

    private void GenerateTactileObstacle(float zPosition)
    {
        tactileObstacle.SetActive(true);
        tactileObstacle.transform.position = new Vector3(Random.Range(lowerClamp, upperClamp), 0, zPosition);
    }

    private void DisableObstacles()
    {
        soundObstacle.SetActive(false);
        tactileObstacle.SetActive(false);
    }

    private void IncrementLevel()
    {
        start = mid;
        mid = end;
        end += (numberOfObstaclesInEachDifficultyLevel * numberOfCopiesOfEachObstacle) / 3;
		limit = end + ((numberOfObstaclesInEachDifficultyLevel * numberOfCopiesOfEachObstacle) / 3);
		if (limit >= 360)
		{
			limit = loadedVisualObstacles.Count - 1;
		}
    }

    private void DecrementLevel()
    {
        end = mid;
        mid = start;
		start -= (numberOfObstaclesInEachDifficultyLevel * numberOfCopiesOfEachObstacle) / 3;
        limit = end + ((numberOfObstaclesInEachDifficultyLevel * numberOfCopiesOfEachObstacle) / 3);
		if (limit >= 360)
		{
			limit = loadedVisualObstacles.Count - 1;
		}
    }
}