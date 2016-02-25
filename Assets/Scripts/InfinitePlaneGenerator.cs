using UnityEngine;
using System.Collections.Generic;

public class InfinitePlaneGenerator : MonoBehaviour
{

    public GameObject planePrefab; 
    public GameObject player;

    //Visual obstacle requirements
    public GameObject[] visualObstaclePrefabs;
    public int visualDisplacementHorizontal;
    public int visualDisplacementForward;
    public int visualDisplacementForwardInitial;


    //Sound obstacle placement requirements
    public GameObject soundObstaclePrefab;
    public int soundDisplacementForward;
    public int soundDisplacementRandomFactor;
    public int soundDisplacementFromPlayer;

    //Tactile obstacle placement requirements
    public GameObject tactileObstaclePrefab;
    public int tactileDisplacementForward;
    public int tactileDisplacementRandomFactor;
    public int tactileDisplacementFromPlayer;

    //Plane update requirements
    private float sizeOfPlaneX;
    private float sizeOfPlaneZ;
    private GameObject[,] planes = new GameObject[3, 3];

    private int playerXPosition;
    //private float playerYPosition; removed -- Unused
    private int playerZPosition;

    private List<GameObject> visualObstacles;
    private List<GameObject> loadedVisualObstacles;
    private GameObject visualObstacle;

    private int zPosVisualObstacle;
    private int xPosVisualObstacle;

    private GameObject soundObstacle;
    private int soundZScale;

    private GameObject tactileObstacle;
    private int tactileZScale;

    private int xClamp;
    private int lowerClamp;
    private int upperClamp;

    private int visualPlacementZTrigger;
    private int visualPlacementInitialTrigger;
    private int nextVisualZDisplacement;

    private int nextSoundZDispalcement;
    private int soundPlacementZTrigger;

    private int nextTactileZDisplacement;
    private int tactilePlacementZTrigger;

    void Start()
    {
        SetupPlaneGrid();

        LoadVisualObstacles();

        LoadSoundObstacle();

        LoadTactileObstacle();

        InitializeVisualObstacles();
    }

    private void SetupPlaneGrid()
    {
        playerXPosition = (int)player.transform.position.x;
        playerZPosition = (int)player.transform.position.z;
        sizeOfPlaneX = planePrefab.GetComponent<Renderer>().bounds.size.x;
        sizeOfPlaneZ = planePrefab.GetComponent<Renderer>().bounds.size.z;
        Vector3 planePosition;
        for (int i = 0; i < 3; i++)
        {
            for (int c = 0; c < 3; c++)
            {
                planes[i, c] = GameObject.Instantiate(planePrefab) as GameObject;
                planes[i, c].name = i + "," + c;
                planePosition = new Vector3((float)sizeOfPlaneX * (i - 1), -5, (float)sizeOfPlaneZ * ((c - 1) * -1));
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

        GameObject temp;
        for (int i = 0; i < visualObstaclePrefabs.Length * 20; i++)
        {
            temp = Instantiate(visualObstaclePrefabs[i % visualObstaclePrefabs.Length]);
            temp.SetActive(false);
            loadedVisualObstacles.Add(temp);
        }
        visualPlacementZTrigger = visualDisplacementForwardInitial;
        visualPlacementInitialTrigger = visualDisplacementForwardInitial - visualDisplacementForward;
    }

    private void LoadSoundObstacle()
    {
        soundObstacle = Instantiate(soundObstaclePrefab);
        soundObstacle.SetActive(false);
        soundZScale = (int)soundObstacle.transform.localScale.z;
        nextSoundZDispalcement = soundDisplacementFromPlayer + (soundZScale / 4);
        soundObstacle.transform.position = new Vector3(Random.Range(lowerClamp, upperClamp), 0, nextSoundZDispalcement);
        soundPlacementZTrigger = nextSoundZDispalcement + (soundZScale / 2) + soundDisplacementFromPlayer;
        soundObstacle.SetActive(true);
    }

    private void LoadTactileObstacle()
    {
        tactileObstacle = Instantiate(tactileObstaclePrefab);
        tactileObstacle.SetActive(false);
        tactileZScale = (int)tactileObstacle.transform.localScale.z;
    }

    private void InitializeVisualObstacles()
    {
        int pick;
        int rotX, rotY, rotZ;
        float xpos = xPosVisualObstacle - (2 * visualDisplacementHorizontal);
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
                rotX = Random.Range(0, 360);
                rotY = Random.Range(0, 360);
                rotZ = Random.Range(0, 360);
                visualObstacle.transform.Rotate(new Vector3(rotX, rotY, rotZ));
                visualObstacle.transform.position = new Vector3(xpos, 0, zpos);
                xpos += visualDisplacementHorizontal;

            }
            zpos += ((5 - (i+1)) / 5.0f) * visualDisplacementForwardInitial;
            xpos = xPosVisualObstacle - (2 * visualDisplacementHorizontal);
        }
        nextVisualZDisplacement = (int)zpos;
    }

