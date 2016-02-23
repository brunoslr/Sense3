using UnityEngine;
using System.Collections.Generic;

public class InfinitePlaneGenerator : MonoBehaviour
{

    public GameObject planePrefab; 
    public GameObject player;

    //Visual obstacle requirements
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
    private float zOffsetBetweenSoundObstacle;

    private GameObject tactileObstacle;
    private float zOffsetBetweenTactileObstacle;

    private int lowerClamp;
    private int upperClamp;

    private int nextZ;

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
        sizeOfPlaneX = planePrefab.GetComponent<Renderer>().bounds.size.x;
        sizeOfPlaneZ = planePrefab.GetComponent<Renderer>().bounds.size.z;

        for (int i = 0; i < 3; i++)
        {
            for (int c = 0; c < 3; c++)
            {
                planes[i, c] = GameObject.Instantiate(planePrefab) as GameObject;
                planes[i, c].name = i + "," + c;
            }
        }

        // Reposition all of the planes.
        for (int i = 0; i < 3; i++)
        {
            for (int c = 0; c < 3; c++)
            {
                // Adjust the x, z positions.
                Vector3 tempPos = new Vector3((float)sizeOfPlaneX * (i - 1), -5, (float)sizeOfPlaneZ * ((c - 1) * -1));
                planes[i, c].transform.position = tempPos;
            }
        }
    }

    private void LoadVisualObstacles()
    {
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

        zPosVisualObstacle = (int)zOffsetVisual;
        nextZ = (int)(zOffsetVisual - (zOffsetVisual / 2));
    }

    private void LoadSoundObstacle()
    {
        zOffsetBetweenSoundObstacle = zOffsetSound;
        soundObstacle = Instantiate(soundObstaclePrefab);
        soundObstacle.SetActive(false);
    }

    private void LoadTactileObstacle()
    {
        zOffsetBetweenTactileObstacle = zOffsetTactile;
        tactileObstacle = Instantiate(tactileObstaclePrefab);
        tactileObstacle.SetActive(false);
    }

    private void InitializeVisualObstacles()
    {
        int pick;
        float xpos = xPosVisualObstacle - (2 * xOffsetVisual);
        float zpos = zPosVisualObstacle;
        for (int i = 0; i < 5; ++i)
        {
            for (int j = 0; j < 5; j++)
            {
                pick = Random.Range(0, loadedVisualObstacles.Count - 1);
                visualObstacle = loadedVisualObstacles[pick];
                visualObstacle.SetActive(true);
                loadedVisualObstacles.RemoveAt(pick);
                visualObstacles.Add(visualObstacle);
                visualObstacle.transform.position = new Vector3(xpos, yOffsetVisual, zpos);
                xpos += xOffsetVisual;

            }
            zpos += zOffsetVisual;
            xpos = xPosVisualObstacle - (2 * xOffsetVisual);
        }
        zPosVisualObstacle = (int)(zpos - zOffsetVisual);
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

        UpdateTactileObstacle();
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
            xPosVisualObstacle -= (int)xOffsetVisual;
            upperClamp = lowerClamp;
            lowerClamp -= 2 * xClamp;
            CreateNewPuzzleOnSide(-1.0f);
        }

        if ((int)playerXPosition > upperClamp)
        {
            xPosVisualObstacle += (int)xOffsetVisual;
            lowerClamp = upperClamp;
            upperClamp += 2 * xClamp;
            CreateNewPuzzleOnSide(1.0f);
        }

        if (playerZPosition > nextZ)
        {
            nextZ += (int)zOffsetVisual;
            zPosVisualObstacle += (int)zOffsetVisual;
            GenerateVisualObstacles();
        }
    }

    private void GenerateVisualObstacles()
    {
        int pick;
        float xpos = xPosVisualObstacle - (2 * xOffsetVisual);
        for (int i = 0; i < 5; ++i)
        {
            pick = Random.Range(0, loadedVisualObstacles.Count);
            visualObstacle = loadedVisualObstacles[pick];
            visualObstacle.SetActive(true);
            loadedVisualObstacles.RemoveAt(pick);
            visualObstacles.Add(visualObstacle);
            visualObstacle.transform.position = new Vector3(xpos, yOffsetVisual, zPosVisualObstacle);
            xpos += xOffsetVisual;
        }
    }

    private void CreateNewPuzzleOnSide(float dir)
    {
        float xpos = xPosVisualObstacle + (2 * xOffsetVisual * dir);
        for (int i = -5; i < 1; i++)
        {
            Vector3 newPosition = new Vector3(xpos, yOffsetVisual, zPosVisualObstacle + (zOffsetVisual * i));
            if ((Physics.OverlapBox(newPosition, Vector3.one * ((xOffsetVisual / 2.0f) - 20.0f))).Length == 0)
            {
                int pick = Random.Range(0, loadedVisualObstacles.Count);
                GameObject visualObstacle = loadedVisualObstacles[pick];
                visualObstacle.SetActive(true);
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
                visualObstacle.Rotate(Vector3.zero);
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
}
