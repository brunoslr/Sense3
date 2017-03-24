using UnityEngine;

public interface IObjectPoolController
{
    GameObject GetObjectAtIndexPrefab(int prefabIndex, bool allowGrowth);

    void PoolObject(GameObject obj);

    GameObject ReturnNewInstanceOf(GameObject prefab);

    void SetObjectInstancePropertiesForPooling(GameObject obj);
}