    void Update()
    {
        playerXPosition = (int)player.transform.position.x;
        //playerYPosition = player.transform.position.y; // Removed, not used
        playerZPosition = (int)player.transform.position.z;

        UpdatePlane();

        UpdateVisualObstacles();

        DeleteVisualObstacles();

        UpdateSoundObstacle();

        //UpdateTactileObstacle();
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
        float xpos = xPosVisualObstacle - (2 * visualDisplacementHorizontal);
        for (int i = 0; i < 5; ++i)
        {
            pick = Random.Range(0, loadedVisualObstacles.Count);
            visualObstacle = loadedVisualObstacles[pick];
            visualObstacle.SetActive(true);
            loadedVisualObstacles.RemoveAt(pick);
            visualObstacles.Add(visualObstacle);
            rotX = Random.Range(0, 360);
            rotY = Random.Range(0, 360);
            rotZ = Random.Range(0, 360);
            visualObstacle.transform.Rotate(new Vector3(rotX, rotY, rotZ));
            visualObstacle.transform.position = new Vector3(xpos, 0, nextVisualZDisplacement);
            xpos += visualDisplacementHorizontal;
        }
    }

    private void CreateVisualObstaclesOnSide(float dir)
    {
        int pick;
        int rotX, rotY, rotZ;
        float xpos = xPosVisualObstacle + (2 * visualDisplacementHorizontal * dir);
        for (int i = -5; i < 1; i++)
        {
            Vector3 newPosition = new Vector3(xpos, 0, nextVisualZDisplacement + (visualDisplacementHorizontal * i));
            if ((Physics.OverlapBox(newPosition, Vector3.one).Length == 0))
            {
                pick = Random.Range(0, loadedVisualObstacles.Count);
                GameObject visualObstacle = loadedVisualObstacles[pick];
                visualObstacle.SetActive(true);
                loadedVisualObstacles.RemoveAt(pick);
                visualObstacles.Add(visualObstacle);
                rotX = Random.Range(0, 360);
                rotY = Random.Range(0, 360);
                rotZ = Random.Range(0, 360);
                visualObstacle.transform.Rotate(new Vector3(rotX, rotY, rotZ));
                visualObstacle.transform.position = new Vector3(xpos, 0, nextVisualZDisplacement + (visualDisplacementHorizontal * i));
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
                visualObstacle.Rotate(Vector3.zero);
                visualObstacle.gameObject.SetActive(false);
                visualObstacles.Remove(visualObstacle.gameObject);
                loadedVisualObstacles.Add(visualObstacle.gameObject);
            }
        }
    }

    private void UpdateSoundObstacle()
    {
        if (playerZPosition > soundPlacementZTrigger)
        {
            nextSoundZDispalcement = playerZPosition + soundDisplacementFromPlayer + soundZScale / 4;
            soundPlacementZTrigger = nextSoundZDispalcement + soundDisplacementFromPlayer + (soundZScale/2);
            GenerateSoundObstacle(nextSoundZDispalcement);
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
        //if (playerZPosition > zOffsetTactile)
        //{
        //    zOffsetTactile += zOffsetBetweenTactileObstacle;
        //    GenerateTactileObstacle(playerZPosition + zOffsetFromPlayerTactile + tactileObstacle.transform.localScale.z);
        //}
    }

    private void GenerateTactileObstacle(float zPosition)
    {
        tactileObstacle.SetActive(true);
        tactileObstacle.transform.position = new Vector3(Random.Range(lowerClamp, upperClamp), 0, zPosition);
    }
}