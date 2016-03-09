using UnityEngine;
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

    //Plane update requirements
    private float sizeOfPlaneX;
    private float sizeOfPlaneZ;
    private GameObject[,] planes = new GameObject[3, 3];

    private int playerXPosition;
    //private float playerYPosition; removed -- Unused
    private int playerZPosition;

    private List<GameObject> visualObstacles;
    private List<GameObject> loadedVisualObstacles;
    private List<GameObject> loadedVisualObstaclesEasy;
    private List<GameObject> loadedVisualObstaclesMedium;
    private List<GameObject> loadedVisualObstaclesHard;
    private GameObject visualObstacle;

    private int xPosVisualObstacle;

    private GameObject soundObstacle;
    private int soundZScale;

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

    void Start()
    {
        InitializePlayerVariables();

        SetupPlaneGrid();

        LoadVisualObstacles();

        LoadSoundObstacle();

        LoadTactileObstacle();

        //LoadDynamicObstacle();

        InitializeVisualObstacles();
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
        loadedVisualObstaclesEasy = new List<GameObject>();
        loadedVisualObstaclesMedium = new List<GameObject>();
        loadedVisualObstaclesHard = new List<GameObject>();

        GameObject temp = new GameObject();
        for (int i = 0; i < 3 * numberOfObstaclesInEachDifficultyLevel * numberOfCopiesOfEachObstacle; i++)
        {
            if (i < numberOfObstaclesInEachDifficultyLevel * numberOfCopiesOfEachObstacle)
            {
                temp = Instantiate(visualObstaclePrefabs[i % numberOfObstaclesInEachDifficultyLevel]);
                loadedVisualObstaclesEasy.Add(temp);
            }
            else if (i >= numberOfObstaclesInEachDifficultyLevel * numberOfCopiesOfEachObstacle && i < 2 * numberOfObstaclesInEachDifficultyLevel * numberOfCopiesOfEachObstacle)
            {
                temp = Instantiate(visualObstaclePrefabs[numberOfObstaclesInEachDifficultyLevel + (i % numberOfObstaclesInEachDifficultyLevel)]);
                loadedVisualObstaclesMedium.Add(temp);
            }
            else if (i >= 2 * numberOfObstaclesInEachDifficultyLevel * numberOfCopiesOfEachObstacle && i < 3 * numberOfObstaclesInEachDifficultyLevel * numberOfCopiesOfEachObstacle)
            {
                temp = Instantiate(visualObstaclePrefabs[2 * numberOfObstaclesInEachDifficultyLevel + (i % numberOfObstaclesInEachDifficultyLevel)]);
                loadedVisualObstaclesHard.Add(temp);
            }
            temp.SetActive(false);
        }
        loadedVisualObstacles = loadedVisualObstaclesEasy;
        visualPlacementZTrigger = visualDisplacementForwardInitial;
        visualPlacementInitialTrigger = visualDisplacementForwardInitial - visualDisplacementForward;
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

    private void LoadDynamicObstacle()
    {
        dynamicObstacle = Instantiate(dynamicObstaclePrefab);
        dynamicObstacle.SetActive(false);
        dynamicPlacementZTrigger = Random.Range(dynamicDisplacementRandomFactorLow, dynamicDisplacementRandomFactorHigh); 
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
                pick = Random.Range(0, loadedVisualObstacles.Count - 1);
                visualObstacle = loadedVisualObstacles[pick];
                visualObstacle.SetActive(true);
                loadedVisualObstacles.RemoveAt(pick);
                visualObstacles.Add(visualObstacle);
                if (visualObstacle.name != "Obstacle 10_easy(Clone)" && visualObstacle.name != "Obstacle 10_medium(Clone)" && visualObstacle.name != "Obstacle 10_hard(Clone)")
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
        DeleteVisualObstacles();

        UpdatePlayer();

        UpdatePlane();

        if (currentPlayerLevel < 7)
        {
            UpdateVisualObstacles();

            UpdateSoundObstacle();

            UpdateTactileObstacle();
        }

        //UpdateDynamicObstacle();
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
                if (loadedVisualObstacles != loadedVisualObstaclesEasy)
                {
                    loadedVisualObstacles = loadedVisualObstaclesEasy;
                }
            }
            else if (currentPlayerLevel >= 3 && currentPlayerLevel <= 4)
            {
                tactileDisplacementRandomFactorLow = 1500;
                tactileDisplacementRandomFactorHigh = 3000;
                if (loadedVisualObstacles != loadedVisualObstaclesMedium)
                {
                    loadedVisualObstacles = loadedVisualObstaclesMedium;
                }
            }
            else if (currentPlayerLevel >= 5 && currentPlayerLevel <= 7)
            {
                tactileDisplacementRandomFactorLow = 0;
                tactileDisplacementRandomFactorHigh = 1500;
                if (loadedVisualObstacles != loadedVisualObstaclesHard)
                {
                    loadedVisualObstacles = loadedVisualObstaclesHard;
                }
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
            pick = Random.Range(0, loadedVisualObstacles.Count - 1);
            visualObstacle = loadedVisualObstacles[pick];
            visualObstacle.SetActive(true);
            loadedVisualObstacles.RemoveAt(pick);
            visualObstacles.Add(visualObstacle);
            if (visualObstacle.name != "Obstacle 10_easy(Clone)" && visualObstacle.name != "Obstacle 10_medium(Clone)" && visualObstacle.name != "Obstacle 10_hard(Clone)")
            {
                rotX = rotationArray[Random.Range(0, 4)];
                rotY = rotationArray[Random.Range(0, 4)];
                rotZ = rotationArray[Random.Range(0, 4)];
                visualObstacle.transform.Rotate(new Vector3(rotX, rotY, rotZ));
            }
            visualObstacle.transform.position = new Vector3(xpos, visualDisplacementVertical, nextVisualZDisplacement);
            xpos += visualDisplacementHorizontal + Random.Range(-visualDisplacementHorizontalRandomRange, visualDisplacementHorizontalRandomRange);
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
                pick = Random.Range(0, loadedVisualObstacles.Count - 1);
                GameObject visualObstacle = loadedVisualObstacles[pick];
                visualObstacle.SetActive(true);
                loadedVisualObstacles.RemoveAt(pick);
                visualObstacles.Add(visualObstacle);
                if (visualObstacle.name != "Obstacle 10_easy(Clone)" && visualObstacle.name != "Obstacle 10_medium(Clone)" && visualObstacle.name != "Obstacle 10_hard(Clone)")
                {
                    rotX = rotationArray[Random.Range(0, 4)];
                    rotY = rotationArray[Random.Range(0, 4)];
                    rotZ = rotationArray[Random.Range(0, 4)];
                    visualObstacle.transform.Rotate(new Vector3(rotX, rotY, rotZ));
                }
                visualObstacle.transform.position = new Vector3(xpos, visualDisplacementVertical, nextVisualZDisplacement + (visualDisplacementHorizontal * i));
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
                    if (currentPlayerLevel >= 0 && currentPlayerLevel <= 2)
                    {
                        loadedVisualObstacles.Add(visualObstacle.gameObject);
                    }
                    else
                    {
                        loadedVisualObstaclesEasy.Add(visualObstacle.gameObject);
                    }
                }
                else if (visualObstacle.gameObject.layer == 11)
                {
                    if (currentPlayerLevel >= 3 && currentPlayerLevel <= 5)
                    {
                        loadedVisualObstacles.Add(visualObstacle.gameObject);
                    }
                    else
                    {
                        loadedVisualObstaclesMedium.Add(visualObstacle.gameObject);
                    }
                }
                else if (visualObstacle.gameObject.layer == 12)
                {
                    if (currentPlayerLevel >= 6 && currentPlayerLevel <= 7)
                    {
                        loadedVisualObstacles.Add(visualObstacle.gameObject);
                    }
                    else
                    {
                        loadedVisualObstaclesHard.Add(visualObstacle.gameObject);
                    }
                }
                visualObstacle.Rotate(Vector3.zero);
                visualObstacle.gameObject.SetActive(false);
                visualObstacles.Remove(visualObstacle.gameObject);
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

    private void UpdateDynamicObstacle()
    {
          if (playerZPosition > dynamicPlacementZTrigger)
        {
            dynamicObstacle.SetActive(false);
            dynamicPlacementZTrigger += Random.Range(dynamicDisplacementRandomFactorLow, dynamicDisplacementRandomFactorHigh);
            dynamicObstacle.SetActive(true);
        }
    }
}