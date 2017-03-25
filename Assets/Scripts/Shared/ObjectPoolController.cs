using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectPoolController
{

    private IObjectPoolController objectPoolController;

    private GameObject[] objectPrefabs;

    /// <summary>
    /// The pooled objects currently available.
    /// </summary>
    public List<GameObject>[] pooledObjects;

    public int[] amountToBuffer = new int[0];

    public int defaultBufferAmount = 3;
    
    public void SetObjectPoolController(IObjectPoolController objectPoolController)
    {
        this.objectPoolController = objectPoolController;
    }
    
    public void SetObjectPrefabs(GameObject[] objectPrefabs)
    {
       this.objectPrefabs = objectPrefabs;
    }

    public void InitializeObjectPool()
    {
        pooledObjects = new List<GameObject>[objectPrefabs.Length];

        int i = 0;
        foreach (GameObject objectPrefab in objectPrefabs)
        {
            pooledObjects[i] = new List<GameObject>();

            int bufferAmount;

            if (i < amountToBuffer.Length) bufferAmount = amountToBuffer[i];
            else
                bufferAmount = defaultBufferAmount;

            for (int n = 0; n < bufferAmount; n++)
            {
                PoolObject(objectPoolController.ReturnNewInstanceOf(objectPrefab));
            }
            i++;
        }
    }


    public GameObject GetObjectAtIndexPrefab(int prefabIndex, bool allowGrowth)
    {
        if (prefabIndex >= objectPrefabs.Length)
        {
            Debug.LogError("Tryed to Instantiate prefab at Index" + prefabIndex + "objects prefab size: " + objectPrefabs.Length);
            return null;
        }
        GameObject prefab = objectPrefabs[prefabIndex];

        if (pooledObjects[prefabIndex].Count > 0)
        {
            GameObject pooledObject = pooledObjects[prefabIndex][0];
            pooledObjects[prefabIndex].RemoveAt(0);

            return pooledObject;

        }
        else if (allowGrowth)
        {
            return objectPoolController.ReturnNewInstanceOf(prefab);
        }

        return null;
    }

    public void PoolObject(GameObject obj)
    {
        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            if (objectPrefabs[i].name == obj.name)
            {
                pooledObjects[i].Add(obj);
                objectPoolController.SetObjectInstancePropertiesForPooling(obj);

                return;
            }
        }
    }
}