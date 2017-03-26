using UnityEngine;

public class ObjectPool : MonoBehaviour, IObjectPoolController
{
    public static ObjectPool instance;
    
    //Controller object that keeps all ObjectPool logic
    public ObjectPoolController controller;

    /// <summary>
    /// The object prefabs which the pool can handle.
    /// </summary>
    public GameObject[] objectPrefabs;

    /// <summary>
    /// The amount of objects of each type to buffer.
    /// </summary>
    public int[] amountToBuffer;

    public int defaultBufferAmount = 3;

    /// <summary>
    /// The container object that we will keep unused pooled objects so we dont clog up the editor with objects.
    /// </summary>
    protected GameObject poolContainer;


    private void OnEnable()
    {
        controller.SetObjectPoolController(this);
        controller.SetObjectPrefabs(objectPrefabs);
    }

    void Awake()
    {

        // First we check if there are any other instances conflicting
        if (instance != null && instance != this)
        {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
        }

        // Here we save our singleton instance
        instance = this;

    }

    void Start()
    {
        poolContainer = new GameObject("ObjectPool");
        controller.InitializeObjectPool();
    }

    #region IObjectPoolController

    /// <summary>
    /// Gets a new objectby the prefabIndex provided.  If no object type exists or if onlypooled is true and there is no objects of that type in the pool
    /// then null will be returned.
    /// </summary>
    /// <returns>
    /// The object for type.
    /// </returns>
    /// <param name='prefabName'>
    /// Object type.
    /// </param>
    /// <param name='allowGrowth'>
    /// If true, it will only return an object if there is one currently pooled.
    /// </param>
    public GameObject GetObjectAtIndexPrefab(int index, bool allowGrowth)
    {
        GameObject pooledObject = controller.GetObjectAtIndexPrefab(index, allowGrowth);
        pooledObject.SetActive(true);

        return pooledObject;
    }

    public void PoolObject(GameObject obj)
    {
        controller.PoolObject(obj);
    }

    /// <summary>
    /// Set Object as not Active and change the Parent Transform as the pool container
    /// </summary>
    /// <param name="obj"></param>
    public void SetObjectInstancePropertiesForPooling(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.parent = poolContainer.transform;
    }

    /// <summary>
    /// Creates a new Instance of a Given Prefab
    /// </summary>
    /// <param name="objectPrefab"></param>
    /// <returns></returns>
    public GameObject ReturnNewInstanceOf(GameObject objectPrefab)
    {
        GameObject newObj = Instantiate(objectPrefab) as GameObject;
        newObj.name = objectPrefab.name;
        return newObj;
    }

    #endregion

    internal int GetObjectsPrefabLength()
    {
        return objectPrefabs.Length;
    }


}
