using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
class FloorPlaneGrid:MonoBehaviour
{

    public static FloorPlaneGrid instance;

    public GameObject planePrefab;
    private GameObject[,] planes;
    private GameObject planesContainer;

    private float sizeOfPlaneX;
    private float sizeOfPlaneZ;



    void Awake()
    {
        instance = this;
    }

    #region PlaneMovement

    internal void SetupPlaneGrid()
    {
        int columns = 3;
        int rows = 3;

        planes = new GameObject[columns, rows];
        planesContainer = new GameObject("Planes");
        //(GameObject)Instantiate(TestMeteor, new Vector3(i, j, 0), Quaternion.identity);

        sizeOfPlaneX = planePrefab.GetComponent<Renderer>().bounds.size.x;
        sizeOfPlaneZ = planePrefab.GetComponent<Renderer>().bounds.size.z;
        // Vector3 planePosition;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                planes[i, j] = InstantiatePlane(planePrefab,
                        new Vector3((float)sizeOfPlaneX * (i - 1), -1, (float)sizeOfPlaneZ * ((j - 1) * -1)),
                        Quaternion.identity);
                planes[i, j].name = j + "," + i;
                planes[i, j].transform.parent = planesContainer.transform;

            }
        }
    }

    internal void UpdatePlaneOnPlayerPosition(int playerXPosition, int playerZPosition)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                float planeXMin = planes[i, j].transform.position.x - (sizeOfPlaneX / 2);
                float planeXMax = planeXMin + sizeOfPlaneX;
                float planeZMin = planes[i, j].transform.position.z - (sizeOfPlaneZ / 2);
                float planeZMax = planeZMin + sizeOfPlaneZ;
                if (playerXPosition > planeXMin && playerXPosition < planeXMax && playerZPosition > planeZMin && playerZPosition < planeZMax)
                {
                    UpdatePlaneGrid(i, j);
                }
            }
        }
    }


    private void UpdatePlaneGrid(int x, int z)
    {
        if (x == 0)
        {
            MovePlaneLeft();
        }
        else if (x == 2)
        {
            MovePlaneRight();
        }
        if (z == 0)
        {
            MovePlaneAhead();
        }
    }

    private void MovePlaneLeft()
    {

        for (int i = 2; i > 0; i--)
        {
            int j = (i - 1) % 3;
            planes[i, 0].transform.position = planes[j, 0].transform.position;
            planes[i, 1].transform.position = planes[j, 1].transform.position;
            planes[i, 2].transform.position = planes[j, 2].transform.position;

        }

        for (int i = 0; i < 3; i++)
        {
            Vector3 addPos = new Vector3(-sizeOfPlaneX, 0, 0);
            planes[0, i].transform.position += addPos;

        }
    }

    private void MovePlaneRight()
    {

        for (int i = 0; i < 2; i++)
        {
            int j = (i + 1) % 3;
            planes[i, 0].transform.position = planes[j, 0].transform.position;
            planes[i, 1].transform.position = planes[j, 1].transform.position;
            planes[i, 2].transform.position = planes[j, 2].transform.position;

        }

        for (int i = 0; i < 3; i++)
        {
            Vector3 addPos = new Vector3(sizeOfPlaneX, 0, 0);
            planes[2, i].transform.position += addPos;

        }

    }

    private void MovePlaneAhead()
    {


        for (int i = 2; i > 0; i--)
        {
            int j = (i - 1) % 3;
            planes[0, i].transform.position = planes[0, j].transform.position;
            planes[1, i].transform.position = planes[1, j].transform.position;
            planes[2, i].transform.position = planes[2, j].transform.position;

        }

        for (int i = 0; i < 3; i++)
        {
            Vector3 addPos = new Vector3(0, 0, sizeOfPlaneZ);
            planes[i, 0].transform.position += addPos;

        }

    }

    /// <summary>
    /// Creates a new Instance of a Prefab at a Given Position
    /// </summary>
    /// <param name="objectPrefab"></param>
    /// <returns></returns>
    public GameObject InstantiatePlane(GameObject planePrefab, Vector3 position, Quaternion rotation)
    {  
        GameObject newObj = Instantiate(planePrefab,position, rotation) as GameObject;
        return newObj;
    }


    #endregion


}

