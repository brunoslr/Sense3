using UnityEngine;
using System.Collections.Generic;

public class InfinitePlaneGenerator_Oculus : MonoBehaviour
{

    public GameObject planePrefab; 
    public GameObject player;
    public int numberOfCopiesOfEachObstacle;

    //Visual obstacle requirements
    public GameObject[] visualObstaclePrefabs;
    public int visualDisplacementHorizontal;
    public int visualDisplacementForward;
    public float visualDisplacementHorizontalRandomRange = 1000;
    public float yDisplacement = 500;

    //Sound obstacle placement requirements
    public GameObject soundObstaclePrefab;
    public int soundDisplacementRandomFactor;
    public int soundDisplacementFromPlayer;

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

    void Start()
    {
        InitializePlayerVariables();

        LoadVisualObstacles();

        SetupPlaneGrid();

        LoadSoundObstacle();

        InitializeVisualObstacles();
    }

    private void InitializeVisualObstacles()
    {
        int pick;
        int rotX, rotY, rotZ;
        float xpos = xPosVisualObstacle - (2 * visualDisplacementHorizontal) + Random.Range(-visualDisplacementHorizontalRandomRange, visualDisplacementHorizontalRandomRange);
        float zpos = visualDisplacementForward;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                pick = Random.Range(0, loadedVisualObstacles.Count - 1);
                visualObstacle = loadedVisualObstacles[pick];
                visualObstacle.SetActive(true);
                loadedVisualObstacles.RemoveAt(pick);
                visualObstacles.Add(visualObstacle);

                rotX = Random.Range(-45, 45);
                rotY = Random.Range(-45, 45);
                rotZ = Random.Range(-45, 45);
                
                visualObstacle.transform.Rotate(new Vector3(rotX, rotY, rotZ));

                visualObstacle.transform.position = new Vector3(xpos, Random.Range(-yDisplacement, yDisplacement), zpos);
                xpos += visualDisplacementHorizontal + Random.Range(-visualDisplacementHorizontalRandomRange, visualDisplacementHorizontalRandomRange);

            }
            zpos += visualDisplacementForward;
            xpos = xPosVisualObstacle - (2 * visualDisplacementHorizontal) + Random.Range(-visualDisplacementHorizontalRandomRange, visualDisplacementHorizontalRandomRange);
        }
        nextVisualZDisplacement = (int)zpos;
    }

    private void InitializePlayerVariables()
    {
        playerXPosition = (int)player.transform.position.x;
        playerZPosition = (int)player.transform.position.z;
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

        GameObject temp = new GameObject();
        for (int i = 0; i < numberOfCopiesOfEachObstacle * visualObstaclePrefabs.Length ; i++)
        {
            temp = Instantiate(visualObstaclePrefabs[i % visualObstaclePrefabs.Length]);
            loadedVisualObstacles.Add(temp);
            
            temp.SetActive(false);
        }

        visualPlacementZTrigger = visualDisplacementForward;
    }

    private void LoadSoundObstacle()
    {
        soundObstacle = Instantiate(soundObstaclePrefab);
        soundObstacle.SetActive(false);
        soundZScale = (int)soundObstacle.transform.localScale.z;
        nextSoundZDisplacement = soundDisplacementFromPlayer + (soundZScale / 4);
        soundObstacle.transform.position = new Vector3(Random.Range(lowerClamp, upperClamp), 0, nextSoundZDisplacement);
        soundPlacementZTrigger = nextSoundZDisplacement + (soundZScale / 2) + 100;
        soundObstacle.SetActive(true);
    }

   /* private void InitializeVisualObstacles()
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
                rotX = rotationArray[Random.Range(0, 4)];
                rotY = rotationArray[Random.Range(0, 4)];
                rotZ = rotationArray[Random.Range(0, 4)];
                visualObstacle.transform.Rotate(new Vector3(rotX, rotY, rotZ));
                visualObstacle.transform.position = new Vector3(xpos, 0, zpos);
                xpos += visualDisplacementHorizontal;

            }
            zpos += ((5 - (i+1)) / 5.0f) * visualDisplacementForwardInitial;
            xpos = xPosVisualObstacle - (2 * visualDisplacementHorizontal);
        }
        nextVisualZDisplacement = (int)zpos;
    } */

    void Update()
    {
        DeleteVisualObstacles();

        UpdatePlayer();

        UpdateVisualObstacles();

        UpdatePlane();
        
        UpdateSoundObstacle();
    }

    private void UpdatePlayer()
    {
        playerXPosition = (int)player.transform.position.x;
        playerZPosition = (int)player.transform.position.z;
    }

#region Plane stuff 
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
   #endregion

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
                visualPlacementZTrigger += (int)visualDisplacementForward;
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
            pick = Random.Range(0, loadedVisualObstacles.Count - 1);
            visualObstacle = loadedVisualObstacles[pick];
            visualObstacle.SetActive(true);
            loadedVisualObstacles.RemoveAt(pick);
            visualObstacles.Add(visualObstacle);
            rotX = Random.Range(-45, 45);
            rotY = Random.Range(-45, 45);
            rotZ = Random.Range(-45, 45);
            visualObstacle.transform.Rotate(new Vector3(rotX, rotY, rotZ));
            visualObstacle.transform.position = new Vector3(xpos, Random.Range(-yDisplacement, yDisplacement), nextVisualZDisplacement);
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
            Vector3 newPosition = new Vector3(xpos, 0, nextVisualZDisplacement + (visualDisplacementForward * i));
            if ((Physics.OverlapBox(newPosition, Vector3.one).Length == 0))
            {
                pick = Random.Range(0, loadedVisualObstacles.Count - 1);
                GameObject visualObstacle = loadedVisualObstacles[pick];
                visualObstacle.SetActive(true);
                loadedVisualObstacles.RemoveAt(pick);
                visualObstacles.Add(visualObstacle);
                rotX = Random.Range(-45, 45);
                rotY = Random.Range(-45, 45);
                rotZ = Random.Range(-45, 45);
                visualObstacle.transform.Rotate(new Vector3(rotX, rotY, rotZ));
                visualObstacle.transform.position = new Vector3(xpos, Random.Range(-yDisplacement, yDisplacement), nextVisualZDisplacement + (visualDisplacementHorizontal * i));
            }
        }
    }

    private void DeleteVisualObstacles()
    {
        Transform visualObstacle;
        for (int i = 0; i < visualObstacles.Count; i++)
        {
            visualObstacle = visualObstacles[i].transform;
            if (visualObstacle.position.z + 5000.0f < playerZPosition)
            {
                loadedVisualObstacles.Add(visualObstacle.gameObject);
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
            soundPlacementZTrigger = nextSoundZDisplacement + (soundZScale / 2) + 100;
            GenerateSoundObstacle(nextSoundZDisplacement);
        }
    }

    private void GenerateSoundObstacle(float zPosition)
    {
        soundObstacle.SetActive(true);
        soundObstacle.GetComponentInChildren<PickUpScript>().pickedUp = false;
        soundObstacle.transform.position = new Vector3(Random.Range(lowerClamp, upperClamp), 0, zPosition);
    }

}