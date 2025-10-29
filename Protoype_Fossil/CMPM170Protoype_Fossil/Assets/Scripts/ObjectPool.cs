using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;

    [System.Serializable]
    public class PoolItem
    {
        public GameObject prefab;
        public int amountToPool;
    }

    public List<PoolItem> itemsToPool;
    private List<GameObject> pooledObjects;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        foreach (var item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = Instantiate(item.prefab);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(GameObject prefab = null)
    {
        // If prefab specified, return matching object
        if (prefab != null)
        {
            foreach (var obj in pooledObjects)
            {
                if (!obj.activeInHierarchy && obj.name.Contains(prefab.name))
                    return obj;
            }
        }
        else // Return any available
        {
            foreach (var obj in pooledObjects)
            {
                if (!obj.activeInHierarchy)
                    return obj;
            }
        }

        return null;
    }
}