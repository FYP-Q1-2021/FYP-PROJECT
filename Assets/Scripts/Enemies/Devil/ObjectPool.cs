using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPool : MonoBehaviour
{
    public GameObject objectToPool;
    public int amountToPool;
    public List<GameObject> pooledObjects = new List<GameObject>();
    public Transform holder;

    public event Action OnObjectsPoolingFinished;

    void Start()
    {
        GameObject go;
        for (int i = 0; i < amountToPool; ++i)
        {
            go = Instantiate(objectToPool);
            go.transform.parent = holder;
            go.SetActive(false);
            pooledObjects.Add(go);
        }

        OnObjectsPoolingFinished?.Invoke();
    }
    
    public GameObject GetPooledObject()
    {
        for(int i = 0; i < amountToPool; ++i)
        {
            if(!pooledObjects[i].activeInHierarchy)
            {
                pooledObjects[i].SetActive(true);
                return pooledObjects[i];
            }
        }
        return null;
    }

    public GameObject GetPooledObjectWithoutActivating()
    {
        for (int i = 0; i < amountToPool; ++i)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}
